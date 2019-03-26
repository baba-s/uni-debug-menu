using UnityEngine.Profiling;

namespace UniDebugMenu.Example
{
	public sealed class MonoMemoryChecker
	{
		public float Used { get; private set; }
		public float Total { get; private set; }

		public string UsedText { get; private set; }
		public string TotalText { get; private set; }

		public void Update()
		{
			Used = ( Profiler.GetMonoUsedSizeLong() >> 10 ) / 1024f;

			// ヒープが大きいほど GC の時間がかかりますが、実行される頻度は少なくなる
			Total = ( Profiler.GetMonoHeapSizeLong() >> 10 ) / 1024f;

			UsedText = Used.ToString( "0.0" ) + " MB";
			TotalText = Total.ToString( "0.0" ) + " MB";
		}
	}
}