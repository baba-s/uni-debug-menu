using UnityEngine;
using UnityEngine.SceneManagement;

namespace UniDebugMenu.Example
{
	/// <summary>
	/// デバッグメニューのサンプルを管理するクラス
	/// </summary>
	public sealed class UniDebugMenuExampleScene : MonoBehaviour
	{
		//==============================================================================
		// 変数
		//==============================================================================
		private ExampleTopListCreator m_creator;

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// 初期化される時に呼び出されます
		/// </summary>
		private void Awake()
		{
			// デバッグメニューのトップ画面のリストを管理するインスタンスを作成します
			m_creator = new ExampleTopListCreator();
			m_creator.Init();

			// デバッグメニューのシーンを加算で読み込みます
			SceneManager.LoadScene( "UniDebugMenuScene", LoadSceneMode.Additive );

			// デバッグメニューでコマンドが実行された時に呼び出されます
			UniDebugMenuScene.mOnChange += () => Debug.Log( "UniDebugMenuScene.mOnChange" );

			// DM ボタンが押された時にデバッグメニューのトップ画面を開くように設定します
			UniDebugMenuScene.mOnOpen += () => DMType.BUTTON_COL_3.Open( m_creator );
		}

		/// <summary>
		/// デバッグメニューを開くボタンが押された
		/// </summary>
		public void OnClick1()
		{
			// デバッグメニューのトップ画面を開きます
			DMType.BUTTON_COL_3.Open( m_creator );
		}

		/// <summary>
		/// システムコマンドの画面を開くボタンが押された
		/// </summary>
		public void OnClick2()
		{
			// デバッグメニューのシステムコマンドの画面を開きます
			DMType.COMMAND_TAB_6.Open( new SystemCommandListCreator() );
		}

		/// <summary>
		/// デバッグメニューを開く DM ボタンを透明にするボタンが押された
		/// </summary>
		public void OnClick3()
		{
			// DM ボタンを透明にします
			// DM ボタンは透明になるだけで当たり判定は有効のままになります
			// true：不透明　false：透明
			UniDebugMenuScene.SetOpenButtonVisible( false );
		}

		/// <summary>
		/// デバッグメニューを削除するボタンが押された
		/// </summary>
		public void OnClick4()
		{
			// デバッグメニューのゲームオブジェクトを削除します
			if ( UniDebugMenuScene.Destroy() )
			{
				// デバッグメニューのゲームオブジェクトが削除できた場合は
				// デバッグメニューのシーンもアンロードします
				SceneManager.UnloadSceneAsync( "UniDebugMenuScene" );
			}
		}
	}
}