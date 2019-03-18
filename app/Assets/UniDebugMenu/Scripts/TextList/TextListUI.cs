using System;
using UnityEngine;
using UnityEngine.UI;

namespace UniDebugMenu.Internal
{
	/// <summary>
	/// テキストのリストの UI のを管理するクラス
	/// </summary>
	[DisallowMultipleComponent]
	public sealed class TextListUI : DebugMenuUIBase
	{
		//==============================================================================
		// クラス
		//==============================================================================
		[Serializable] public sealed class LoopListViewUI : LoopListViewUI<ActionData, TextListElemUI> { }

		//==============================================================================
		// 変数(SerializeField)
		//==============================================================================
		[SerializeField] private Button				m_updateButtonUI		= null	;
		[SerializeField] private SortButtonUI		m_sortButtonUI			= null	;
		[SerializeField] private SearchFieldUI		m_searchFieldUI			= null	;
		[SerializeField] private TabButtonUIList	m_tabButtonUIList		= null	;
		[SerializeField] private OptionButtonUIList	m_optionButtonUIList	= null	;
		[SerializeField] private LoopListViewUI		m_view					= null	;
		[SerializeField] private GameObject			m_emptyTextUI			= null	;

		//==============================================================================
		// 変数(readonly)
		//==============================================================================
		private readonly Selectable<int>	m_selectedTabIndex	= new Selectable<int>()	;	// 選択中のタブのインデックス
		private readonly SelectableBool		m_isSort			= new SelectableBool()	;	// 並び替えする場合 true

		//==============================================================================
		// 変数
		//==============================================================================
		private IListCreator<ActionData>	m_creator	;
		private bool						m_isInit	;

		//==============================================================================
		// プロパティ
		//==============================================================================
		protected override object Creator => m_creator;

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// 初期化される時に呼び出されます
		/// </summary>
		protected override void DoAwake()
		{
			m_updateButtonUI.onClick.SetListener( UpdateDisp );

			m_sortButtonUI		.mOnClick = m_isSort.Not;
			m_tabButtonUIList	.mOnClick = m_selectedTabIndex.SetValueIfNotEqual;

			m_searchFieldUI		.mOnClick = UpdateDisp;
			m_selectedTabIndex	.mChanged = UpdateDisp;
			m_isSort			.mChanged = UpdateDisp;
		}

		/// <summary>
		/// 表示を設定します
		/// </summary>
		protected override void DoSetDisp( object creator )
		{
			var tabIndex	= m_selectedTabIndex.Value;
			var isReverse	= m_isSort.Value;

			var data = new ListCreateData
			(
				debugMenuScene	: DebugMenuScene	,
				gameObject		: gameObject		,
				target			: this				,
				searchFieldUI	: m_searchFieldUI	,
				tabIndex		: tabIndex			,
				isReverse		: isReverse
			);

			m_creator = creator as IListCreator<ActionData>;
			m_creator.Create( data );

			m_sortButtonUI			.SetDisp( isReverse );
			m_tabButtonUIList		.SetDisp( tabIndex, m_creator.TabNameList );
			m_optionButtonUIList	.SetDisp( m_creator.OptionActionList );
			m_view					.SetDisp( m_creator );
			m_emptyTextUI			.SetActive( m_creator.IsEmpty );

			m_optionButtonUIList.mOnComplete = optionData => OpenToastUI( $"{optionData.m_text} 完了" );

			m_view.mOnComplete = ( elemData, elemIndex ) => OpenToastUI( $"{elemData.m_text} 完了" );
		}

		/// <summary>
		/// トーストを表示します
		/// </summary>
		private void OpenToastUI( string message )
		{
			if ( m_creator.IsNotShowToast ) return;

			DebugToastUI.Open( message );
			DebugMenuScene.OnChange();
		}

		/// <summary>
		/// 表示を更新します
		/// </summary>
		protected override void DoRefresh()
		{
			m_view.Refresh();
		}
	}
}