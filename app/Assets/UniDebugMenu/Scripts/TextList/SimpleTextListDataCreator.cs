using UniDebugMenu.Example;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UniDebugMenu
{
	/// <summary>
	/// テキストのリストにシンプルな文字列を表示するクラス
	/// </summary>
	[Serializable]
	public class SimpleTextListDataCreator : ListCreatorBase<ActionData>
	{
		//==============================================================================
		// 変数(SerializeField)
		//==============================================================================
		private readonly IList<ActionData> m_sourceList;

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
		/// 作成して返します
		/// </summary>
		public SimpleTextListDataCreator( string text )
		{
			if ( string.IsNullOrWhiteSpace( text ) )
			{
				m_sourceList = new ActionData[ 0 ];
				return;
			}

			m_sourceList = text
				.Split( '\n' )
				.SelectMany( c => c.SubstringAtCount( 80 ) )
				.Select( c => new ActionData( c ) )
				.ToArray()
			;
		}

		/// <summary>
		/// リストの表示に使用するデータを作成します
		/// </summary>
		protected override void DoCreate( ListCreateData data )
		{
			m_list = m_sourceList
				.Where( c => data.IsMatch( c.m_text ) )
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