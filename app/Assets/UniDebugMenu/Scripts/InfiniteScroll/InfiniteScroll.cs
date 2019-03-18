using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UniDebugMenu.Internal
{
	/// <summary>
	/// 無限スクロールの UI を管理するクラス
	/// </summary>
	public class InfiniteScroll : MonoBehaviour
	{
		//==============================================================================
		// 列挙型
		//==============================================================================
		public enum Direction
		{
			Vertical,
			Horizontal,
		}

		//==============================================================================
		// 変数
		//==============================================================================
		[SerializeField] private RectTransform	m_original	= null;
		[SerializeField] private Direction		m_direction	= Direction.Vertical;

		//==============================================================================
		// 変数(readonly)
		//==============================================================================
		private readonly LinkedList<RectTransform> m_itemList = new LinkedList<RectTransform>();

		//==============================================================================
		// 変数
		//==============================================================================
		private int						m_count					;
		private float					m_diffPreFramePosition	;
		private int						m_currentIndex			;
		private RectTransform			m_rectTransform			;
		private float?					m_itemScale				;
		private Action<int, GameObject>	m_onUpdate				;

		//==============================================================================
		// プロパティ
		//==============================================================================
		public bool isHorizontal	=> m_direction == Direction.Horizontal	;
		public bool isVertical		=> m_direction == Direction.Vertical	;

		private float anchoredPosition => m_direction == Direction.Vertical ?
			-rectTransform.anchoredPosition.y :
			 rectTransform.anchoredPosition.x
		;

		private RectTransform rectTransform
		{
			get
			{
				if ( m_rectTransform == null )
				{
					m_rectTransform = GetComponent<RectTransform>();
				}
				return m_rectTransform;
			}
		}

		public float itemScale
		{
			get
			{
				if ( m_original != null && m_itemScale == null )
				{
					m_itemScale = isVertical ?
						m_original.sizeDelta.y :
						m_original.sizeDelta.x
					;
				}
				return m_itemScale.Value;
			}
		}

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// 初期化される時に呼び出されます
		/// </summary>
		public void Init( Action<int, GameObject> onUpdate )
		{
			var scrollRect = GetComponentInParent<ScrollRect>();

			var rect	= scrollRect.GetComponent<RectTransform>().rect;
			var size	= isVertical ? rect.height : rect.width;
			var count	= Mathf.FloorToInt( size / itemScale );

			m_count = count + 3;

			transform.DetachChildren();

			foreach ( var n in m_itemList )
			{
				Destroy( n.gameObject );
			}

			m_itemList.Clear();

			m_diffPreFramePosition	= 0;
			m_currentIndex			= 0;
			m_onUpdate				= onUpdate;

			scrollRect.horizontal					= isHorizontal	;
			scrollRect.vertical						= isVertical	;
			scrollRect.content						= rectTransform	;
			scrollRect.horizontalNormalizedPosition	= 0				;
			scrollRect.verticalNormalizedPosition	= 1				;

			m_original.gameObject.SetActive(false);

			for ( int i = 0; i < m_count; i++ )
			{
				var item = Instantiate( m_original, transform );
				item.name = i.ToString();
				item.anchoredPosition = isVertical ?
					new Vector2( 0, -itemScale * i ) :
					new Vector2( itemScale * i, 0 )
				;
				item.gameObject.SetActive( true );

				m_itemList.AddLast( item );

				m_onUpdate?.Invoke( i, item.gameObject );
			}
		}

		/// <summary>
		/// 更新される時に呼び出されます
		/// </summary>
		private void Update()
		{
			if ( m_itemList.First == null ) return;

			while ( anchoredPosition - m_diffPreFramePosition < -itemScale * 2 )
			{
				m_diffPreFramePosition -= itemScale;

				var item = m_itemList.First.Value;
				m_itemList.RemoveFirst();
				m_itemList.AddLast( item );

				var pos = itemScale * m_count + itemScale * m_currentIndex;
				item.anchoredPosition = isVertical ?
					new Vector2( 0, -pos ) :
					new Vector2( pos, 0 )
				;

				m_onUpdate?.Invoke( m_currentIndex + m_count, item.gameObject );

				m_currentIndex++;
			}

			while ( anchoredPosition - m_diffPreFramePosition > 0 )
			{
				m_diffPreFramePosition += itemScale;

				var item = m_itemList.Last.Value;
				m_itemList.RemoveLast();
				m_itemList.AddFirst( item );

				m_currentIndex--;

				var pos = itemScale * m_currentIndex;
				item.anchoredPosition = isVertical ?
					new Vector2( 0, -pos ) :
					new Vector2( pos, 0 )
				;
				m_onUpdate?.Invoke( m_currentIndex, item.gameObject );
			}
		}

		/// <summary>
		/// 表示を更新します
		/// </summary>
		public void Refresh()
		{
			var index = m_currentIndex;

			foreach ( var n in m_itemList )
			{
				m_onUpdate?.Invoke( index, n.gameObject );
				index++;
			}
		}
	}
}