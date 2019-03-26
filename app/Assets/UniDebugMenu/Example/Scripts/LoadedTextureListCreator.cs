using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

namespace UniDebugMenu.Example
{
	/// <summary>
	/// 読み込み済みの UnityEngine.Object の一覧を表示するクラス
	/// </summary>
	[Serializable]
	public sealed class LoadedObjectListCreator<T> : ListCreatorBase<ActionData> where T : UnityEngine.Object
	{
		//==============================================================================
		// クラス
		//==============================================================================
		private sealed class LoadedData
		{
			public readonly long m_size;
			public readonly string m_text;

			public LoadedData( T obj )
			{
				m_size = Profiler.GetRuntimeMemorySizeLong( obj );
				var memory = ( m_size >> 10 ) / 1024f;
				m_text = $"{memory:0.00} MB    {obj.name}";
			}
		}

		//==============================================================================
		// 変数
		//==============================================================================
		private IList<ActionData> m_list;

		//==============================================================================
		// プロパティ
		//==============================================================================
		public override int Count => m_list.Count;

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// リストの表示に使用するデータを作成します
		/// </summary>
		protected override void DoCreate( ListCreateData data )
		{
			m_list = Resources
				.FindObjectsOfTypeAll<T>()
				.Where( c => ( c.hideFlags & HideFlags.NotEditable ) == 0 )
				.Where( c => ( c.hideFlags & HideFlags.HideAndDontSave ) == 0 )
				.Select( c => new LoadedData( c ) )
				.Where( c => data.IsMatch( c.m_text ) )
				.OrderByDescending( c => c.m_size )
				.Select( c => new ActionData( c.m_text ) )
				.ToArray()
				.ReverseIf( data.IsReverse )
			;
		}

		/// <summary>
		/// 指定されたインデックスの要素の表示に使用するデータを返します
		/// </summary>
		protected override ActionData DoGetElemData( int index ) => m_list.ElementAtOrDefault( index );
	}
}