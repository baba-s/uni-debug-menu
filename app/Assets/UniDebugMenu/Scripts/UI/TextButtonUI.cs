using System;
using UnityEngine;
using UnityEngine.UI;

namespace UniDebugMenu.Internal
{
	/// <summary>
	/// 汎用的なテキスト付きのボタンを管理するクラス
	/// </summary>
	[DisallowMultipleComponent]
	public sealed class TextButtonUI : MonoBehaviour, IScrollItemUI<ActionData>
	{
		//==============================================================================
		// 変数(SerializeField)
		//==============================================================================
		[SerializeField] private Button	m_buttonUI	= null;
		[SerializeField] private Text	m_textUI	= null;

		//==============================================================================
		// デリゲート
		//==============================================================================
		public Action<int> mOnComplete { private get; set; }

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// リセットされる時に呼び出されます
		/// </summary>
		private void Reset()
		{
			m_buttonUI	= GetComponent<Button>();
			m_textUI	= GetComponentInChildren<Text>();
		}

		/// <summary>
		/// 表示を設定します
		/// </summary>
		public void SetDisp( ActionData data )
		{
			var text = data.m_text;

			m_textUI.text = text;
			m_buttonUI.onClick.SetListener( () =>
			{
				// ボタンが押されたら指定されたアクションを実行します
				// 指定されたアクションが完了までに時間がかかる場合は
				// そのアクションが完了してから完了通知を投げます
				data.m_onClick?.Invoke( () => mOnComplete?.Invoke( 0 ) );
			} );
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