using System;
using System.Collections.Generic;
using System.Linq;

namespace UniDebugMenu.Example
{
	/// <summary>
	/// プロファイラ情報を表示するクラス
	/// </summary>
	[Serializable]
	public sealed class ProfilerInfoListCreator : ListCreatorBase<ActionData>
	{
		//==============================================================================
		// 変数(readonly)
		//==============================================================================
		private readonly MonoMemoryChecker m_monoMemoryChecker = new MonoMemoryChecker();
		private readonly UnityMemoryChecker m_unityMemoryChecker = new UnityMemoryChecker();

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
		private string ToText()
		{
			m_monoMemoryChecker.Update();
			m_unityMemoryChecker.Update();

			var appender = new StringAppender
			{
				{ "<b>Mono</b>" },
				{ "" },
				{ $"    Used: {m_monoMemoryChecker.UsedText}" },
				{ $"    Total: {m_monoMemoryChecker.TotalText}" },
				{ "" },
				{ "<b>Unity</b>" },
				{ "" },
				{ $"    Used: {m_unityMemoryChecker.UsedText}" },
				{ $"    Unused: {m_unityMemoryChecker.UnusedText}" },
				{ $"    Total: {m_unityMemoryChecker.TotalText}" },
				{ "" },
			};

			return appender.ToString();
		}
	}
}