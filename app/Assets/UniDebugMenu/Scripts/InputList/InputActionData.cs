using System;

namespace UniDebugMenu
{
	/// <summary>
	/// InputField のバリデーションタイプ
	/// </summary>
	public enum InputValidateType
	{
		NONE	,
		INTEGER	,
	}

	/// <summary>
	/// 入力欄付きのボタンのデータを管理するクラス
	/// </summary>
	public sealed class InputActionData
	{
		//==============================================================================
		// 変数(readonly)
		//==============================================================================
		public readonly InputValidateType		m_validateType	;
		public readonly Action<string, Action>	m_onClick		;

		//==============================================================================
		// 関数
		//==============================================================================
		public InputActionData
		(
			InputValidateType		validateType	,
			Action<string, Action>	onClick
		)
		{
			m_validateType	= validateType	;
			m_onClick		= onClick		;
		}

		public InputActionData
		(
			Action<string, Action>	onClick
		) : this ( InputValidateType.NONE, onClick )
		{
		}

		public InputActionData
		(
			InputValidateType	validateType	,
			Action<string>		onClick
		)
		{
			m_validateType	= validateType;
			m_onClick		= ( str, onEnded ) =>
			{
				onClick?.Invoke( str );
				onEnded?.Invoke();
			};
		}

		public InputActionData
		(
			Action<string>	onClick
		) : this ( InputValidateType.NONE, onClick )
		{
		}
	}
}