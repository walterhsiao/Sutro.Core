using g3;
using gs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace gsSlicer.UnitTests.fill
{
    [TestClass]
    public class BasicFillLoopTests
    {
        [TestMethod]
        public void DefaultConstructor()
        {
            var loop = new BasicFillLoop();
        }

        [TestMethod]
        public void EnumerableVector2dConstructor()
        {
            // Arrange
            var vertices = new List<Vector2d>()
            {
                new Vector2d(0,0),
                new Vector2d(3,0),
                new Vector2d(0,4)
            };

            // Act
            var loop = new BasicFillLoop(vertices);

            // Assert
            Assert.AreEqual(3, loop.VertexCount);
            Assert.AreEqual(3, loop.SegmentCount);
        }
    }
}
