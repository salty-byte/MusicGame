using NUnit.Framework;
using UnityEngine;
using System.IO;
using Assert = UnityEngine.Assertions.Assert;

namespace Tests
{
	/// <summary>
	/// <c>TjaLoader</c>クラスのテスト。
	/// </summary>
	class TjaTests
	{
		/// <summary>
		/// テストデータ読み込み先のパス。
		/// </summary>
		public static readonly string DataPath = $"{Application.dataPath}/Tests/Resources/TjaToMsd";

		/// <summary>
		/// テストデータ出力先のパス。
		/// </summary>
		public static readonly string OutputPath = $"{DataPath}/result";

		/// <summary>
		/// このクラスのテスト開始時に、テスト用のフォルダを作成する。
		/// </summary>
		[OneTimeSetUp]
		public void CreateTestFolder()
		{
			var di = new DirectoryInfo(OutputPath);
			if (!di.Exists)
			{
				di.Create();
			}
		}

		/// <summary>
		/// 指定したtjaファイルを読み込み、msdファイルとして出力するテスト。
		/// </summary>
		[Test]
		public void WriteText ()
		{
			string name = "Arabesque";
			string file = $"{DataPath}/{name}.tja";
			var loader = new TjaLoader ();
			var result = loader.Load (file);

			var md = new MusicData (name, "6");
			md.notes = result;
			md.audioFileName = $"{name}.ogg";

			string json = JsonUtility.ToJson (md);
			string outputFile = $"{OutputPath}/{name}.msd";
			FileUtils.WriteText (outputFile, json);

			Assert.IsTrue(new FileInfo(outputFile).Exists, "ファイルが作成されていること。");
		}

		/// <summary>
		/// このクラスのテスト終了時に、テスト用のフォルダを削除する。
		/// </summary>
		[OneTimeTearDown]
		public void DeleteFile()
		{
			var di = new DirectoryInfo(OutputPath);
			if (di.Exists)
			{
				di.Delete(true);
			}
		}
	}
}
