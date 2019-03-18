using System;
using UnityEngine;

namespace UniDebugMenu.Internal
{
	/// <summary>
	/// 使い回しスクロールの UI を管理するクラス
	/// </summary>
	[Serializable]
	public abstract class LoopListViewUI<TData, TComponent> where TComponent : Component, IScrollItemUI<TData>
	{
		//==============================================================================
		// 変数(SerializeField)
		//==============================================================================
		[SerializeField] private ItemControllerLimited	m_view			= null	;
		[SerializeField] private int					m_countPerRow	= 1		;

		//==============================================================================
		// 変数
		//==============================================================================
		private IListCreator<TData> m_creator;

		//==============================================================================
		// デリゲート
		//==============================================================================
		public Action<TData, int> mOnComplete { private get; set; }

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// 表示を設定します
		/// </summary>
		public void SetDisp( IListCreator<TData> creator )
		{
			m_creator = creator;

			var listCount = creator.Count;

			var count = listCount / m_countPerRow;
			if ( 0 < listCount % m_countPerRow )
			{
				count++;
			}

			m_view.Init( count, OnUpdate );
		}

		/// <summary>
		/// スクロール内の要素を更新する時に呼び出されます
		/// </summary>
		private void OnUpdate( int index, GameObject gameObject )
		{
			var children = gameObject.GetComponentsInChildren<TComponent>( true );

			for ( int i = 0; i < m_countPerRow; i++ )
			{
				int itemIndex   = index * m_countPerRow + i;
				var child       = children[ i ];
				var childObj    = child.gameObject;

				if ( itemIndex < 0 || m_creator.Count <= itemIndex )
				{
					childObj.SetActive( false );
				}
				else
				{
					var data = m_creator.GetElemData( itemIndex );
					childObj.SetActive( true );
					child.mOnComplete = elemIndex =>
					{
						child.SetDisp( data );
						mOnComplete?.Invoke( data, elemIndex );
					};
					child.SetDisp( data );
				}
			}
		}

		/// <summary>
		/// 表示を更新します
		/// </summary>
		public void Refresh()
		{
			m_view.Refresh();
		}
	}
}