using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UniDebugMenu.Internal
{
	/// <summary>
	/// デバッグ用のトースト表示の UI を管理するクラス
	/// </summary>
	[DisallowMultipleComponent]
	public sealed partial class DebugToastUI : MonoBehaviour
	{
		//==============================================================================
		// 変数(SerializeField)
		//==============================================================================
		[SerializeField] private CanvasGroup	m_canvasGroup	= null	;
		[SerializeField] private RectTransform	m_baseUI		= null	;
		[SerializeField] private Text			m_textUI		= null	;
		[SerializeField] private float			m_padding		= 0		;
		[SerializeField] private float			m_duration		= 0		;

		//==============================================================================
		// 変数
		//==============================================================================
		private bool		m_isInit	;
		private IEnumerator	m_coroutine	;

		//==============================================================================
		// 変数(static)
		//==============================================================================
		private static DebugToastUI m_instance;

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// リセットされる時に呼び出されます
		/// </summary>
		private void Reset()
		{
			m_canvasGroup	= GetComponentInChildren<CanvasGroup>();
			m_textUI		= GetComponentInChildren<Text>();
		}

		/// <summary>
		/// 初期化される時に呼び出されます
		/// </summary>
		private void Awake()
		{
			Init();
		}

		/// <summary>
		/// 初期化します
		/// </summary>
		private void Init()
		{
			if ( m_isInit ) return;
			m_isInit = true;

			m_canvasGroup.alpha = 0;
		}

		/// <summary>
		/// 表示を設定します
		/// </summary>
		public void DoOpen( string text )
		{
			if ( m_coroutine != null )
			{
				StopCoroutine( m_coroutine );
				m_coroutine = null;
			}

			var textWithoutNewLine = text
				.Replace( "\n", " " )
				.Replace( "\r", " " )
			;

			m_coroutine = OnUpdate( textWithoutNewLine );
			StartCoroutine( m_coroutine );
		}

		/// <summary>
		/// 表示を更新します
		/// </summary>
		private IEnumerator OnUpdate( string text )
		{
			m_canvasGroup.alpha = 0;
			m_textUI.text = text;

			yield return null;

			var size = m_baseUI.sizeDelta;
			size.x = m_textUI.preferredWidth + m_padding;
			m_baseUI.sizeDelta = size;

			m_canvasGroup.alpha = 1;

			yield return new WaitForSecondsRealtime( m_duration );

			m_canvasGroup.alpha = 0;
		}

		//==============================================================================
		// 関数(static)
		//==============================================================================
		/// <summary>
		/// インスタンスを設定します
		/// </summary>
		public static void SetInstance( DebugToastUI instance )
		{
			m_instance = instance;
		}

		/// <summary>
		/// 開きます
		/// </summary>
		public static void Open( string text )
		{
			m_instance?.DoOpen( text );
		}
	}
}