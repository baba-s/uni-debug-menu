using System;

namespace UniDebugMenu
{
	/// <summary>
	/// チェックボックスで使用するデータを管理するクラス
	/// </summary>
	public sealed class ToggleActionData
	{
		//==============================================================================
		// 変数(readonly)
		//==============================================================================
		public readonly Func<bool>				m_getter	;
		public readonly Action<bool, Action>	m_setter	;

		//==============================================================================
		// 関数
		//==============================================================================
		public ToggleActionData
		(
			Func<bool>				getter	,
			Action<bool, Action>	setter
		)
		{
			m_getter	= getter	;
			m_setter	= setter;
		}

		public ToggleActionData
		(
			Func<bool>		getter	,
			Action<bool>	setter
		)
		{
			m_getter	= getter	;
			m_setter	= ( isOn, onEnded ) =>
			{
				setter?.Invoke( isOn );
				onEnded?.Invoke();
			};
		}
	}
}