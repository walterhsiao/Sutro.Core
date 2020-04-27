using g3;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace gs.UnitTests.Fill
{
    [TestClass]
    public class FillBaseTests
    {
        private static double delta = 1e-4;

        [TestMethod]
        public void ElementsReversed()
        {
            // Act
            var elements = FillFactory.CreateTriangleCCW().ElementsReversed().ToList();

            // Assert
            Assert.AreEqual(3, elements.Count);

            Assert.AreEqual(0, elements[0].NodeStart.x, delta);
            Assert.AreEqual(0, elements[0].NodeStart.y, delta);

            Assert.AreEqual(4, elements[1].NodeStart.x, delta);
            Assert.AreEqual(3, elements[1].NodeStart.y, delta);

            Assert.AreEqual(4, elements[2].NodeStart.x, delta);
            Assert.AreEqual(0, elements[2].NodeStart.y, delta);

            Assert.AreEqual(0, elements[2].NodeEnd.x, delta);
            Assert.AreEqual(0, elements[2].NodeEnd.y, delta);
        }

        [TestMethod]
        public void FindClosestElementToPoint_Case1()
        {
            // Arrange 
            var loop = FillFactory.CreateTriangleCCW();
            var point = new Vector2d(5, 2);

            // Act
            var distance = loop.FindClosestElementToPoint(point, out int index, out double parameter);

            // Assert
            Assert.AreEqual(1, distance, delta);
            Assert.AreEqual(1, index);
            Assert.AreEqual(2d / 3d, parameter, delta);
        }

        [TestMethod]
        public void FindClosestElementToPoint_Case2()
        {
            // Arrange 
            var loop = FillFactory.CreateTriangleCCW();
            var point = new Vector2d(5, -1);

            // Act
            var distance = loop.FindClosestElementToPoint(point, out int index, out double parameter);

            // Assert
            Assert.AreEqual(Math.Sqrt(2), distance, delta);
            Assert.AreEqual(1, index);
            Assert.AreEqual(0, parameter, delta);
        }

        [TestMethod]
        public void TotalLength()
        {
            // Act
            var length = FillFactory.CreateTriangleCCW().TotalLength();

            // Assert
            Assert.AreEqual(4 + 3 + (Math.Sqrt(4 * 4 + 3 * 3)), length, delta);
        }
    }
}
