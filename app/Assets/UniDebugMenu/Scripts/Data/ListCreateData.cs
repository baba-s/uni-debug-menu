using UnityEngine;

namespace UniDebugMenu
{
	/// <summary>
	/// リストを作成する時に使用するデータ
	/// </summary>
	public sealed class ListCreateData
	{
		//==============================================================================
		// 変数(readonly)
		//==============================================================================
		private readonly UniDebugMenuScene	m_debugMenuScene	;
		private readonly GameObject			m_gameObject		;
		private readonly DebugMenuUIBase	m_target			;
		private readonly ISearchFieldUI		m_searchFieldUI		;
		private readonly int				m_tabIndex			;
		private readonly bool				m_isReverse			;

		//==============================================================================
		// プロパティ
		//==============================================================================
		public UniDebugMenuScene	DebugMenuScene	=> m_debugMenuScene	;
		public GameObject			GameObject		=> m_gameObject		;
		public DebugMenuUIBase		Target			=> m_target			;
		public int					TabIndex		=> m_tabIndex		;
		public bool					IsReverse		=> m_isReverse		;

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public ListCreateData
		(
			UniDebugMenuScene	debugMenuScene	,
			GameObject			gameObject		,
			DebugMenuUIBase		target			,
			ISearchFieldUI		searchFieldUI	,
			int					tabIndex		,
			bool				isReverse
		)
		{
			m_debugMenuScene	= debugMenuScene	;
			m_gameObject		= gameObject		;
			m_target			= target			;
			m_searchFieldUI		= searchFieldUI		;
			m_tabIndex			= tabIndex			;
			m_isReverse			= isReverse			;
		}

		/// <summary>
		/// 指定された文字列が入力された文字列にマッチする場合 true を返します
		/// </summary>
		public bool IsMatch( string text ) => m_searchFieldUI.IsMatch( text );

		/// <summary>
		/// 指定されたいずれかの文字列が入力された文字列にマッチする場合 true を返します
		/// </summary>
		public bool IsMatch( params string[] texts ) => m_searchFieldUI.IsMatch( texts );
	}
}