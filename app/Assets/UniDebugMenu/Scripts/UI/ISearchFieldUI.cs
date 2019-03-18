namespace UniDebugMenu
{
	/// <summary>
	/// 検索欄の UI のインターフェイス
	/// </summary>
	public interface ISearchFieldUI
	{
		/// <summary>
		/// 指定された文字列が入力された文字列にマッチする場合 true を返します
		/// </summary>
		bool IsMatch( string text );

		/// <summary>
		/// 指定されたいずれかの文字列が入力された文字列にマッチする場合 true を返します
		/// </summary>
		bool IsMatch( params string[] texts );
	}
}