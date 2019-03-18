using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UniDebugMenu.Example
{
	/// <summary>
	/// システムコマンドのリストを作成するクラス
	/// </summary>
	[Serializable]
	public sealed class SystemCommandListCreator : ListCreatorBase<CommandData>
	{
		//==============================================================================
		// 列挙型
		//==============================================================================
		private enum TabType
		{
			NONE		,
			APPLICATION	,
			DEBUG		,
			SCREEN		,
			TIME		,
		}

		//==============================================================================
		// 変数(SerializeField)
		//==============================================================================
		private readonly IList<CommandDataWithTabType<TabType>> m_sourceList;

		//==============================================================================
		// 変数
		//==============================================================================
		private IList<CommandData> m_list;

		//==============================================================================
		// プロパティ
		//==============================================================================
		public override int		Count		=> m_list.Count;
		public override bool	IsShowToast	=> true;

		public override string[] TabNameList => new []
		{
			"All"			,
			"Application"	,
			"Debug"			,
			"Screen"		,
			"Time"			,
		};

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// 作成して返します
		/// </summary>
		public SystemCommandListCreator()
		{
			var creator = new CommandDataCreator<TabType>();

			m_sourceList = new[]
			{
				creator.Create
				(
					TabType.APPLICATION,
					() => "APPLICATION"
				),
				creator.Create
				(
					TabType.APPLICATION,
					() => $"FPS: {Application.targetFrameRate}",
					new InputActionData( InputValidateType.INTEGER, str => Application.targetFrameRate = int.Parse( str ) ),
					new ActionData( "30", () => Application.targetFrameRate = 30 ),
					new ActionData( "60", () => Application.targetFrameRate = 60 )
				),
				creator.Create
				(
					TabType.DEBUG,
					() => "DEBUG"
				),
				creator.Create
				(
					TabType.DEBUG,
					() => $"ログ出力: {Debug.unityLogger.logEnabled}",
					new ToggleActionData( () => Debug.unityLogger.logEnabled, isOn => Debug.unityLogger.logEnabled = isOn	)
				),
				creator.Create
				(
					TabType.DEBUG,
					() => "不具合",
					new ActionData( "エラーログ"	, () => Debug.LogError( "エラー発生テスト" )					),
					new ActionData( "例外"			, () => Debug.LogException( new Exception( "例外発生テスト" ) )	)
				),
				creator.Create
				(
					TabType.DEBUG,
					() => "無限ループ",
					new ActionData( "while"	, () => { while ( true ) { } }	),
					new ActionData( "for"	, () => { for ( ; ; ) { } }		)
				),
				creator.Create
				(
					TabType.SCREEN,
					() => "SCREEN"
				),
				creator.Create
				(
					TabType.SCREEN,
					() => $"スリープするかどうか: {Screen.sleepTimeout == SleepTimeout.SystemSetting}",
					new ActionData( "しない", () => Screen.sleepTimeout = SleepTimeout.NeverSleep		),
					new ActionData( "する"	, () => Screen.sleepTimeout = SleepTimeout.SystemSetting	)
				),
				creator.Create
				(
					TabType.SCREEN,
					() => $"端末の向き: {Screen.orientation}",
					new ActionData( "自動回転"			, () => Screen.orientation = ScreenOrientation.AutoRotation			),
					new ActionData( "縦持ち"			, () => Screen.orientation = ScreenOrientation.Portrait				),
					new ActionData( "縦持ち\n（逆）"	, () => Screen.orientation = ScreenOrientation.PortraitUpsideDown	)
				),
				creator.Create
				(
					TabType.SCREEN,
					() => "",
					new ActionData( "横持ち"			, () => Screen.orientation = ScreenOrientation.Landscape			),
					new ActionData( "横持ち\n（左下）"	, () => Screen.orientation = ScreenOrientation.LandscapeLeft		),
					new ActionData( "横持ち\n（右下）"	, () => Screen.orientation = ScreenOrientation.LandscapeRight		)
				),
				creator.Create
				(
					TabType.TIME,
					() => "TIME"
				),
				creator.Create
				(
					TabType.TIME,
					() => $"タイム\nスケール: {Time.timeScale}",
					new InputActionData( str => Time.timeScale = float.Parse( str ) ),
					new ActionData( "0"		, () => Time.timeScale = 0		),
					new ActionData( "0.5"	, () => Time.timeScale = 0.5f	) ,
					new ActionData( "1"		, () => Time.timeScale = 1		) ,
					new ActionData( "2"		, () => Time.timeScale = 2		) ,
					new ActionData( "4"		, () => Time.timeScale = 4		)
				),
				creator.Create
				(
					TabType.NONE,
					() => "OTHER"
				),
				creator.Create
				(
					TabType.NONE,
					() => "Resources.UnloadUnusedAssets",
					new ActionData( "実行"	, () => Resources.UnloadUnusedAssets() )
				),
				creator.Create
				(
					TabType.NONE,
					() => "GC.Collect",
					new ActionData( "実行"	, () => GC.Collect() )
				),
				creator.Create
				(
					TabType.NONE,
					() => "Caching.ClearCache",
					new ActionData( "実行"	, () => Caching.ClearCache() )
				),
			};
		}

		/// <summary>
		/// リストの表示に使用するデータを作成します
		/// </summary>
		protected override void DoCreate( ListCreateData data )
		{
			var tabType		= ( TabType )data.TabIndex;
			var isAll		= tabType == 0;

			m_list = m_sourceList
				.Where( c => isAll || c.m_tabType == tabType )
				.Select( c => c.m_data )
				.Where( c => data.IsMatch( c.m_getText() ) )
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