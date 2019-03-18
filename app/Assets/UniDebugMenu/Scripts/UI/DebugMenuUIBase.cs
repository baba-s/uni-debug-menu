using UniDebugMenu.Internal;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace UniDebugMenu
{
	/// <summary>
	/// デバッグメニューの各画面の UI の基底クラス
	/// </summary>
	public abstract class DebugMenuUIBase : MonoBehaviour, IDebugMenuUI
	{
        //==============================================================================
        // 変数(SerializeField)
        //==============================================================================
		[SerializeField] private Button m_backButtonUI = null;	// 戻るボタンの UI

		//==============================================================================
		// プロパティ
		//==============================================================================
		protected abstract object Creator { get; }

		public UniDebugMenuScene DebugMenuScene { protected get; set; }

		//==============================================================================
		// イベント
		//==============================================================================
		/// <summary>
		/// 戻る時に呼び出されます
		/// </summary>
		public Action mOnBack { private get; set; }

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// 初期化する時に呼び出されます
		/// </summary>
		private void Awake()
		{
			m_backButtonUI?.onClick.SetListener( () => mOnBack?.Invoke() );

			DoAwake();
		}

		/// <summary>
		/// 初期化する時に呼び出される処理を派生クラスで記述します
		/// </summary>
		protected virtual void DoAwake(){ }

		/// <summary>
		/// 表示を更新します
		/// </summary>
		public void UpdateDisp()
		{
			SetDisp( Creator );
		}

		/// <summary>
		/// 表示を設定します
		/// </summary>
		public void SetDisp( object creator ) => DoSetDisp( creator );

		/// <summary>
		/// 表示を設定する処理を派生クラスで記述します
		/// </summary>
		protected abstract void DoSetDisp( object creator );

		/// <summary>
		/// 表示を更新します
		/// </summary>
		public void Refresh() => DoRefresh();

		/// <summary>
		/// 表示を更新する処理を派生クラスで記述します
		/// </summary>
		protected abstract void DoRefresh();
	}
}