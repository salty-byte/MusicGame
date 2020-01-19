using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Assert = UnityEngine.Assertions.Assert;

namespace Tests
{
	/// <summary>
	/// <c>MusicLoader</c>クラスのテスト。
	/// <c>UnityWebRequestMultimedia</c>を使っているため、PlayModeでのテストとしている。
	/// </summary>
	class MusicLoaderTests
	{
		/// <summary>
		/// 正常系のファイルロードテスト。
		/// <c>ExpectedResult</c>がnullなのは、テストの戻り値がIEnumeratorのため。
		/// </summary>
		/// <param name="fileName">ロード対象の楽曲名。</param>
		/// <returns>IEnumerator</returns>
		[UnityTest]
		[TestCase ("test.ogg", ExpectedResult = null)]
		public IEnumerator LoadExistAudio (string fileName)
		{
			float startTime = Time.realtimeSinceStartup;
			var func = MusicLoader.LoadAudioClipAsync ($"{Application.dataPath}/Tests/Resources/{fileName}");
			while (func.MoveNext ()) {
				yield return null;
			}
			float endTime = Time.realtimeSinceStartup - startTime;
			var clip = (AudioClip)func.Current;

			Assert.IsTrue (endTime < 0.5f, "ロード時間が0.5秒未満であること。");
			Assert.IsNotNull (clip, "楽曲がnullでないこと。");
		}

		/// <summary>
		/// 存在しないファイルを指定した場合のテスト。
		/// <c>ExpectedResult</c>がnullなのは、テストの戻り値がIEnumeratorのため。
		/// </summary>
		/// <param name="fileName">ロード対象の楽曲名。</param>
		/// <returns>IEnumerator</returns>
		[UnityTest]
		[TestCase ("test.og", ExpectedResult = null)]
		public IEnumerator LoadNotExistAudio (string fileName)
		{
			var func = MusicLoader.LoadAudioClipAsync ($"{Application.dataPath}/Tests/Resources/{fileName}");
			while (func.MoveNext ()) {
				yield return null;
			}
			Assert.IsNull (func.Current, "楽曲がnullであること。");
		}
	}
}
