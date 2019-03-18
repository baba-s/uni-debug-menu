using System;
using UnityEngine;

namespace UniDebugMenu.Internal
{
	/// <summary>
	/// 制限付き無限スクロールを管理するクラス
	/// </summary>
	[RequireComponent( typeof( InfiniteScroll ) )]
	public class ItemControllerLimited : MonoBehaviour
	{
		//==============================================================================
		// 変数
		//==============================================================================
		private InfiniteScroll	m_infiniteScroll	;
		private RectTransform	m_rectTransform		;
		private bool			m_isCache			;

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// キャッシュします
		/// </summary>
		private void Cache()
		{
			if ( m_isCache ) return;
			m_isCache = true;

			m_infiniteScroll	= GetComponent<InfiniteScroll>();
			m_rectTransform		= GetComponent<RectTransform>();
		}

		/// <summary>
		/// 初期化される時に呼び出されます
		/// </summary>
		public void Init( int count, Action<int, GameObject> onUpdate )
		{
			Cache();

			m_infiniteScroll.Init( onUpdate );

			var delta = m_rectTransform.sizeDelta;
			delta.y = m_infiniteScroll.itemScale * count;
			m_rectTransform.sizeDelta = delta;
		}

		/// <summary>
		/// 表示を更新します
		/// </summary>
		public void Refresh()
		{
			Cache();

			m_infiniteScroll.Refresh();
		}
	}
}