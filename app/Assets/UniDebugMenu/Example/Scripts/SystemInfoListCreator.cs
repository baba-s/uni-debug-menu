using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UniDebugMenu.Example
{
	/// <summary>
	/// システム情報を表示するクラス
	/// </summary>
	[Serializable]
	public sealed class SystemInfoListCreator : ListCreatorBase<ActionData>
	{
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
			m_list = ToText()
				.Split( '\n' )
				.Where( c => data.IsMatch( c ) )
				.Select( c => new ActionData( c ) )
				.ToArray()
				.ReverseIf( data.IsReverse )
			;
		}

		/// <summary>
		/// 指定されたインデックスの要素の表示に使用するデータを返します
		/// </summary>
		protected override ActionData DoGetElemData( int index ) => m_list.ElementAtOrDefault( index );

		/// <summary>
		/// テキストを整形して返します
		/// </summary>
		private static string ToText()
		{
#if ENABLE_IL2CPP
            const string IL2CPP = "True";
#else
            const string IL2CPP = "False";
#endif
            var activeScene		= SceneManager.GetActiveScene();
			var application		= ToProperty( new Application()		);
			var debug			= ToProperty( new Debug()			);
			var sceneManager	= ToProperty( new SceneManager()	);
			var screen			= ToProperty( new Screen()			);
			var time			= ToProperty( new Time()			);
			var systemInfo		= ToProperty( new SystemInfo()		);

			var appender = new StringAppender
			{
				{ "<b>Application</b>" },
				{ "" },
				{ application },
				{ "" },
				{ "<b>Debug</b>" },
				{ "" },
				{ debug },
				{ "" },
				{ "<b>Scene Manager</b>" },
				{ "" },
				{ $"    GetActiveScene: {activeScene.name}({activeScene.buildIndex})" },
				{ "" },
				{ sceneManager },
				{ "" },
				{ "<b>Screen</b>" },
				{ "" },
				{ screen },
				{ "" },
				{ "<b>Time</b>" },
				{ "" },
				{ time },
				{ "" },
				{ "<b>System Info</b>" },
				{ "" },
				{ systemInfo },
				{ "" },
				{ "<b>Other</b>" },
				{ "" },
				{ "    IL2CPP: {0}", IL2CPP },
				{ "" },
			};

			return appender.ToString();
		}

		/// <summary>
		/// 指定されたオブジェクトのプロパティを文字列で変え島す
		/// </summary>
		private static string ToProperty( object obj )
		{
			var values = obj
				.GetType()
				.GetProperties( BindingFlags.Static | BindingFlags.Public )
				.Where( c => c.CanRead )
				.Select( c => $"    {c.Name}: {c.GetValue( obj, null )}" )
			;

			var text = string.Join( "\n", values );

			return text;
		}
	}
}