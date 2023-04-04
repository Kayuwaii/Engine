using Microsoft.VisualStudio.TestTools.UnitTesting;
using Engine.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Base.Tests
{
    [TestClass()]
    public class GeneralTests
    {
        [TestMethod()]
        public void getPercentTest()
        {
            bool t1 = General.getPercent(100, 25) == 25;
            bool t2 = General.getPercent(1000, 25) == 2.5d;
            bool t3 = General.getPercent(10000, 25) == 0.25d;

            Assert.IsTrue(t1 && t2 && t3);
        }

        [TestMethod()]
        public void isEvenTest()
        {
            Assert.IsTrue(4.isEven());
        }
    }
}