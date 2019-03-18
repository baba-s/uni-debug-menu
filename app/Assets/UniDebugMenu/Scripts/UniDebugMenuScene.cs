using System;
using UniDebugMenu.Internal;
using UnityEngine;
using UnityEngine.UI;

namespace UniDebugMenu
{
	/// <summary>
	/// デバッグメニューの画面タイプ
	/// </summary>
	public enum DMType
	{
		TEXT_TAB_6		,	// テキスト表示 ( タブ  6個 )
		TEXT_TAB_12		,	// テキスト表示 ( タブ 12個 )
		BUTTON_COL_2	,	// ボタン表示 ( 2列 )
		BUTTON_COL_3	,	// ボタン表示 ( 3列 )
		COMMAND_TAB_6	,	// コマンド表示 ( タブ  6個 )
		COMMAND_TAB_12	,	// コマンド表示 ( タブ 12個 )
	}

	/// <summary>
	/// DMType 型の拡張メソッドを管理するクラス
	/// </summary>
	public static class DMTypeExt
	{
		/// <summary>
		/// 指定された種類のデバッグメニューを開きます
		/// </summary>
		public static void Open<T>( this DMType self, ListCreatorBase<T> creator )
		{
			UniDebugMenuScene.Open( self, creator );
		}
	}

	/// <summary>
	/// デバッグメニュー画面を管理するクラス
	/// </summary>
	[DisallowMultipleComponent]
	public sealed class UniDebugMenuScene : MonoBehaviour
	{
		//==============================================================================
		// 変数(SerializeField)
		//==============================================================================
		[SerializeField] private GameObject			m_openBaseUI			= null;
		[SerializeField] private DragableButtonUI	m_openButtonUI			= null;
		[SerializeField] private CanvasGroup		m_closeBaseUI			= null;
		[SerializeField] private Button				m_closeButtonUI			= null;
		[SerializeField] private Transform			m_instantiateRoot		= null;
		[SerializeField] private TextListUI			m_textListUI_Tab6		= null;
		[SerializeField] private TextListUI			m_textListUI_Tab12		= null;
		[SerializeField] private ButtonListUI		m_buttonListUI_Col2		= null;
		[SerializeField] private ButtonListUI		m_buttonListUI_Col3		= null;
		[SerializeField] private CommandListUI		m_commandListUI_Tab6	= null;
		[SerializeField] private CommandListUI		m_commandListUI_Tab12	= null;
		[SerializeField] private DebugToastUI		m_debugToastUI			= null;

		//==============================================================================
		// 変数(readonly)
		//==============================================================================

		//==============================================================================
		// 変数
		//==============================================================================
		private bool m_isOpen;	// 開いている場合 true

		//==============================================================================
		// 変数(static)
		//==============================================================================
		private static UniDebugMenuScene m_instance;

		//==============================================================================
		// プロパティ
		//==============================================================================

		//==============================================================================
		// イベント(static)
		//==============================================================================
		public static event Action	mOnOpen		;
		public static event Action	mOnChange	;

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// 破棄される時に呼び出されます
		/// </summary>
		private void OnDestroy()
		{
			if ( m_instance == this )
			{
				m_instance	= null;
				mOnOpen		= null;
				mOnChange	= null;

				DebugToastUI.SetInstance( null );
			}
		}

		/// <summary>
		/// 初期化される時に呼び出されます
		/// </summary>
		private void Awake()
		{
			if ( m_instance != null )
			{
				Destroy( gameObject );
				return;
			}

			m_instance = this;

			DontDestroyOnLoad( gameObject );

			DebugToastUI.SetInstance( m_debugToastUI );

			m_openButtonUI	.onClick.SetListener( () => mOnOpen?.Invoke() );
			m_closeButtonUI	.onClick.SetListener( () => Close() );

			// 作業用にアクティブにしたままでも正常に動作するように
			m_textListUI_Tab6	.gameObject.SetActive( false );
			m_textListUI_Tab12	.gameObject.SetActive( false );
			m_buttonListUI_Col2	.gameObject.SetActive( false );
			m_buttonListUI_Col3	.gameObject.SetActive( false );
			m_commandListUI_Tab6	.gameObject.SetActive( false );
			m_commandListUI_Tab12	.gameObject.SetActive( false );

			// 閉じた状態にします
			m_openBaseUI.SetActive( false );
		}

