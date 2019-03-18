namespace UniDebugMenu.Internal
{
	/// <summary>
	/// リストの表示に使用するデータを作成するインターフェイス
	/// </summary>
	public interface IListCreator<T>
	{
		//==============================================================================
		// プロパティ
		//==============================================================================
		/// <summary>
		/// 要素数を返します
		/// </summary>
		int Count { get; }

		/// <summary>
		/// 要素が存在しない場合 true を返します
		/// </summary>
		bool IsEmpty { get; }

		/// <summary>
		/// トースト表示する場合 true を返します
		/// </summary>
		bool IsShowToast { get; }

		/// <summary>
		/// トースト表示しない場合 true を返します
		/// </summary>
		bool IsNotShowToast { get; }

		/// <summary>
		/// タブ名の配列を返します
		/// </summary>
		string[] TabNameList { get; }

		/// <summary>
		/// オプションボタンの挙動を返します
		/// </summary>
		ActionData[] OptionActionList { get; }

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// リストの表示に使用するデータを作成します
		/// </summary>
		void Create( ListCreateData data );

		/// <summary>
		/// 指定されたインデックスの要素の表示に使用するデータを返します
		/// </summary>
		T GetElemData( int index );
	}
}