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
        private static double delta = 1e-4;

        private BasicFillLoop CreateTriangleCCW()
        {
            return new BasicFillLoop(new Vector2d[] {
                new Vector2d(0, 0),
                new Vector2d(4, 0),
                new Vector2d(4, 3),
            });
        }

        private BasicFillLoop CreateTriangleCW()
        {
            return new BasicFillLoop(new Vector2d[] {
                new Vector2d(4, 3),
                new Vector2d(4, 0),
                new Vector2d(0, 0),
            });
        }


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
                new Vector2d(2,0),
                new Vector2d(2,4),
                new Vector2d(0,4)
            };

            // Act
            var loop = new BasicFillLoop(vertices);

            // Assert
            Assert.AreEqual(3, loop.VertexCount);
            Assert.AreEqual(3, loop.SegmentCount);
            Assert.AreEqual(2, loop[0].x);
        }

        [TestMethod]
        public void RollNoEffectCCW()
        {
            // Arrange
            var triangle = CreateTriangleCCW();
            var result = new BasicFillLoop();

            // Act
            triangle.RollMidSegment(0, -triangle.GetSegment2dAfterVertex(0).Extent, result);

            // Assert
            Assert.AreEqual(3, result.VertexCount);

            Assert.AreEqual(0, result[0].x, delta);
            Assert.AreEqual(0, result[0].y, delta);

            Assert.AreEqual(4, result[1].x, delta);
            Assert.AreEqual(0, result[1].y, delta);

            Assert.AreEqual(4, result[2].x, delta);
            Assert.AreEqual(3, result[2].y, delta);
        }

        [TestMethod]
        public void RollNoEffectCW()
        {
            // Arrange
            var triangle = CreateTriangleCW();
            var result = new BasicFillLoop();

            // Act
            triangle.RollMidSegment(0, -triangle.GetSegment2dAfterVertex(0).Extent, result);

            // Assert
            Assert.AreEqual(3, result.VertexCount);

            Assert.AreEqual(4, result[0].x, delta);
            Assert.AreEqual(3, result[0].y, delta);

            Assert.AreEqual(4, result[1].x, delta);
            Assert.AreEqual(0, result[1].y, delta);

            Assert.AreEqual(0, result[2].x, delta);
            Assert.AreEqual(0, result[2].y, delta);
        }

        [TestMethod]
        public void RollFirstSegmentHalfwayCCW()
        {
            // Arrange
            var triangle = CreateTriangleCCW();
            var result = new BasicFillLoop();

            // Act
            triangle.RollMidSegment(0, 0, result);

            // Assert
            Assert.AreEqual(4, result.VertexCount);

            Assert.AreEqual(2, result[0].x, delta);
            Assert.AreEqual(0, result[0].y, delta);

            Assert.AreEqual(4, result[1].x, delta);
            Assert.AreEqual(0, result[1].y, delta);

            Assert.AreEqual(4, result[2].x, delta);
            Assert.AreEqual(3, result[2].y, delta);

            Assert.AreEqual(0, result[3].x, delta);
            Assert.AreEqual(0, result[3].y, delta);
        }


        [TestMethod]
        public void RollFirstSegmentHalfwayCW()
        {
            // Arrange
            var triangle = CreateTriangleCW();
            var result = new BasicFillLoop();

            // Act
            triangle.RollMidSegment(0, 0, result);

            // Assert
            Assert.AreEqual(4, result.VertexCount);

            Assert.AreEqual(4, result[0].x, delta);
            Assert.AreEqual(1.5, result[0].y, delta);

            Assert.AreEqual(4, result[1].x, delta);
            Assert.AreEqual(0, result[1].y, delta);

            Assert.AreEqual(0, result[2].x, delta);
            Assert.AreEqual(0, result[2].y, delta);

            Assert.AreEqual(4, result[3].x, delta);
            Assert.AreEqual(3, result[3].y, delta);
        }

        [TestMethod]
        public void RollSecondSegmentHalfwayCCW()
        {

            // Arrange
            var triangle = CreateTriangleCCW();
            var result = new BasicFillLoop();

            // Act
            triangle.RollMidSegment(1, -0.5, result);

            // Assert
            Assert.AreEqual(4, result.VertexCount);

            Assert.AreEqual(4, result[0].x, delta);
            Assert.AreEqual(1, result[0].y, delta);

            Assert.AreEqual(4, result[1].x, delta);
            Assert.AreEqual(3, result[1].y, delta);

            Assert.AreEqual(0, result[2].x, delta);
            Assert.AreEqual(0, result[2].y, delta);

            Assert.AreEqual(4, result[3].x, delta);
            Assert.AreEqual(0, result[3].y, delta);
        }

        [TestMethod]
        public void RollSecondSegmentHalfwayCW()
        {

            // Arrange
            var triangle = CreateTriangleCW();
            var result = new BasicFillLoop();

            // Act
            triangle.RollMidSegment(1, 0, result);
            
            // Assert
            Assert.AreEqual(4, result.VertexCount);

            Assert.AreEqual(2, result[0].x, delta);
            Assert.AreEqual(0, result[0].y, delta);

            Assert.AreEqual(0, result[1].x, delta);
            Assert.AreEqual(0, result[1].y, delta);

            Assert.AreEqual(4, result[2].x, delta);
            Assert.AreEqual(3, result[2].y, delta);

            Assert.AreEqual(4, result[3].x, delta);
            Assert.AreEqual(0, result[3].y, delta);
        }

        [TestMethod]
        public void RollFirstSegmentWithinToleranceLowSideCCW()
        {

            // Arrange
            var triangle = CreateTriangleCCW();
            var result = new BasicFillLoop();

            // Act
            triangle.RollMidSegment(0, -1.999, result, 0.005);

            // Assert
            Assert.AreEqual(3, result.VertexCount);

            Assert.AreEqual(0, result[0].x, delta);
            Assert.AreEqual(0, result[0].y, delta);

            Assert.AreEqual(4, result[1].x, delta);
            Assert.AreEqual(0, result[1].y, delta);

            Assert.AreEqual(4, result[2].x, delta);
            Assert.AreEqual(3, result[2].y, delta);
        }

        [TestMethod]
        public void RollFirstSegmentWithinToleranceLowSideCW()
        {

            // Arrange
            var triangle = CreateTriangleCW();
            var result = new BasicFillLoop();

            // Act
            triangle.RollMidSegment(1, -1.999, result, 0.005);

            // Assert
            Assert.AreEqual(3, result.VertexCount);

            Assert.AreEqual(4, result[0].x, delta);
            Assert.AreEqual(0, result[0].y, delta);

            Assert.AreEqual(0, result[1].x, delta);
            Assert.AreEqual(0, result[1].y, delta);

            Assert.AreEqual(4, result[2].x, delta);
            Assert.AreEqual(3, result[2].y, delta);
        }


        [TestMethod]
        public void RollFirstSegmentWithinToleranceHighSideCCW()
        {
            // Arrange
            var triangle = CreateTriangleCCW();
            var result = new BasicFillLoop();

            // Act
            triangle.RollMidSegment(0, 1.999, result, 0.005);

            // Assert
            Assert.AreEqual(3, result.VertexCount);

            Assert.AreEqual(4, result[0].x, delta);
            Assert.AreEqual(0, result[0].y, delta);

            Assert.AreEqual(4, result[1].x, delta);
            Assert.AreEqual(3, result[1].y, delta);

            Assert.AreEqual(0, result[2].x, delta);
            Assert.AreEqual(0, result[2].y, delta);
        }

        [TestMethod]
        public void RollFirstSegmentWithinToleranceHighSideCW()
        {
            // Arrange
            var triangle = CreateTriangleCW();
            var result = new BasicFillLoop();

            // Act
            triangle.RollMidSegment(1, 1.999, result, 0.005);

            // Assert
            Assert.AreEqual(3, result.VertexCount);

            Assert.AreEqual(0, result[0].x, delta);
            Assert.AreEqual(0, result[0].y, delta);

            Assert.AreEqual(4, result[1].x, delta);
            Assert.AreEqual(3, result[1].y, delta);

            Assert.AreEqual(4, result[2].x, delta);
            Assert.AreEqual(0, result[2].y, delta);
        }

        [TestMethod]
        public void ConvertToCurve()
        {
            // Act
            var result = new BasicFillCurve();
            CreateTriangleCCW().ConvertToCurve(result);

            // Assert
            Assert.AreEqual(4, result.VertexCount);

            Assert.AreEqual(0, result[0].x, delta);
            Assert.AreEqual(0, result[0].y, delta);

            Assert.AreEqual(4, result[1].x, delta);
            Assert.AreEqual(0, result[1].y, delta);

            Assert.AreEqual(4, result[2].x, delta);
            Assert.AreEqual(3, result[2].y, delta);

            Assert.AreEqual(0, result[3].x, delta);
            Assert.AreEqual(0, result[3].y, delta);
        }

        [TestMethod]
        public void SplitOnceFirstSegment()
        {
            // Act
            var result = new List<BasicFillCurve>();
            Func<BasicFillCurve> createFillCurveF = () => new BasicFillCurve();
            CreateTriangleCCW().SplitAtDistances(new double[] { 3 }, result, createFillCurveF);

            // Assert
            Assert.AreEqual(2, result.Count);

            var curve0 = result[0];
            Assert.AreEqual(2, curve0.VertexCount);
            Assert.AreEqual(0, curve0[0].x, delta);
            Assert.AreEqual(0, curve0[0].y, delta);
            Assert.AreEqual(3, curve0[1].x, delta);
            Assert.AreEqual(0, curve0[1].y, delta);

            var curve1 = result[1];
            Assert.AreEqual(4, curve1.VertexCount);
            Assert.AreEqual(3, curve1[0].x, delta);
            Assert.AreEqual(0, curve1[0].y, delta);
            Assert.AreEqual(4, curve1[1].x, delta);
            Assert.AreEqual(0, curve1[1].y, delta);
            Assert.AreEqual(4, curve1[2].x, delta);
            Assert.AreEqual(3, curve1[2].y, delta);
            Assert.AreEqual(0, curve1[3].x, delta);
            Assert.AreEqual(0, curve1[3].y, delta);
        }

        [TestMethod]
        public void SplitOnceSecondSegment()
        {
            // Act
            var result = new List<BasicFillCurve>();
            Func<BasicFillCurve> createFillCurveF = () => new BasicFillCurve();
            CreateTriangleCCW().SplitAtDistances(new double[] { 5 }, result, createFillCurveF);

            // Assert
            Assert.AreEqual(2, result.Count);

            var curve0 = result[0];
            Assert.AreEqual(3, curve0.VertexCount);
            Assert.AreEqual(0, curve0[0].x, delta);
            Assert.AreEqual(0, curve0[0].y, delta);
            Assert.AreEqual(4, curve0[1].x, delta);
            Assert.AreEqual(0, curve0[1].y, delta);
            Assert.AreEqual(4, curve0[2].x, delta);
            Assert.AreEqual(1, curve0[2].y, delta);

            var curve1 = result[1];
            Assert.AreEqual(3, curve1.VertexCount);
            Assert.AreEqual(4, curve1[0].x, delta);
            Assert.AreEqual(1, curve1[0].y, delta);
            Assert.AreEqual(4, curve1[1].x, delta);
            Assert.AreEqual(3, curve1[1].y, delta);
            Assert.AreEqual(0, curve1[2].x, delta);
            Assert.AreEqual(0, curve1[2].y, delta);
        }

        [TestMethod]
        public void SplitTwiceFirstSegment()
        {
            // Act
            var result = new List<BasicFillCurve>();
            Func<BasicFillCurve> createFillCurveF = () => new BasicFillCurve();
            CreateTriangleCCW().SplitAtDistances(new double[] { 1, 3 }, result, createFillCurveF);

            // Assert
            Assert.AreEqual(3, result.Count);

            var curve0 = result[0];
            Assert.AreEqual(2, curve0.VertexCount);
            Assert.AreEqual(0, curve0[0].x, delta);
            Assert.AreEqual(0, curve0[0].y, delta);
            Assert.AreEqual(1, curve0[1].x, delta);
            Assert.AreEqual(0, curve0[1].y, delta);

            var curve1 = result[1];
            Assert.AreEqual(2, curve1.VertexCount);
            Assert.AreEqual(1, curve1[0].x, delta);
            Assert.AreEqual(0, curve1[0].y, delta);
            Assert.AreEqual(3, curve1[1].x, delta);
            Assert.AreEqual(0, curve1[1].y, delta);

            var curve2 = result[2];
            Assert.AreEqual(4, curve2.VertexCount);
            Assert.AreEqual(3, curve2[0].x, delta);
            Assert.AreEqual(0, curve2[0].y, delta);
            Assert.AreEqual(4, curve2[1].x, delta);
            Assert.AreEqual(0, curve2[1].y, delta);
            Assert.AreEqual(4, curve2[2].x, delta);
            Assert.AreEqual(3, curve2[2].y, delta);
            Assert.AreEqual(0, curve2[3].x, delta);
            Assert.AreEqual(0, curve2[3].y, delta);
        }

        [TestMethod]
        public void SplitTwiceFirstSegmentConnect()
        {
            // Act
            var result = new List<BasicFillCurve>();
            Func<BasicFillCurve> createFillCurveF = () => new BasicFillCurve();
            CreateTriangleCCW().SplitAtDistances(new double[] { 1, 3 }, result, createFillCurveF, true);

            // Assert
            Assert.AreEqual(2, result.Count);

            var curve0 = result[0];
            Assert.AreEqual(2, curve0.VertexCount);
            Assert.AreEqual(1, curve0[0].x, delta);
            Assert.AreEqual(0, curve0[0].y, delta);
            Assert.AreEqual(3, curve0[1].x, delta);
            Assert.AreEqual(0, curve0[1].y, delta);

            var curve1 = result[1];
            Assert.AreEqual(5, curve1.VertexCount);
            Assert.AreEqual(3, curve1[0].x, delta);
            Assert.AreEqual(0, curve1[0].y, delta);
            Assert.AreEqual(4, curve1[1].x, delta);
            Assert.AreEqual(0, curve1[1].y, delta);
            Assert.AreEqual(4, curve1[2].x, delta);
            Assert.AreEqual(3, curve1[2].y, delta);
            Assert.AreEqual(0, curve1[3].x, delta);
            Assert.AreEqual(0, curve1[3].y, delta);
            Assert.AreEqual(1, curve1[4].x, delta);
            Assert.AreEqual(0, curve1[4].y, delta);
        }
    }
}
