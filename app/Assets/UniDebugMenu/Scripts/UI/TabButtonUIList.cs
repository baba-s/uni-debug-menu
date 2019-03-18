using System;
using System.Linq;
using UnityEngine;

namespace UniDebugMenu.Internal
{
	/// <summary>
	/// タブボタンの UI のリストを管理するクラス
	/// </summary>
	[DisallowMultipleComponent]
	public sealed class TabButtonUIList : MonoBehaviour
	{
		//==============================================================================
		// 変数(SerializeField)
		//==============================================================================
		[SerializeField] private TabButtonUI[] m_uiList = null;

		//==============================================================================
		// イベント
		//==============================================================================
		public Action<int> mOnClick;

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// リセットされる時に呼び出されます
		/// </summary>
		private void Reset()
		{
			m_uiList = GetComponentsInChildren<TabButtonUI>();
		}

		/// <summary>
		/// 初期化される時に呼び出されます
		/// </summary>
		private void Awake()
		{
			for ( int i = 0; i < m_uiList.Length; i++ )
			{
				var ui		= m_uiList[ i ];
				var index	= i;

				ui.mOnClick = () => mOnClick?.Invoke( index );
			}
		}

		/// <summary>
		/// 表示を設定します
		/// </summary>
		public void SetDisp( int tabIndex, params string[] list )
		{
			for ( int i = 0; i < m_uiList.Length; i++ )
			{
				var ui			= m_uiList[ i ];
				var text		= list.ElementAtOrDefault( i );
				var isActive	= !string.IsNullOrWhiteSpace( text );
				var isSelected	= i == tabIndex;

				ui.SetActive( isActive );
				if ( !isActive ) continue;
				ui.SetDisp( isSelected, text );
			}
		}
	}
}