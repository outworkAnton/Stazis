using System;
using NUnit.Framework;
using Stazis;

namespace StazisTest
{
	[TestFixture]
	public class DataOperationsTesting
	{
		[Test]
		public void Check_IsDate_Condition()
		{
			DateTime dt = DateTime.Now;
			Assert.IsTrue(DataOperations.IsDate(dt));
		}
	}
}
