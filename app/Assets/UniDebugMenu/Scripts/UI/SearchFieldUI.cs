using System;
using UnityEngine;
using UnityEngine.UI;

namespace UniDebugMenu.Internal
{
	/// <summary>
	/// 検索欄の UI を管理するクラス
	/// </summary>
	[DisallowMultipleComponent]
	public sealed class SearchFieldUI : MonoBehaviour, ISearchFieldUI
	{
		//==============================================================================
		// 変数(SerializeField)
		//==============================================================================
		[SerializeField] private InputField		m_inputFieldUI		= null;
		[SerializeField] private Button			m_searchButtonUI	= null;
		[SerializeField] private Button			m_deleteButtonUI	= null;

		//==============================================================================
		// イベント
		//==============================================================================
		public Action mOnClick;

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// 初期化される時に呼び出されます
		/// </summary>
		private void Awake()
		{
			m_searchButtonUI.onClick.SetListener( () => mOnClick?.Invoke() );
			m_deleteButtonUI.onClick.SetListener( () =>
			{
				m_inputFieldUI.text = string.Empty;
				mOnClick?.Invoke();
			} );
		}

		/// <summary>
		/// 指定された文字列が入力された文字列にマッチする場合 true を返します
		/// </summary>
		public bool IsMatch( string text )
		{
			return string.IsNullOrWhiteSpace( m_inputFieldUI.text ) || text.Contains( m_inputFieldUI.text );
		}

		/// <summary>
		/// 指定されたいずれかの文字列が入力された文字列にマッチする場合 true を返します
		/// </summary>
		public bool IsMatch( params string[] texts )
		{
			return Array.Exists( texts, c => IsMatch( c ) );
		}
	}
}