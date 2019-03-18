using System;

namespace UniDebugMenu.Example
{
	/// <summary>
	/// コマンドのリストの UI のデータを作成して返すクラス
	/// </summary>
	public sealed class CommandDataCreator<T>
	{
		/// <summary>
		/// 入力欄と複数の実行ボタンを指定するコンストラクタ
		/// </summary>
		public CommandDataWithTabType<T> Create
		(
			T						tabType			,
			Func<string>			getText			,
			InputActionData			inputActionData	,
			params ActionData[]		actionDataList
		)
		{
			return new CommandDataWithTabType<T>( tabType, getText, inputActionData, actionDataList );
		}

		/// <summary>
		/// 複数の実行ボタンを指定するコンストラクタ
		/// </summary>
		public CommandDataWithTabType<T> Create
		(
			T						tabType			,
			Func<string>			getText			,
			params ActionData[]		actionDataList
		)
		{
			return new CommandDataWithTabType<T>( tabType, getText, actionDataList );
		}

		/// <summary>
		/// 区切り線を指定するコンストラクタ
		/// </summary>
		public CommandDataWithTabType<T> Create
		(
			T						tabType			,
			Func<string>			getText
		)
		{
			return new CommandDataWithTabType<T>( tabType, getText );
		}

		/// <summary>
		/// チェックボックスを指定するコンストラクタ
		/// </summary>
		public CommandDataWithTabType<T> Create
		(
			T						tabType			,
			Func<string>			getText			,
			ToggleActionData		toggleActionData
		)
		{
			return new CommandDataWithTabType<T>( tabType, getText, toggleActionData );
		}
	}
}