using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UniDebugMenu.Example
{
	/// <summary>
	/// ログ情報のリストを作成するクラス
	/// </summary>
	[Serializable]
	public sealed class LogListCreator : ListCreatorBase<ActionData>, IDisposable
	{
		//==============================================================================
		// 列挙型
		//==============================================================================
		private enum TabType
		{
			ALL		,	// 全て
			LOG		,	// 情報
			WARNING	,	// 警告
			ERROR	,	// エラー
		}

		//==============================================================================
		// クラス
		//==============================================================================
		/// <summary>
		/// 出力されたログの情報を管理するクラス
		/// </summary>
		private sealed class LogData
		{
			//==========================================================================
			// 変数(readonly)
			//==========================================================================
			public readonly TabType	m_type				;	// タブの種類
			public readonly string	m_fullCondition		;	// 概要（全部）
			public readonly string	m_condition			;	// 概要
			public readonly string	m_stackTrace		;	// スタックトレース
			public readonly string	m_dateTime			;	// 日時
			public readonly string	m_message			;	// 表示用のテキスト

			//==========================================================================
			// 関数
			//==========================================================================
			// コンストラクタ
			public LogData
			(
				TabType	type			,
				string	fullCondition	,
				string	condition		,
				string	stackTrace		,
				string	dateTime
			)
			{
				m_type			= type			;
				m_fullCondition	= fullCondition	;
				m_condition		= condition		;
				m_stackTrace	= stackTrace	;
				m_dateTime		= dateTime		;

				var colorTag = ToColorTag( type );

				m_message = $"<color={colorTag}>[{dateTime}] {condition}</color>";
			}

			// コンストラクタ
			public LogData
			(
				string	fullCondition	,
				string	condition		,
				string	stackTrace		,
				LogType	type
			) : this (
				type			: ToTabType( type ),
				fullCondition	: fullCondition,
				condition		: condition,
				stackTrace		: stackTrace,
				dateTime		: DateTime.Now.ToString( "HH:mm:ss" )
			)
			{
			}

			// 一行分のログ情報に変換して返します
			public IEnumerable<LogData> ToSimpleLogList()
			{
				var conditions = m_fullCondition
					.Split( '\n' )
					.SelectMany( c => c.SubstringAtCount( 80 ) )
				;

				foreach ( var n in conditions )
				{
					var logData = new LogData
					(
						type			: m_type			,
						fullCondition	: m_fullCondition	,
						condition		: n					,
						stackTrace		: m_stackTrace		,
						dateTime		: m_dateTime
					);

					yield return logData;
				}
			}

			// 詳細な文字列に整形して返します
			public override string ToString()
			{
				var appender = new StringAppender
				{
					{ "■ DateTime"		},
					{ "　"				},
					{ m_dateTime		},
					{ "　"				},
					{ "■ Condition"	},
					{ "　"				},
					{ m_fullCondition	},
					{ "　"				},
					{ "■ StackTrace"	},
					{ "　"				},
					{ m_stackTrace		},
					{ "　"				},
				};
				return appender.ToString();
			}

			// ログの種類をタブの種類に変換します
			private static TabType ToTabType( LogType self )
			{
				switch ( self )
				{
					case LogType.Log		: return TabType.LOG		;
					case LogType.Warning	: return TabType.WARNING	;
					case LogType.Error		: return TabType.ERROR		;
					case LogType.Assert		: return TabType.ERROR		;
					case LogType.Exception	: return TabType.ERROR		;
				}
				return TabType.ALL;
			}

			// タブの種類を color タグに変換します
			private static string ToColorTag( TabType self )
			{
				switch ( self )
				{
					case TabType.WARNING	: return "yellow"	;
					case TabType.ERROR		: return "red"		;
				}
				return "white";
			}
		}

		//==============================================================================
		// 変数(static)
		//==============================================================================
		private static readonly List<LogData> m_logList = new List<LogData>();  // ログ情報のリスト

		//==============================================================================
		// 変数(readonly)
		//==============================================================================
		private readonly int m_max;

		//==============================================================================
		// 変数
		//==============================================================================
		private IList<ActionData> m_list;

		//==============================================================================
		// プロパティ
		//==============================================================================
		public override int Count => m_list.Count;

		public override string[] TabNameList => new []
		{
			"全て"		,
			"情報"		,
			"警告"		,
			"エラー"	,
		};

		public override ActionData[] OptionActionList => new []
		{
			new ActionData( "クリア", () => Clear() ),
		};

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public LogListCreator( int max )
		{
			m_max = max;
		}

		/// <summary>
		/// イベントを登録します
		/// </summary>
		public void Init() => Application.logMessageReceivedThreaded += HandleLog;

		/// <summary>
		/// イベントを解除します
		/// </summary>
		public void Dispose() => Application.logMessageReceivedThreaded -= HandleLog;

		/// <summary>
		/// ログが出力された時に呼び出されます
		/// </summary>
		private void HandleLog( string condition, string stackTrace, LogType type )
		{
			var data = new LogData( condition, condition, stackTrace, type );
			m_logList.Insert( 0, data );
			while ( m_max <= m_logList.Count )
			{
				m_logList.RemoveAt( m_logList.Count - 1 );
			}
		}

		/// <summary>
		/// リストの表示に使用するデータを作成します
		/// </summary>
		protected override void DoCreate( ListCreateData data )
		{
			var tabIndex	= data.TabIndex;
			var tabType		= ( TabType )tabIndex;
			var isAll		= tabType == TabType.ALL;

			m_list = m_logList
				.Where( c => isAll || c.m_type == tabType )
				.SelectMany( c => c.ToSimpleLogList() )
				.Where( c => data.IsMatch( c.m_fullCondition ) )
				.Select( c => new ActionData( c.m_message, () => OpenAdd( DMType.TEXT_TAB_6, new SimpleTextListDataCreator( c.ToString() ) ) ) )
				.ToArray()
				.ReverseIf( data.IsReverse )
			;
		}

		/// <summary>
		/// 指定されたインデックスの要素の表示に使用するデータを返します
		/// </summary>
		protected override ActionData DoGetElemData( int index ) => m_list.ElementAtOrDefault( index );

		/// <summary>
		/// ログをすべて消去します
		/// </summary>
		private void Clear()
		{
			m_logList.Clear();
			UpdateDisp();
		}
	}
}