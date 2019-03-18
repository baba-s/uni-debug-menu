using System;
using System.Collections;
using System.Text;

namespace UniDebugMenu.Example
{
	/// <summary>
	/// 可変型の文字列を管理するクラス
	/// </summary>
	public class StringAppender : IEnumerable
	{
		//==============================================================================
		// 変数
		//==============================================================================
		private readonly StringBuilder mBuilder = new StringBuilder();

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// 指定された文字列を追加します
		/// </summary>
		public void Add( string value )
		{
			mBuilder.AppendLine( value );
		}

		/// <summary>
		/// 指定された文字列を追加します
		/// </summary>
		public void Add( string format, params object[] args )
		{
			mBuilder.AppendFormat( format, args ).AppendLine();
		}

		/// <summary>
		/// 指定された文字列を追加します
		/// </summary>
		public void Add( string format, object arg0 )
		{
			mBuilder.AppendFormat( format, arg0 ).AppendLine();
		}

		/// <summary>
		/// 指定された文字列を追加します
		/// </summary>
		public void Add( string format, object arg0, object arg1 )
		{
			mBuilder.AppendFormat( format, arg0, arg1 ).AppendLine();
		}

		/// <summary>
		/// 指定された文字列を追加します
		/// </summary>
		public void Add( string format, object arg0, object arg1, object arg2 )
		{
			mBuilder.AppendFormat( format, arg0, arg1, arg2 ).AppendLine();
		}
		/// <summary>
		/// 指定された文字列を追加します
		/// </summary>
		public void Add( bool condition, string value )
		{
			if ( !condition )
			{
				return;
			}
			Add( value );
		}

		/// <summary>
		/// 指定された文字列を追加します
		/// </summary>
		public void Add( bool condition, string format, params object[] args )
		{
			if ( !condition )
			{
				return;
			}
			Add( format, args );
		}

		/// <summary>
		/// 指定された文字列を追加します
		/// </summary>
		public void Add( bool condition, string format, object arg0 )
		{
			if ( !condition )
			{
				return;
			}
			Add( format, arg0 );
		}

		/// <summary>
		/// 指定された文字列を追加します
		/// </summary>
		public void Add( bool condition, string format, object arg0, object arg1 )
		{
			if ( !condition )
			{
				return;
			}
			Add( format, arg0, arg1 );
		}

		/// <summary>
		/// 指定された文字列を追加します
		/// </summary>
		public void Add( bool condition, string format, object arg0, object arg1, object arg2 )
		{
			if ( !condition )
			{
				return;
			}
			Add( format, arg0, arg1, arg2 );
		}

		/// <summary>
		/// 文字列を返します
		/// </summary>
		public override string ToString()
		{
			return mBuilder.ToString().TrimEnd();
		}

		/// <summary>
		/// コレクションを反復処理する列挙子を返します
		/// </summary>
		public IEnumerator GetEnumerator()
		{
			throw new NotImplementedException();
		}
	}
}