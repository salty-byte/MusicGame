using System;
using NUnit.Framework;

namespace Tests
{
	/// <summary>
	/// <c>SceneName</c>の数や文字列のテストを行う。
	/// </summary>
	public class SceneNameTests
	{
		/// <summary>
		/// シーン名の数をテストする。
		/// </summary>
		public void Length ()
		{
			Assert.AreSame (4, Enum.GetValues (typeof (SceneName)).Length);
		}

		/// <summary>
		/// シーン名に割り当てられている文字列をテストする。
		/// </summary>
		/// <param name="expected">期待値。</param>
		/// <param name="name">テスト対象の<c>SceneName</c>。</param>
		[TestCase ("Start", SceneName.Start)]
		[TestCase ("Load", SceneName.Load)]
		[TestCase ("Select", SceneName.Select)]
		[TestCase ("Play", SceneName.Play)]
		public void GetString (string expected, SceneName name)
		{
			Assert.AreEqual (expected, name.GetString ());
		}
	}
}
