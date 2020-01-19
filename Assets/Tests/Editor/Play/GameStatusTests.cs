using System;
using NUnit.Framework;
using Assert = UnityEngine.Assertions.Assert;

namespace Tests
{
	/// <summary>
	/// <c>GameStatus</c>クラスのテストを行う。
	/// </summary>
	public class GameStatusTests
	{
		/// <summary>
		/// <c>GameStatus</c>の数をテストする。
		/// </summary>
		[Test]
		public void Length ()
		{
			Assert.AreEqual (5, Enum.GetValues (typeof (GameStatus)).Length);
		}
	}
}
