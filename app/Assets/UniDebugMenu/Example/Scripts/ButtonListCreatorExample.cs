using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UniDebugMenu.Example
{
	/// <summary>
	/// ボタンのリストを作成するサンプルクラス
	/// </summary>
	[Serializable]
	public sealed class ButtonListCreatorExample : ListCreatorBase<ActionData>
	{
		//==============================================================================
		// 変数(readonly)
		//==============================================================================
		private readonly List<ActionData> m_sourceList = new List<ActionData>();

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
		public ButtonListCreatorExample()
		{
			for ( int i = 0; i < 9999; i++ )
			{
				var text	= ( i + 1 ).ToString( "0000" );
				var data	= new ActionData( text, () => Debug.Log( text ) ) ;

				m_sourceList.Add( data );
			}
		}

		/// <summary>
		/// リストの表示に使用するデータを作成します
		/// </summary>
		protected override void DoCreate( ListCreateData data )
		{
			m_list = m_sourceList
				.Where( c => data.IsMatch( c.m_text ) )	// 検索にヒットしたかどうか
				.ToArray()
				.ReverseIf( data.IsReverse )	// 逆順にするかどうか
			;
		}

		/// <summary>
		/// 指定されたインデックスの要素の表示に使用するデータを返します
		/// </summary>
		protected override ActionData DoGetElemData( int index ) => m_list.ElementAtOrDefault( index );
	}
}