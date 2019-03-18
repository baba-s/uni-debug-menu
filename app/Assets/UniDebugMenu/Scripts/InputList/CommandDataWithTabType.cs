using System;

namespace UniDebugMenu
{
	/// <summary>
	/// コマンドのリストの UI のデータを管理するクラス
	/// </summary>
	public sealed class CommandDataWithTabType<T>
	{
		//==============================================================================
		// 変数(readonly)
		//==============================================================================
		public readonly T			m_tabType	;
		public readonly CommandData	m_data		;

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// 入力欄と複数の実行ボタンを指定するコンストラクタ
		/// </summary>
		public CommandDataWithTabType
		(
			T						tabType			,
			Func<string>			getText			,
			InputActionData			inputActionData	,
			params ActionData[]		actionDataList
		)
		{
			m_tabType	= tabType;
			m_data		= new CommandData( getText, inputActionData, actionDataList );
		}

		/// <summary>
		/// 複数の実行ボタンを指定するコンストラクタ
		/// </summary>
		public CommandDataWithTabType
		(
			T						tabType			,
			Func<string>			getText			,
			params ActionData[]		actionDataList
		)
		{
			m_tabType	= tabType;
			m_data		= new CommandData( getText, actionDataList );
		}

		/// <summary>
		/// 区切り線を指定するコンストラクタ
		/// </summary>
		public CommandDataWithTabType
		(
			T						tabType			,
			Func<string>			getText
		)
		{
			m_tabType	= tabType;
			m_data		= new CommandData( getText );
		}

		/// <summary>
		/// チェックボックスを指定するコンストラクタ
		/// </summary>
		public CommandDataWithTabType
		(
			T						tabType			,
			Func<string>			getText			,
			ToggleActionData		toggleActionData
		)
		{
			m_tabType	= tabType;
			m_data		= new CommandData( getText, toggleActionData );
		}
	}

}