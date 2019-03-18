using System;
using UnityEngine;
using UnityEngine.UI;

namespace UniDebugMenu.Internal
{
	/// <summary>
	/// トグルボタンを管理するクラス
	/// </summary>
	[DisallowMultipleComponent]
	public sealed class ToggleButtonUI : MonoBehaviour
	{
		//==============================================================================
		// 変数(SerializeField)
		//==============================================================================
		[SerializeField] private Toggle	m_toggleUI	= null;

		//==============================================================================
		// デリゲート
		//==============================================================================
		public Action mOnComplete { private get; set; }

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// リセットされる時に呼び出されます
		/// </summary>
		private void Reset()
		{
			m_toggleUI = GetComponentInChildren<Toggle>();
		}

		/// <summary>
		/// 表示を設定します
		/// </summary>
		public void SetDisp( ToggleActionData data )
		{
			if ( data == null ) return;

			// 値を設定した時にイベントが発行されないようにするために
			// 一旦イベントをすべて解除してから値を設定しています
			var onValueChanged = m_toggleUI.onValueChanged;
			m_toggleUI.onValueChanged = new Toggle.ToggleEvent();
			m_toggleUI.isOn = data.m_getter.Invoke();
			m_toggleUI.onValueChanged = onValueChanged;

			m_toggleUI.onValueChanged.SetListener( isOn =>
			{
				// ボタンが押されたら指定されたアクションを実行します
				// 指定されたアクションが完了までに時間がかかる場合は
				// そのアクションが完了してから完了通知を投げます
				data.m_setter?.Invoke( isOn, () => mOnComplete?.Invoke() );
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