		/// <summary>
		/// 指定された種類のデバッグメニューをルートとして開きます
		/// </summary>
		private void DoOpen<T>( DMType type, ListCreatorBase<T> creator )
		{
			if ( m_isOpen ) return;
			m_isOpen = true;

			m_openBaseUI.SetActive( true );

			OpenImpl( type, creator, isActive =>
			{
				if ( !isActive ) return;
				Close();
			} );
		}

		/// <summary>
		/// 指定された種類のデバッグメニューを加算して開きます
		/// </summary>
		public void OpenAdd<T>
		(
			DMType				type		,
			ListCreatorBase<T>	creator		,
			GameObject			gameObject
		)
		{
			OpenImpl( type, creator, gameObject.SetActive );
		}

		/// <summary>
		/// 指定された種類のデバッグメニューを開きます
		/// </summary>
		private void OpenImpl<T>
		(
			DMType				type		,
			ListCreatorBase<T>	creator		,
			Action<bool>		onSetActive
		)
		{
			// 表示するコンテンツの複製元を取得します
			var original = GetContent( type );

			// 表示するコンテンツを複製して表示を設定します
			var obj = Instantiate( original, m_instantiateRoot );
			obj.DebugMenuScene = this;
			obj.gameObject.SetActive( true );
			obj.SetDisp( creator );

			// 現在表示されているコンテンツを非表示にします
			onSetActive?.Invoke( false );

			// 戻るボタンが押されたら追加したコンテンツを削除して
			// 最初に表示していたコンテンツを表示します
			obj.mOnBack = () =>
			{
				Destroy( obj.gameObject );
				onSetActive?.Invoke( true );
			};
		}

		/// <summary>
		/// 指定されたコンテンツタイプに紐づくオブジェクトを返します
		/// </summary>
		private DebugMenuUIBase GetContent( DMType type )
		{
			switch ( type )
			{
				case DMType.TEXT_TAB_6		: return m_textListUI_Tab6		;
				case DMType.TEXT_TAB_12		: return m_textListUI_Tab12		;
				case DMType.BUTTON_COL_2	: return m_buttonListUI_Col2	;
				case DMType.BUTTON_COL_3	: return m_buttonListUI_Col3	;
				case DMType.COMMAND_TAB_6	: return m_commandListUI_Tab6	;
				case DMType.COMMAND_TAB_12	: return m_commandListUI_Tab12	;
			}
			return null;
		}

		/// <summary>
		/// 閉じます
		/// </summary>
		private void Close()
		{
			if ( !m_isOpen ) return;
			m_isOpen = false;

			foreach ( Transform child in m_instantiateRoot )
			{
				Destroy( child.gameObject );
			}

			m_openBaseUI.SetActive( false );
		}

		/// <summary>
		/// 変更があったことを通知します
		/// </summary>
		public void OnChange()
		{
			mOnChange?.Invoke();
		}

		/// <summary>
		/// 開くボタンを透明にするか不透明にするかを設定します
		/// </summary>
		private void DoSetOpenButtonVisible( bool isVisible )
		{
			m_closeBaseUI.alpha = isVisible ? 1 : 0;
		}

		//==============================================================================
		// 関数(static)
		//==============================================================================
		/// <summary>
		/// 指定された種類のデバッグメニューをルートとして開きます
		/// </summary>
		public static void Open<T>( DMType type, ListCreatorBase<T> creator )
		{
			m_instance.DoOpen( type, creator );
		}

		/// <summary>
		/// 開くボタンを透明にするか不透明にするかを設定します
		/// </summary>
		public static void SetOpenButtonVisible( bool isVisible )
		{
			m_instance.DoSetOpenButtonVisible( isVisible );
		}

		/// <summary>
		/// デバッグメニューを削除します。削除できた場合 true を返します
		/// </summary>
		public static bool Destroy()
		{
			if ( m_instance == null ) return false;

			Destroy( m_instance.gameObject );
			return true;
		}
	}
}