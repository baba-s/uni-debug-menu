using System;

namespace UniDebugMenu
{
	/// <summary>
	/// 入力欄付きのリストの要素の UI のデータを管理するクラス
	/// </summary>
	public sealed class CommandData
	{
		//==============================================================================
		// 変数(readonly)
		//==============================================================================
		public readonly Func<string>		m_getText			;
		public readonly InputActionData		m_inputActionData	;
		public readonly ToggleActionData	m_toggleActionData	;
		public readonly ActionData[]		m_actionDataList	;
		public readonly bool				m_isBorder			;

		//==============================================================================
		// プロパティ
		//==============================================================================
		public bool IsLeft { get; set; }

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// 入力欄と複数の実行ボタンを指定するコンストラクタ
		/// </summary>
		public CommandData
		(
			Func<string>			getText			,
			InputActionData			inputActionData	,
			params ActionData[]		actionDataList
		)
		{
			m_getText			= getText			;
			m_inputActionData	= inputActionData	;
			m_actionDataList	= actionDataList	;
		}

		/// <summary>
		/// 複数の実行ボタンを指定するコンストラクタ
		/// </summary>
		public CommandData
		(
			Func<string>			getText			,
			params ActionData[]		actionDataList
		) : this ( getText, null, actionDataList )
		{
		}

		/// <summary>
		/// 区切り線を指定するコンストラクタ
		/// </summary>
		public CommandData
		(
			Func<string>			getText
		) : this ( getText, null, new ActionData[ 0 ] )
		{
			m_isBorder = true;
		}

		/// <summary>
		/// チェックボックスを指定するコンストラクタ
		/// </summary>
		public CommandData
		(
			Func<string>			getText			,
			ToggleActionData		toggleActionData
		)
		{
			m_getText			= getText				;
			m_toggleActionData	= toggleActionData		;
			m_actionDataList	= new ActionData[ 0 ]	;
		}
	}
}