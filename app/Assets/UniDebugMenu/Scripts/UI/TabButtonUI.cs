using System;
using UnityEngine;
using UnityEngine.UI;

namespace UniDebugMenu.Internal
{
	/// <summary>
	/// タブボタンの UI を管理するクラス
	/// </summary>
	[DisallowMultipleComponent]
	public sealed class TabButtonUI : MonoBehaviour
	{
		//==============================================================================
		// 変数(SerializeField)
		//==============================================================================
		[SerializeField] private Button	m_buttonUI	= null;
		[SerializeField] private Image	m_imageUI	= null;
		[SerializeField] private Text	m_textUI	= null;

		//==============================================================================
		// イベント
		//==============================================================================
		public Action mOnClick { set { m_buttonUI.onClick.SetListener( value ); } }

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// リセットされる時に呼び出されます
		/// </summary>
		private void Reset()
		{
			m_buttonUI	= GetComponent<Button>();
			m_imageUI	= GetComponent<Image>();
			m_textUI	= GetComponentInChildren<Text>();
		}

		/// <summary>
		/// 表示を設定します
		/// </summary>
		public void SetDisp( bool isSelected, string text )
		{
			var color = isSelected ? Color.yellow : Color.white;

			m_imageUI.color = color;
			m_textUI.text = text;
		}

		/// <summary>
		/// アクティブかどうかを設定します
		/// </summary>
		public void SetActive( bool isActive )
		{
			gameObject.SetActive( isActive );
		}
	}
}