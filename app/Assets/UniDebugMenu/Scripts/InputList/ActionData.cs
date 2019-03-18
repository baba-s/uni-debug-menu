using System;

namespace UniDebugMenu
{
	/// <summary>
	/// ボタンの表示名とクリックイベントを管理するクラス
	/// </summary>
	public sealed class ActionData
	{
		//==============================================================================
		// 変数(readonly)
		//==============================================================================
		public readonly string			m_text		= string.Empty;
		public readonly Action<Action>	m_onClick	= null;

		//==============================================================================
		// 関数
		//==============================================================================
		public ActionData() { }

		public ActionData
		(
			string			text	,
			Action<Action>	onClick
		)
		{
			m_text		= text		;
			m_onClick	= onClick	;
		}

		public ActionData
		(
			string	text	,
			Action	onClick
		)
		{
			m_text		= text		;
			m_onClick	= onEnded =>
			{
				onClick?.Invoke();
				onEnded?.Invoke();
			};
		}

		public ActionData
		(
			string	text
		)
		{
			m_text = text;
			m_onClick = null;
		}
	}
}