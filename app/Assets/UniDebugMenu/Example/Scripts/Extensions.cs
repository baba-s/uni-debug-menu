using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace UniDebugMenu.Example
{
	/// <summary>
	/// 拡張メソッドを管理するクラス
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// 配列内全体の要素のシーケンスを反転させます
		/// </summary>
		public static T[] ReverseIf<T>( this T[] self, bool condition )
		{
			if ( !condition )
			{
				return self;
			}
			Array.Reverse( self );
			return self;
		}

		/// <summary>
		/// すべての親オブジェクトを返します
		/// </summary>
		public static GameObject[] GetAllParent( this GameObject self )
		{
			var result = new List<GameObject>();
			for ( var parent = self.transform.parent; parent != null; parent = parent.parent )
			{
				result.Add( parent.gameObject );
			}
			return result.ToArray();
		}

		/// <summary>
		/// 指定された文字列を指定された回数分繰り返し連結した文字列を生成して返します
		/// </summary>
		public static string Repeat( this string self, int repeatCount )
		{
			var builder = new StringBuilder();
			for ( int i = 0; i < repeatCount; i++ )
			{
				builder.Append( self );
			}
			return builder.ToString();
		}

		/// <summary>
		/// 指定された文字列を指定された文字数で分割して返します
		/// </summary>
		public static string[] SubstringAtCount( this string self, int count )
		{
			var result = new List<string>();
			var length = ( int )Math.Ceiling( ( double )self.Length / count );

			for ( int i = 0; i < length; i++ )
			{
				int start = count * i;
				if ( self.Length <= start )
				{
					break;
				}
				if ( self.Length < start + count )
				{
					result.Add( self.Substring( start ) );
				}
				else
				{
					result.Add( self.Substring( start, count ) );
				}
			}

			return result.ToArray();
		}
	}
}