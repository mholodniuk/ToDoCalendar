using NUnit.Framework;
using System;

namespace ToDoCalendar.Tests
{
    public class UnitTests
    {
        [Test]
        public void TestMethod1()
        {
            string invalidName = "";
            string validTime = "12:45";
            Assert.False(Utils.Utils.isActivityValid(invalidName, validTime));
        }
    }
}
