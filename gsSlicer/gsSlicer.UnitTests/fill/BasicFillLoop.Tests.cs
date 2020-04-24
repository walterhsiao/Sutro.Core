using g3;
using gs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace gsSlicer.UnitTests.fill
{
    [TestClass]
    public class BasicFillLoopTests
    {
        private static double delta = 1e-4;

        private FillLoop<FillSegment> CreateTriangleCCW()
        {
            return new FillLoop<FillSegment>(new Vector2d[] {
                new Vector2d(0, 0),
                new Vector2d(4, 0),
                new Vector2d(4, 3),
            });
        }

        private FillLoop<FillSegment> CreateTriangleCW()
        {
            return new FillLoop<FillSegment>(new Vector2d[] {
                new Vector2d(4, 3),
                new Vector2d(4, 0),
                new Vector2d(0, 0),
            });
        }

        [TestMethod]
        public void EnumerableVector2dConstructor()
        {
            // Arrange
            var vertices = new List<Vector2d>()
            {
                new Vector2d(2,0),
                new Vector2d(2,4),
                new Vector2d(0,4)
            };

            // Act
            var loop = new FillLoop<FillSegment>(vertices);

            // Assert
            Assert.AreEqual(3, loop.Elements.Count);
            Assert.AreEqual(2, loop.Elements[0].NodeStart.x);
        }

        [TestMethod]
        public void RollNoEffectCCW()
        {
            // Arrange
            var triangle = CreateTriangleCCW();

            // Act
            var result = triangle.RollBetweenVertex(0, 0);

            // Assert
            Assert.AreEqual(3, result.Elements.Count);

            Assert.AreEqual(0, result.Elements[0].NodeStart.x, delta);
            Assert.AreEqual(0, result.Elements[0].NodeStart.y, delta);

            Assert.AreEqual(4, result.Elements[1].NodeStart.x, delta);
            Assert.AreEqual(0, result.Elements[1].NodeStart.y, delta);

            Assert.AreEqual(4, result.Elements[2].NodeStart.x, delta);
            Assert.AreEqual(3, result.Elements[2].NodeStart.y, delta);

            Assert.AreEqual(0, result.Elements[2].NodeEnd.x, delta);
            Assert.AreEqual(0, result.Elements[2].NodeEnd.y, delta);
        }

        [TestMethod]
        public void RollNoEffectCW()
        {
            // Arrange
            var triangle = CreateTriangleCW();

            // Act
            var result = triangle.RollBetweenVertex(0, 0);

            // Assert
            Assert.AreEqual(3, result.Elements.Count);

            Assert.AreEqual(4, result.Elements[0].NodeStart.x, delta);
            Assert.AreEqual(3, result.Elements[0].NodeStart.y, delta);

            Assert.AreEqual(4, result.Elements[1].NodeStart.x, delta);
            Assert.AreEqual(0, result.Elements[1].NodeStart.y, delta);

            Assert.AreEqual(0, result.Elements[2].NodeStart.x, delta);
            Assert.AreEqual(0, result.Elements[2].NodeStart.y, delta);
        }

        [TestMethod]
        public void RollFirstSegmentHalfwayCCW()
        {
            // Arrange
            var triangle = CreateTriangleCCW();

            // Act
            var result = triangle.RollBetweenVertex(0, 0.5);

            // Assert
            Assert.AreEqual(4, result.Elements.Count);

            Assert.AreEqual(2, result.Elements[0].NodeStart.x, delta);
            Assert.AreEqual(0, result.Elements[0].NodeStart.y, delta);

            Assert.AreEqual(4, result.Elements[1].NodeStart.x, delta);
            Assert.AreEqual(0, result.Elements[1].NodeStart.y, delta);

            Assert.AreEqual(4, result.Elements[2].NodeStart.x, delta);
            Assert.AreEqual(3, result.Elements[2].NodeStart.y, delta);

            Assert.AreEqual(0, result.Elements[3].NodeStart.x, delta);
            Assert.AreEqual(0, result.Elements[3].NodeStart.y, delta);
        }

        [TestMethod]
        public void RollFirstSegmentHalfwayCW()
        {
            // Arrange
            var triangle = CreateTriangleCW();

            // Act
            var result = triangle.RollBetweenVertex(0, 0.5);

            // Assert
            Assert.AreEqual(4, result.Elements.Count);

            Assert.AreEqual(4, result.Elements[0].NodeStart.x, delta);
            Assert.AreEqual(1.5, result.Elements[0].NodeStart.y, delta);

            Assert.AreEqual(4, result.Elements[1].NodeStart.x, delta);
            Assert.AreEqual(0, result.Elements[1].NodeStart.y, delta);

            Assert.AreEqual(0, result.Elements[2].NodeStart.x, delta);
            Assert.AreEqual(0, result.Elements[2].NodeStart.y, delta);

            Assert.AreEqual(4, result.Elements[3].NodeStart.x, delta);
            Assert.AreEqual(3, result.Elements[3].NodeStart.y, delta);
        }

        [TestMethod]
        public void RollSecondSegmentHalfwayCCW()
        {
            // Arrange
            var triangle = CreateTriangleCCW();

            // Act
            var result = triangle.RollBetweenVertex(1, -0.5);

            // Assert
            Assert.AreEqual(4, result.Elements.Count);

            Assert.AreEqual(4, result.Elements[0].NodeStart.x, delta);
            Assert.AreEqual(1, result.Elements[0].NodeStart.y, delta);

            Assert.AreEqual(4, result.Elements[1].NodeStart.x, delta);
            Assert.AreEqual(3, result.Elements[1].NodeStart.y, delta);

            Assert.AreEqual(0, result.Elements[2].NodeStart.x, delta);
            Assert.AreEqual(0, result.Elements[2].NodeStart.y, delta);

            Assert.AreEqual(4, result.Elements[3].NodeStart.x, delta);
            Assert.AreEqual(0, result.Elements[3].NodeStart.y, delta);
        }

        [TestMethod]
        public void RollSecondSegmentHalfwayCW()
        {
            // Arrange
            var triangle = CreateTriangleCW();

            // Act
            var result = triangle.RollBetweenVertex(1, 0);

            // Assert
            Assert.AreEqual(4, result.Elements.Count);

            Assert.AreEqual(2, result.Elements[0].NodeStart.x, delta);
            Assert.AreEqual(0, result.Elements[0].NodeStart.y, delta);

            Assert.AreEqual(0, result.Elements[1].NodeStart.x, delta);
            Assert.AreEqual(0, result.Elements[1].NodeStart.y, delta);

            Assert.AreEqual(4, result.Elements[2].NodeStart.x, delta);
            Assert.AreEqual(3, result.Elements[2].NodeStart.y, delta);

            Assert.AreEqual(4, result.Elements[3].NodeStart.x, delta);
            Assert.AreEqual(0, result.Elements[3].NodeStart.y, delta);
        }

        [TestMethod]
        public void RollFirstSegmentWithinToleranceLowSideCCW()
        {
            // Arrange
            var triangle = CreateTriangleCCW();

            // Act
            var result = triangle.RollBetweenVertex(0, 4 * 0.004, 0.005);

            // Assert
            Assert.AreEqual(3, result.Elements.Count);

            Assert.AreEqual(0, result.Elements[0].NodeStart.x, delta);
            Assert.AreEqual(0, result.Elements[0].NodeStart.y, delta);

            Assert.AreEqual(4, result.Elements[1].NodeStart.x, delta);
            Assert.AreEqual(0, result.Elements[1].NodeStart.y, delta);

            Assert.AreEqual(4, result.Elements[2].NodeStart.x, delta);
            Assert.AreEqual(3, result.Elements[2].NodeStart.y, delta);
        }

        [TestMethod]
        public void RollFirstSegmentWithinToleranceLowSideCW()
        {
            // Arrange
            var triangle = CreateTriangleCW();

            // Act
            var result = triangle.RollBetweenVertex(1, 4 * 0.004, 0.005);

            // Assert
            Assert.AreEqual(3, result.Elements.Count);

            Assert.AreEqual(4, result.Elements[0].NodeStart.x, delta);
            Assert.AreEqual(0, result.Elements[0].NodeStart.y, delta);

            Assert.AreEqual(0, result.Elements[1].NodeStart.x, delta);
            Assert.AreEqual(0, result.Elements[1].NodeStart.y, delta);

            Assert.AreEqual(4, result.Elements[2].NodeStart.x, delta);
            Assert.AreEqual(3, result.Elements[2].NodeStart.y, delta);
        }

        [TestMethod]
        public void RollFirstSegmentWithinToleranceHighSideCCW()
        {
            // Arrange
            var triangle = CreateTriangleCCW();

            // Act
            var result = triangle.RollBetweenVertex(0, 0.9999, 0.005);

            // Assert
            Assert.AreEqual(3, result.Elements.Count);

            Assert.AreEqual(4, result.Elements[0].NodeStart.x, delta);
            Assert.AreEqual(0, result.Elements[0].NodeStart.y, delta);

            Assert.AreEqual(4, result.Elements[1].NodeStart.x, delta);
            Assert.AreEqual(3, result.Elements[1].NodeStart.y, delta);

            Assert.AreEqual(0, result.Elements[2].NodeStart.x, delta);
            Assert.AreEqual(0, result.Elements[2].NodeStart.y, delta);
        }

        [TestMethod]
        public void RollFirstSegmentWithinToleranceHighSideCW()
        {
            // Arrange
            var triangle = CreateTriangleCW();

            // Act
            var result = triangle.RollBetweenVertex(1, 0.9999, 0.005);

            // Assert
            Assert.AreEqual(3, result.Elements.Count);

            Assert.AreEqual(0, result.Elements[0].NodeStart.x, delta);
            Assert.AreEqual(0, result.Elements[0].NodeStart.y, delta);

            Assert.AreEqual(4, result.Elements[1].NodeStart.x, delta);
            Assert.AreEqual(3, result.Elements[1].NodeStart.y, delta);

            Assert.AreEqual(4, result.Elements[2].NodeStart.x, delta);
            Assert.AreEqual(0, result.Elements[2].NodeStart.y, delta);
        }

        [TestMethod]
        public void ConvertToCurve()
        {
            // Act
            var result = CreateTriangleCCW().ConvertToCurve();

            // Assert
            Assert.AreEqual(4, result.Elements.Count);

            Assert.AreEqual(0, result.Elements[0].NodeStart.x, delta);
            Assert.AreEqual(0, result.Elements[0].NodeStart.y, delta);

            Assert.AreEqual(4, result.Elements[1].NodeStart.x, delta);
            Assert.AreEqual(0, result.Elements[1].NodeStart.y, delta);

            Assert.AreEqual(4, result.Elements[2].NodeStart.x, delta);
            Assert.AreEqual(3, result.Elements[2].NodeStart.y, delta);

            Assert.AreEqual(0, result.Elements[3].NodeStart.x, delta);
            Assert.AreEqual(0, result.Elements[3].NodeStart.y, delta);
        }

        [TestMethod]
        public void SplitOnceFirstSegment()
        {
            // Act
            Func<FillCurve<FillSegment>> createFillCurveF = () => new FillCurve<FillSegment>();
            var result = CreateTriangleCCW().SplitAtDistances(new double[] { 3 });

            // Assert
            Assert.AreEqual(2, result.Count);

            var curve0 = result[0];
            Assert.AreEqual(2, curve0.Elements.Count);
            Assert.AreEqual(0, curve0.Elements[0].NodeStart.x, delta);
            Assert.AreEqual(0, curve0.Elements[0].NodeStart.y, delta);
            Assert.AreEqual(3, curve0.Elements[1].NodeStart.x, delta);
            Assert.AreEqual(0, curve0.Elements[1].NodeStart.y, delta);

            var curve1 = result[1];
            Assert.AreEqual(4, curve1.Elements.Count);
            Assert.AreEqual(3, curve1.Elements[0].NodeStart.x, delta);
            Assert.AreEqual(0, curve1.Elements[0].NodeStart.y, delta);
            Assert.AreEqual(4, curve1.Elements[1].NodeStart.x, delta);
            Assert.AreEqual(0, curve1.Elements[1].NodeStart.y, delta);
            Assert.AreEqual(4, curve1.Elements[2].NodeStart.x, delta);
            Assert.AreEqual(3, curve1.Elements[2].NodeStart.y, delta);
            Assert.AreEqual(0, curve1.Elements[3].NodeStart.x, delta);
            Assert.AreEqual(0, curve1.Elements[3].NodeStart.y, delta);
        }

        [TestMethod]
        public void SplitOnceSecondSegment()
        {
            // Act
            Func<FillCurve<FillSegment>> createFillCurveF = () => new FillCurve<FillSegment>();
            var result = CreateTriangleCCW().SplitAtDistances(new double[] { 5 });

            // Assert
            Assert.AreEqual(2, result.Count);

            var curve0 = result[0];
            Assert.AreEqual(3, curve0.Elements.Count);
            Assert.AreEqual(0, curve0.Elements[0].NodeStart.x, delta);
            Assert.AreEqual(0, curve0.Elements[0].NodeStart.y, delta);
            Assert.AreEqual(4, curve0.Elements[1].NodeStart.x, delta);
            Assert.AreEqual(0, curve0.Elements[1].NodeStart.y, delta);
            Assert.AreEqual(4, curve0.Elements[2].NodeStart.x, delta);
            Assert.AreEqual(1, curve0.Elements[2].NodeStart.y, delta);

            var curve1 = result[1];
            Assert.AreEqual(3, curve1.Elements.Count);
            Assert.AreEqual(4, curve1.Elements[0].NodeStart.x, delta);
            Assert.AreEqual(1, curve1.Elements[0].NodeStart.y, delta);
            Assert.AreEqual(4, curve1.Elements[1].NodeStart.x, delta);
            Assert.AreEqual(3, curve1.Elements[1].NodeStart.y, delta);
            Assert.AreEqual(0, curve1.Elements[2].NodeStart.x, delta);
            Assert.AreEqual(0, curve1.Elements[2].NodeStart.y, delta);
        }

        [TestMethod]
        public void SplitTwiceFirstSegment()
        {
            // Act
            Func<FillCurve<FillSegment>> createFillCurveF = () => new FillCurve<FillSegment>();
            var result = CreateTriangleCCW().SplitAtDistances(new double[] { 1, 3 });

            // Assert
            Assert.AreEqual(3, result.Count);

            var curve0 = result[0];
            Assert.AreEqual(2, curve0.Elements.Count);
            Assert.AreEqual(0, curve0.Elements[0].NodeStart.x, delta);
            Assert.AreEqual(0, curve0.Elements[0].NodeStart.y, delta);
            Assert.AreEqual(1, curve0.Elements[1].NodeStart.x, delta);
            Assert.AreEqual(0, curve0.Elements[1].NodeStart.y, delta);

            var curve1 = result[1];
            Assert.AreEqual(2, curve1.Elements.Count);
            Assert.AreEqual(1, curve1.Elements[0].NodeStart.x, delta);
            Assert.AreEqual(0, curve1.Elements[0].NodeStart.y, delta);
            Assert.AreEqual(3, curve1.Elements[1].NodeStart.x, delta);
            Assert.AreEqual(0, curve1.Elements[1].NodeStart.y, delta);

            var curve2 = result[2];
            Assert.AreEqual(4, curve2.Elements.Count);
            Assert.AreEqual(3, curve2.Elements[0].NodeStart.x, delta);
            Assert.AreEqual(0, curve2.Elements[0].NodeStart.y, delta);
            Assert.AreEqual(4, curve2.Elements[1].NodeStart.x, delta);
            Assert.AreEqual(0, curve2.Elements[1].NodeStart.y, delta);
            Assert.AreEqual(4, curve2.Elements[2].NodeStart.x, delta);
            Assert.AreEqual(3, curve2.Elements[2].NodeStart.y, delta);
            Assert.AreEqual(0, curve2.Elements[3].NodeStart.x, delta);
            Assert.AreEqual(0, curve2.Elements[3].NodeStart.y, delta);
        }

        [TestMethod]
        public void SplitTwiceFirstSegmentConnect()
        {
            // Act
            Func<FillCurve<FillSegment>> createFillCurveF = () => new FillCurve<FillSegment>();
            var result = CreateTriangleCCW().SplitAtDistances(new double[] { 1, 3 });

            // Assert
            Assert.AreEqual(2, result.Count);

            var curve0 = result[0];
            Assert.AreEqual(2, curve0.Elements.Count);
            Assert.AreEqual(1, curve0.Elements[0].NodeStart.x, delta);
            Assert.AreEqual(0, curve0.Elements[0].NodeStart.y, delta);
            Assert.AreEqual(3, curve0.Elements[1].NodeStart.x, delta);
            Assert.AreEqual(0, curve0.Elements[1].NodeStart.y, delta);

            var curve1 = result[1];
            Assert.AreEqual(5, curve1.Elements.Count);
            Assert.AreEqual(3, curve1.Elements[0].NodeStart.x, delta);
            Assert.AreEqual(0, curve1.Elements[0].NodeStart.y, delta);
            Assert.AreEqual(4, curve1.Elements[1].NodeStart.x, delta);
            Assert.AreEqual(0, curve1.Elements[1].NodeStart.y, delta);
            Assert.AreEqual(4, curve1.Elements[2].NodeStart.x, delta);
            Assert.AreEqual(3, curve1.Elements[2].NodeStart.y, delta);
            Assert.AreEqual(0, curve1.Elements[3].NodeStart.x, delta);
            Assert.AreEqual(0, curve1.Elements[3].NodeStart.y, delta);
            Assert.AreEqual(1, curve1.Elements[4].NodeStart.x, delta);
            Assert.AreEqual(0, curve1.Elements[4].NodeStart.y, delta);
        }
    }
}