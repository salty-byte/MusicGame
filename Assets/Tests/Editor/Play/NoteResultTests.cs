using System;
using NUnit.Framework;
using Assert = UnityEngine.Assertions.Assert;

namespace Tests
{
	/// <summary>
	/// <c>NotResult</c>クラスのテストを行う。
	/// </summary>
	public class NoteResultTests
	{
		/// <summary>
		/// <c>NoteResult</c>の数をテストする。
		/// </summary>
		[Test]
		public void Length ()
		{
			Assert.AreEqual (5, Enum.GetValues (typeof (NoteResult)).Length);
		}
	}
}
