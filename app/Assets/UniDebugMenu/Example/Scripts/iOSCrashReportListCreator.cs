using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UniDebugMenu.Example
{
	/// <summary>
	/// iOS のクラッシュ情報を表示するクラス
	/// </summary>
	[Serializable]
	public sealed class iOSCrashReportListCreator : ListCreatorBase<ActionData>
	{
		//==============================================================================
		// 変数
		//==============================================================================
		private IList<ActionData> m_list;

		//==============================================================================
		// プロパティ
		//==============================================================================
		public override int Count => m_list.Count;

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// リストの表示に使用するデータを作成します
		/// </summary>
		protected override void DoCreate( ListCreateData data )
		{
			m_list = ToText()
				.Split( '\n' )
				.Where( c => data.IsMatch( c ) )
				.Select( c => new ActionData( c ) )
				.ToArray()
				.ReverseIf( data.IsReverse )
			;
		}

		/// <summary>
		/// 指定されたインデックスの要素の表示に使用するデータを返します
		/// </summary>
		protected override ActionData DoGetElemData( int index ) => m_list.ElementAtOrDefault( index );

		/// <summary>
		/// テキストを整形して返します
		/// </summary>
		private static string ToText()
		{
			var reports = CrashReport.reports;
			var texts = reports.Select( c => $"{c.time.ToString()}: {c.text}" );
			var result = string.Join( "\n", texts );

			return result;
		}
	}
}