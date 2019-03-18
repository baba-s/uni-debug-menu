using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UniDebugMenu.Example
{
	/// <summary>
	/// ゲームオブジェクトのリストを作成するクラス
	/// </summary>
	[Serializable]
	public sealed class GameObjectListCreator : ListCreatorBase<CommandData>
	{
		//==============================================================================
		// クラス
		//==============================================================================
		/// <summary>
		/// ゲームオブジェクトの情報を管理するクラス
		/// </summary>
		private sealed class GameObjectData
		{
			//==========================================================================
			// 変数(readonly)
			//==========================================================================
			public readonly int		m_index			;
			public readonly string	m_name			;
			public readonly int		m_parentCount	;

			//==========================================================================
			// 変数
			//==========================================================================
			public GameObject m_gameObject;	// 削除後に null を代入するので readonly ではない

			//==========================================================================
			// プロパティ
			//==========================================================================
			public string Text
			{
				get
				{
					var line		= ( m_index + 1 ).ToString( "0000" );
					var indent		= "  ".Repeat( m_parentCount );
					var isDestroyed	= m_gameObject == null;
					var isActive	= !isDestroyed && m_gameObject.activeInHierarchy && m_gameObject.activeSelf;
					var colorTag	= isDestroyed ? "red" : isActive ? "while" : "silver";

					return $"<color={colorTag}>{line}  {indent}{m_name}</color>";
				}
			}

			//==========================================================================
			// 関数
			//==========================================================================
			/// <summary>
			/// コンストラクタ
			/// </summary>
			public GameObjectData( int index, GameObject gameObject )
			{
				m_index			= index				;
				m_gameObject	= gameObject		;
				m_name			= gameObject.name	;
				m_parentCount	= m_gameObject.GetAllParent().Length;
			}

			/// <summary>
			/// アクティブかどうかを切り替えます
			/// </summary>
			public void ToggleActive()
			{
				m_gameObject?.SetActive( !m_gameObject.activeSelf );
			}

			/// <summary>
			/// ゲームオブジェクトを削除します
			/// </summary>
			public void Destroy()
			{
				GameObject.Destroy( m_gameObject );
				m_gameObject = null;
			}
		}

		//==============================================================================
		// 変数
		//==============================================================================
		private IList<CommandData> m_list;

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
			/*
			 * http://baba-s.hatenablog.com/entry/2019/03/13/214323
			 * ゲームオブジェクトを順番に並べるために
			 * Resources.FindObjectsOfTypeAll ではなく
			 * GameObject.FindObjectsOfType を使用しています
			 */
			m_list = GameObject
				.FindObjectsOfType<GameObject>()
				.Where( c => c.transform.parent == null )
				.SelectMany( c => c.GetComponentsInChildren<Transform>( true ) )
				.Select( c => c.gameObject )
				.Select( ( val, index ) => new GameObjectData( index, val ) )
				.Where( c => data.IsMatch( c.Text ) )
				.Select( c =>
				{
					var elemData = new CommandData
					(
						() => c.Text,
						new ActionData( "詳細", () =>
						{
							var components = c.m_gameObject
								.GetComponents<MonoBehaviour>()
								.Select( behaviour => JsonUtility.ToJson( behaviour, true ) )
							;

							var infoText = string.Join( "\n", components );

							OpenAdd( DMType.TEXT_TAB_6, new SimpleTextListDataCreator( infoText ) );
						} ),
						new ActionData( "削除", () =>
						{
							c.Destroy();
							Refresh();
						} ),
						new ActionData( "アクティブ\n切り替え", () =>
						{
							c.ToggleActive();
							Refresh();
						} )
					)
					{
						IsLeft = true,
					};
					return elemData;
				} )
				.ToArray()
				.ReverseIf( data.IsReverse )
			;
		}

		/// <summary>
		/// 指定されたインデックスの要素の表示に使用するデータを返します
		/// </summary>
		protected override CommandData DoGetElemData( int index ) => m_list.ElementAtOrDefault( index );
	}
}