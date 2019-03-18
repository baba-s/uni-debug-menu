using System;

namespace UniDebugMenu.Internal
{
	/// <summary>
	/// デバッグメニューの各画面の UI のインターフェイス
	/// </summary>
	public interface IDebugMenuUI
	{
		/// <summary>
		/// 戻る時に呼び出されます
		/// </summary>
		Action mOnBack { set; }
	}
}