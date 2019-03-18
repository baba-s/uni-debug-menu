using System;

namespace UniDebugMenu.Internal
{
	/// <summary>
	/// 選択中の bool 値を管理するクラス
	/// </summary>
	public sealed class SelectableBool : Selectable<bool>
	{
		//==============================================================================
		// デリゲート
		//==============================================================================
		/// <summary>
		/// 値が true に変更された時に呼び出されます
		/// </summary>
		public Action mChangedTrue;

		/// <summary>
		/// 値が false に変更された時に呼び出されます
		/// </summary>
		public Action mChangedFalse;

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public SelectableBool() : base()
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public SelectableBool( bool value ) : base()
		{
			SetValueWithoutCallback( value );
		}

		/// <summary>
		/// 論理否定を実行します
		/// </summary>
		public void Not()
		{
			Value = !Value;
		}

		/// <summary>
		/// true になります
		/// </summary>
		public void True()
		{
			Value = true;
		}

		/// <summary>
		/// false になります
		/// </summary>
		public void False()
		{
			Value = false;
		}

		/// <summary>
		/// 値が変更された時に呼び出されます
		/// </summary>
		protected override void DoOnChanged()
		{
			if ( Value )
			{
				mChangedTrue?.Invoke();
			}
			else
			{
				mChangedFalse?.Invoke();
			}
		}
	}
}