using g3;
using System;
using System.Collections.Generic;

namespace gs
{
    /// <summary>
    /// Collection of loop and curve centerlines of fill curves
    ///
    /// [TODO] support thickness variation along curves?
    /// </summary>
	public class FillCurveSet2d
    {
        public List<FillLoopBase<BasicSegmentInfo>> Loops = 
            new List<FillLoopBase<BasicSegmentInfo>>();
        
        public List<FillCurveBase<BasicSegmentInfo>> Curves = 
            new List<FillCurveBase<BasicSegmentInfo>>();

        public void Append(GeneralPolygon2d poly, IFillType fillType)
        {
            Loops.Add(new BasicFillLoop(poly.Outer.VerticesItr(false)) { FillType = fillType });
            foreach (var h in poly.Holes)
                Loops.Add(new BasicFillLoop(h.VerticesItr(false)) { FillType = fillType });
        }

        public void Append(List<GeneralPolygon2d> polys, IFillType fillType)
        {
            foreach (var p in polys)
                Append(p, fillType);
        }

        public void Append(Polygon2d poly, IFillType fillType)
        {
            Loops.Add(new BasicFillLoop(poly.VerticesItr(false)) { FillType = fillType });
        }

        public void Append(List<Polygon2d> polys, IFillType fillType)
        {
            foreach (var p in polys)
                Append(p, fillType);
        }

        public IEnumerable<FillElementBase<BasicSegmentInfo>> EnumerateAll()
        {
            foreach (var loop in Loops)
                yield return loop;

            foreach (var curve in Curves)
                yield return curve;
        }

        public void Append(FillLoopBase<BasicSegmentInfo> loop)
        {
            Loops.Add(loop);
        }

        public void Append(FillElementBase<BasicSegmentInfo> element)
        {
            switch (element)
            {
                case FillLoopBase<BasicSegmentInfo> loop:
                    Append(loop);
                    break;
                case FillCurveBase<BasicSegmentInfo> curve:
                    Append(curve);
                    break;
            }
        }

        public void Append(List<FillLoopBase<BasicSegmentInfo>> loops)
        {
            foreach (var l in loops)
                Loops.Add(l);
        }

        public void Append(FillCurveBase<BasicSegmentInfo> curve)
        {
            Curves.Add(curve);
        }

        public void Append(List<FillCurveBase<BasicSegmentInfo>> curves)
        {
            foreach (var p in curves)
                Append(p);
        }

        public void SetFillType(IFillType fillType)
        {
            foreach (var loop in Loops)
                loop.FillType = fillType;

            foreach (var curve in Curves)
                curve.FillType = fillType;
        }

        public double TotalLength()
        {
            double len = 0;
            foreach (var loop in Loops)
                len += loop.Perimeter;
            foreach (var curve in Curves)
                len += curve.ArcLength;
            return len;
        }
    }
}