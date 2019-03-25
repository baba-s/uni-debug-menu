using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Diagnostics;

namespace UniDebugMenu.Example
{
	/// <summary>
	/// iOS のクラッシュ情報を表示するクラス
	/// </summary>
	[Serializable]
	public sealed class iOSCrashReportListCreator : ListCreatorBase<ActionData>
	{
		//==============================================================================
		// 定数(static readonly)
		//==============================================================================
		private static readonly string NL = Environment.NewLine;
		private static readonly string TIME_FORMAT = "yyyy/MM/dd HH:mm:ss";

		//==============================================================================
		// クラス
		//==============================================================================
		private sealed class CrashData
		{
			public readonly string m_time;
			public readonly string m_summary;
			public readonly string m_text;
			public readonly string m_listText;
			public readonly string m_detailText;

			public CrashData( CrashReport report )
			{
				m_time = report.time.ToString( TIME_FORMAT );
				m_summary = report.text.Split( new [] { NL }, StringSplitOptions.None ).FirstOrDefault();
				m_text = report.text;
				m_listText = $"{m_time}: {m_summary}";
				m_detailText = $"{m_time}{NL}{NL}{NL}{m_text}";
			}
		}

		//==============================================================================
		// 変数
		//==============================================================================
		private IList<ActionData> m_list;

		//==============================================================================
		// プロパティ
		//==============================================================================
		public override int Count => m_list.Count;

		public override ActionData[] OptionActionList => new[]
		{
			new ActionData( "テスト", () => Utils.ForceCrash( ForcedCrashCategory.AccessViolation ) ),
			new ActionData( "削除", () => { CrashReport.RemoveAll(); UpdateDisp(); } ),
		};

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// リストの表示に使用するデータを作成します
		/// </summary>
		protected override void DoCreate( ListCreateData data )
		{
			m_list = CrashReport.reports
				.Take( CrashReport.reports.Length - 1 ) // 末尾のレポートが重複していたので無視
				.OrderByDescending( c => c.time )
				.Select( c => new CrashData( c ) )
				.Where( c => data.IsMatch( c.m_time, c.m_summary ) )
				.Select( c => new ActionData( c.m_listText, () => OpenAdd( DMType.TEXT_TAB_6, new SimpleTextListDataCreator( c.m_detailText ) ) ) )
				.ToArray()
				.ReverseIf( data.IsReverse )
			;
		}

		/// <summary>
		/// 指定されたインデックスの要素の表示に使用するデータを返します
		/// </summary>
		protected override ActionData DoGetElemData( int index ) => m_list.ElementAtOrDefault( index );
	}
}