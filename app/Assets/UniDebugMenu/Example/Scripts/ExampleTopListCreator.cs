using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UniDebugMenu.Example
{
	/// <summary>
	/// デバッグメニューのトップ画面のリストを作成するクラス
	/// </summary>
	[Serializable]
	public sealed class ExampleTopListCreator : ListCreatorBase<ActionData>, IDisposable
	{
		//==============================================================================
		// 変数(readonly)
		//==============================================================================
		private readonly ActionData[] m_sourceList;

		// ログ情報のリストを作成するインスタンス
		private readonly LogListCreator m_logDataCreator = new LogListCreator( 1500 );

		//==============================================================================
		// 変数
		//==============================================================================
		private IList<ActionData> m_list;

		//==============================================================================
		// プロパティ
		//==============================================================================
		public override int Count => m_list.Count;

		public override ActionData[] OptionActionList => new []
		{
			new ActionData( "ボタン1"	, () => Debug.Log( "ボタン1が押されました" ) ),
			new ActionData( "ボタン2"	, () => Debug.Log( "ボタン2が押されました" ) ),
			new ActionData( "ボタン3"	, () => Debug.Log( "ボタン3が押されました" ) ),
			new ActionData( "ボタン4"	, () => Debug.Log( "ボタン4が押されました" ) ),
			new ActionData( "ボタン5"	, () => Debug.Log( "ボタン5が押されました" ) ),
		};

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public ExampleTopListCreator()
		{
			m_sourceList = new []
			{
				new ActionData( "テキスト表示サンプル"			, () => OpenAdd( DMType.TEXT_TAB_6		, new TextListCreatorExample() ) ),
				new ActionData( "ボタン表示サンプル 2列"		, () => OpenAdd( DMType.BUTTON_COL_2	, new TextListCreatorExample() ) ),
				new ActionData( "ボタン表示サンプル 3列"		, () => OpenAdd( DMType.BUTTON_COL_3	, new TextListCreatorExample() ) ),
				new ActionData( "システム情報"					, () => OpenAdd( DMType.TEXT_TAB_6		, new SystemInfoListCreator() ) ),
				new ActionData( "システムコマンド"				, () => OpenAdd( DMType.COMMAND_TAB_6	, new SystemCommandListCreator() ) ),
				new ActionData( "ゲームオブジェクト一覧"		, () => OpenAdd( DMType.COMMAND_TAB_6	, new GameObjectListCreator() ) ),
				new ActionData( "ログ情報"						, () => OpenAdd( DMType.TEXT_TAB_6		, m_logDataCreator ) ),
				new ActionData( "iOS クラッシュ情報"			, () => OpenAdd( DMType.TEXT_TAB_6		, new iOSCrashReportListCreator() ) ),
				new ActionData( "読み込み済みテクスチャ一覧"	, () => OpenAdd( DMType.TEXT_TAB_6		, new LoadedObjectListCreator<Texture>() ) ),
				new ActionData( "読み込み済みマテリアル一覧"	, () => OpenAdd( DMType.TEXT_TAB_6		, new LoadedObjectListCreator<Material>() ) ),
			};
		}

		/// <summary>
		/// 初期化します
		/// </summary>
		public void Init() => m_logDataCreator.Init();

		/// <summary>
		/// 破棄します
		/// </summary>
		public void Dispose() => m_logDataCreator.Dispose();

		/// <summary>
		/// リストの表示に使用するデータを作成します
		/// </summary>
		protected override void DoCreate( ListCreateData data )
		{
			m_list = m_sourceList
				.Where( c => data.IsMatch( c.m_text ) )
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