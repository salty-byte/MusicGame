using System.Collections;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Assert = UnityEngine.Assertions.Assert;

namespace Tests
{
	/// <summary>
	/// <c>FileUtils</c>クラスのテスト。
	/// </summary>
	class FileUtilsTests
	{
		public static readonly string OutputPath = $"{Application.dataPath}/Tests/Resources/Output/";

		/// <summary>
		/// このクラスのテスト開始時に、テスト用のフォルダを作成する。
		/// </summary>
		[OneTimeSetUp]
		public void CreateTestFolder ()
		{
			var di = new DirectoryInfo (OutputPath);
			if (!di.Exists) {
				di.Create ();
			}
		}

		/// <summary>
		/// [Editor]ゲームデータパスのテスト。
		/// </summary>
		[Test]
		public void GetGameDataPath ()
		{
			string path = FileUtils.GetGameDataPath ();
			Assert.IsNotNull (path);
			Assert.IsTrue (path.EndsWith ("/Assets/GameData/"),
				"ゲームデータの格納場所が「/Assets/GameData/」配下であること。");
		}

		/// <summary>
		/// 文字列をファイルに出力するテスト。
		/// </summary>
		[Test]
		public void WriteText ()
		{
			string file = $"{OutputPath}/WiteTest.txt";
			bool result = FileUtils.WriteText (file, "Test");
			Assert.IsTrue (result);
			Assert.IsTrue (new FileInfo (file).Exists, "ファイルが作成されていること。");
		}

		/// <summary>
		/// ファイルから文字列を取得するテスト。
		/// </summary>
		/// <param name="data"></param>
		[Test]
		[TestCase ("Test")]
		[TestCase ("!#¥|$%&'(SDFGHJ1234567¥r¥ntest")]
		public void ReadText (string data)
		{
			string file = $"{OutputPath}/ReadTest.txt";
			FileUtils.WriteText (file, data);

			string text = FileUtils.ReadText (file);
			Assert.AreEqual (data, text, "テキストが読み込まれること。");
		}

		/// <summary>
		/// このクラスのテスト終了時に、テスト用のフォルダを削除する。
		/// </summary>
		[OneTimeTearDown]
		public void DeleteFile ()
		{
			var di = new DirectoryInfo (OutputPath);
			if (di.Exists) {
				di.Delete (true);
			}
		}
	}
}
