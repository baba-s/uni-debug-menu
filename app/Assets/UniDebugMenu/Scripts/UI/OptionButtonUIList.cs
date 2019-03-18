using System;
using System.Linq;
using UnityEngine;

namespace UniDebugMenu.Internal
{
	/// <summary>
	/// オプションボタンの UI のリストを管理するクラス
	/// </summary>
	[DisallowMultipleComponent]
	public sealed class OptionButtonUIList : MonoBehaviour
	{
		//==============================================================================
		// 変数(SerializeField)
		//==============================================================================
		[SerializeField] private OptionButtonUI[] m_uiList = null;

		//==============================================================================
		// デリゲート
		//==============================================================================
		public Action<ActionData> mOnComplete { private get; set; }

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// リセットされる時に呼び出されます
		/// </summary>
		private void Reset()
		{
			m_uiList = GetComponentsInChildren<OptionButtonUI>();
		}

		/// <summary>
		/// 表示を設定します
		/// </summary>
		public void SetDisp( params ActionData[] list )
		{
			for ( int i = 0; i < m_uiList.Length; i++ )
			{
				var ui			= m_uiList[ i ];
				var data		= list.ElementAtOrDefault( i );
				var isActive	= data != null;

				ui.SetActive( isActive );
				if ( !isActive ) continue;
				ui.SetDisp( data );
				ui.mOnComplete = () => mOnComplete?.Invoke( data );
			}
		}
	}
}