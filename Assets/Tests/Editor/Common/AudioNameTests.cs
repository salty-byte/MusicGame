using System;
using NUnit.Framework;
using Assert = UnityEngine.Assertions.Assert;

namespace Tests
{
	/// <summary>
	/// <c>SEName</c>の数や文字列のテストを行う。
	/// </summary>
	class AudioNameTest
	{
		/// <summary>
		/// SE名の数をテストする。
		/// </summary>
		public void Length ()
		{
			Assert.AreEqual (4, Enum.GetValues (typeof (SEName)).Length);
		}

		/// <summary>
		/// SE名に割り当てられている文字列をテストする。
		/// </summary>
		/// <param name="expected">期待値。</param>
		/// <param name="name">テスト対象の<c>SEName</c>。</param>
		[TestCase ("SE_Click_Start", SEName.ClickStart)]
		[TestCase ("SE_Decide", SEName.Decide)]
		[TestCase ("SE_Excellent", SEName.Excellent)]
		[TestCase ("SE_Good", SEName.Good)]
		public void GetString (string expected, SEName name)
		{
			Assert.AreEqual (expected, name.GetString ());
		}
	}
}
