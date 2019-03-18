using System;
using UnityEngine.Events;

namespace UniDebugMenu.Internal
{
	/// <summary>
	/// UnityEvent 型の拡張メソッドを管理するクラス
	/// </summary>
	public static class UnityEventExt
	{
		/// <summary>
		/// 登録済みのイベントリスナーをすべて解除してから指定されたリスナーのみ追加します
		/// </summary>
		public static void SetListener( this UnityEvent self, Action call )
		{
			self.RemoveAllListeners();
			self.AddListener( () => call() );
		}

		/// <summary>
		/// 登録済みのイベントリスナーをすべて解除してから指定されたリスナーのみ追加します
		/// </summary>
		public static void SetListener<T>( this UnityEvent<T> self, Action<T> call )
		{
			self.RemoveAllListeners();
			self.AddListener( value => call( value ) );
		}
	}
}