using UniDebugMenu.Internal;
using System;
using UnityEngine;

namespace UniDebugMenu
{
	/// <summary>
	/// リストの表示に使用するデータを作成する基底クラス
	/// </summary>
	[Serializable]
	public abstract class ListCreatorBase<T> : IListCreator<T>
	{
		//==============================================================================
		// 変数
		//==============================================================================
		private UniDebugMenuScene	m_debugMenuScene	;
		private GameObject			m_gameObject		;

		//==============================================================================
		// プロパティ
		//==============================================================================
		/// <summary>
		/// デバッグメニューの画面を取得または設定します
		/// </summary>
		protected DebugMenuUIBase target { get; private set; }

		/// <summary>
		/// 要素数を返します
		/// </summary>
		public abstract int Count { get; }

		/// <summary>
		/// 要素が存在しない場合 true を返します
		/// </summary>
		public bool IsEmpty => Count <= 0;

		/// <summary>
		/// トースト表示する場合 true を返します
		/// </summary>
		public virtual bool IsShowToast => false;

		/// <summary>
		/// トースト表示しない場合 true を返します
		/// </summary>
		public bool IsNotShowToast => !IsShowToast;

		/// <summary>
		/// タブ名の配列を返します
		/// </summary>
		public virtual string[] TabNameList => new string[ 0 ];

		/// <summary>
		/// オプションボタンの挙動を返します
		/// </summary>
		public virtual ActionData[] OptionActionList => new ActionData[ 0 ];

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// リストの表示に使用するデータを作成します
		/// </summary>
		public void Create( ListCreateData data )
		{
			m_debugMenuScene	= data.DebugMenuScene	;
			m_gameObject		= data.GameObject		;
			target				= data.Target			;

			DoCreate( data );
		}

		/// <summary>
		/// リストの表示に使用するデータを作成する処理を派生クラスで記述します
		/// </summary>
		protected abstract void DoCreate( ListCreateData data );

		/// <summary>
		/// 指定されたインデックスの要素の表示に使用するデータを返します
		/// </summary>
		public T GetElemData( int index ) => DoGetElemData( index );

		/// <summary>
		/// 指定されたインデックスの要素の表示に使用するデータを返す処理を派生クラスで記述します
		/// </summary>
		protected abstract T DoGetElemData( int index );

		/// <summary>
		/// 表示を更新します
		/// </summary>
		public void UpdateDisp() => target.UpdateDisp();

		/// <summary>
		/// 表示を更新します
		/// </summary>
		public void Refresh() => target.Refresh();

		/// <summary>
		/// 指定された種類のデバッグメニューを加算して開きます
		/// </summary>
		protected void OpenAdd<TData>( DMType type, ListCreatorBase<TData> creator )
		{
			m_debugMenuScene.OpenAdd( type, creator, m_gameObject );
		}
	}
}