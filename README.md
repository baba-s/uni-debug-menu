# UniDebugMenu

横向きかつクリックやタップ可能なゲームで使用できるカスタマイズ可能なデバッグメニュー  

## 機能

- 3種類のメニュー表示を使用可能
	- テキスト表示
	- ボタン表示
	- カスタマイズ可能なボタン表示
- 大量のデータを実装可能
- 多階層のメニューを実装可能
- 検索可能
- カテゴリ分け可能
- トースト表示
- イベント検知
- ドラッグ可能な開くボタン
- どこからでも開ける
- 複数解像度対応

## バージョン

- Unity 2018.3.7f1

## サンプル

### トップ画面

![](https://cdn-ak.f.st-hatena.com/images/fotolife/b/baba_s/20190318/20190318121622.png)

### システム情報

![](https://cdn-ak.f.st-hatena.com/images/fotolife/b/baba_s/20190318/20190318121129.png)

![](https://cdn-ak.f.st-hatena.com/images/fotolife/b/baba_s/20190318/20190318121138.png)

![](https://cdn-ak.f.st-hatena.com/images/fotolife/b/baba_s/20190318/20190318121145.png)

### システムコマンド

![](https://cdn-ak.f.st-hatena.com/images/fotolife/b/baba_s/20190318/20190318121153.png)

![](https://cdn-ak.f.st-hatena.com/images/fotolife/b/baba_s/20190318/20190318121201.png)

### ゲームオブジェクト一覧

![](https://cdn-ak.f.st-hatena.com/images/fotolife/b/baba_s/20190318/20190318121208.png)

### ログ情報

![](https://cdn-ak.f.st-hatena.com/images/fotolife/b/baba_s/20190318/20190318121215.png)

![](https://cdn-ak.f.st-hatena.com/images/fotolife/b/baba_s/20190318/20190318121224.png)

## 3種類のメニュー表示を使用可能

### テキスト表示

![](https://cdn-ak.f.st-hatena.com/images/fotolife/b/baba_s/20190318/20190318121129.png)

### ボタン表示

![](https://cdn-ak.f.st-hatena.com/images/fotolife/b/baba_s/20190318/20190318121245.png)

![](https://cdn-ak.f.st-hatena.com/images/fotolife/b/baba_s/20190318/20190318121252.png)

### カスタマイズ可能なボタン表示

![](https://cdn-ak.f.st-hatena.com/images/fotolife/b/baba_s/20190318/20190318121153.png)

## 大量のデータを実装可能

![](https://cdn-ak.f.st-hatena.com/images/fotolife/b/baba_s/20190318/20190318121306.gif)

## 多階層のメニューを実装可能

![](https://cdn-ak.f.st-hatena.com/images/fotolife/b/baba_s/20190318/20190318121326.gif)

## 検索可能

![](https://cdn-ak.f.st-hatena.com/images/fotolife/b/baba_s/20190318/20190318122249.gif)

## カテゴリ分け可能

![](https://cdn-ak.f.st-hatena.com/images/fotolife/b/baba_s/20190318/20190318122302.gif)

## トースト表示

![](https://cdn-ak.f.st-hatena.com/images/fotolife/b/baba_s/20190318/20190318122324.gif)

## ドラッグ可能な開くボタン

![](https://cdn-ak.f.st-hatena.com/images/fotolife/b/baba_s/20190318/20190318122400.gif)

## どこからでも開ける

![](https://cdn-ak.f.st-hatena.com/images/fotolife/b/baba_s/20190318/20190318122651.gif)

## 複数解像度対応

### 18:9

![](https://cdn-ak.f.st-hatena.com/images/fotolife/b/baba_s/20190318/20190318122531.png)

### 4:3

![](https://cdn-ak.f.st-hatena.com/images/fotolife/b/baba_s/20190318/20190318122547.png)

## スクリプト

### 基本

```cs
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UniDebugMenu.Example
{
    /// <summary>
    /// デバッグメニューのサンプルを管理するクラス
    /// </summary>
    public sealed class UniDebugMenuExampleScene : MonoBehaviour
    {
        //==============================================================================
        // 変数
        //==============================================================================
        private ExampleTopListCreator m_creator;

        //==============================================================================
        // 関数
        //==============================================================================
        /// <summary>
        /// 初期化される時に呼び出されます
        /// </summary>
        private void Awake()
        {
            // デバッグメニューのトップ画面のリストを管理するインスタンスを作成します
            m_creator = new ExampleTopListCreator();
            m_creator.Init();

            // デバッグメニューのシーンを加算で読み込みます
            SceneManager.LoadScene( "UniDebugMenuScene", LoadSceneMode.Additive );

            // デバッグメニューでコマンドが実行された時に呼び出されます
            UniDebugMenuScene.mOnChange += () => Debug.Log( "UniDebugMenuScene.mOnChange" );

            // DM ボタンが押された時にデバッグメニューのトップ画面を開くように設定します
            UniDebugMenuScene.mOnOpen += () => DMType.BUTTON_COL_3.Open( m_creator );
        }

        /// <summary>
        /// デバッグメニューを開くボタンが押された
        /// </summary>
        public void OnClick1()
        {
            // デバッグメニューのトップ画面を開きます
            DMType.BUTTON_COL_3.Open( m_creator );
        }

        /// <summary>
        /// システムコマンドの画面を開くボタンが押された
        /// </summary>
        public void OnClick2()
        {
            // デバッグメニューのシステムコマンドの画面を開きます
            DMType.COMMAND_TAB_6.Open( new SystemCommandListCreator() );
        }

        /// <summary>
        /// デバッグメニューを開く DM ボタンを透明にするボタンが押された
        /// </summary>
        public void OnClick3()
        {
            // DM ボタンを透明にします
            // DM ボタンは透明になるだけで当たり判定は有効のままになります
            // true：不透明　false：透明
            UniDebugMenuScene.SetOpenButtonVisible( false );
        }

        /// <summary>
        /// デバッグメニューを削除するボタンが押された
        /// </summary>
        public void OnClick4()
        {
            // デバッグメニューのゲームオブジェクトを削除します
            if ( UniDebugMenuScene.Destroy() )
            {
                // デバッグメニューのゲームオブジェクトが削除できた場合は
                // デバッグメニューのシーンもアンロードします
                SceneManager.UnloadSceneAsync( "UniDebugMenuScene" );
            }
        }
    }
}
```

### リスト生成

```cs
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
            new ActionData( "ボタン1"   , () => Debug.Log( "ボタン1が押されました" ) ),
            new ActionData( "ボタン2"   , () => Debug.Log( "ボタン2が押されました" ) ),
            new ActionData( "ボタン3"   , () => Debug.Log( "ボタン3が押されました" ) ),
            new ActionData( "ボタン4"   , () => Debug.Log( "ボタン4が押されました" ) ),
            new ActionData( "ボタン5"   , () => Debug.Log( "ボタン5が押されました" ) ),
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
                new ActionData( "テキスト表示サンプル"      , () => OpenAdd( DMType.TEXT_TAB_6      , new TextListCreatorExample() ) ),
                new ActionData( "ボタン表示サンプル 2列"    , () => OpenAdd( DMType.BUTTON_COL_2    , new TextListCreatorExample() ) ),
                new ActionData( "ボタン表示サンプル 3列"    , () => OpenAdd( DMType.BUTTON_COL_3    , new TextListCreatorExample() ) ),
                new ActionData( "システム情報"              , () => OpenAdd( DMType.TEXT_TAB_6      , new SystemInfoListCreator() ) ),
                new ActionData( "システムコマンド"          , () => OpenAdd( DMType.COMMAND_TAB_6   , new SystemCommandListCreator() ) ),
                new ActionData( "ゲームオブジェクト一覧"    , () => OpenAdd( DMType.COMMAND_TAB_6   , new GameObjectListCreator() ) ),
                new ActionData( "ログ情報"                  , () => OpenAdd( DMType.TEXT_TAB_6      , m_logDataCreator ) ),
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
```