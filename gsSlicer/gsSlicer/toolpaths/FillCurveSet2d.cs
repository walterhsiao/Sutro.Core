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
        public List<FillLoopBase<FillSegment>> Loops = 
            new List<FillLoopBase<FillSegment>>();
        
        public List<FillCurveBase<FillSegment>> Curves = 
            new List<FillCurveBase<FillSegment>>();

        public void Append(GeneralPolygon2d poly, IFillType fillType)
        {
            Loops.Add(new FillLoopBase<FillSegment>(poly.Outer.VerticesItr(false)) { FillType = fillType });
            foreach (var h in poly.Holes)
                Loops.Add(new FillLoopBase<FillSegment>(h.VerticesItr(false)) { FillType = fillType });
        }

        public void Append(List<GeneralPolygon2d> polys, IFillType fillType)
        {
            foreach (var p in polys)
                Append(p, fillType);
        }

        public void Append(Polygon2d poly, IFillType fillType)
        {
            Loops.Add(new FillLoopBase<FillSegment>(poly.VerticesItr(false)) { FillType = fillType });
        }

        public void Append(List<Polygon2d> polys, IFillType fillType)
        {
            foreach (var p in polys)
                Append(p, fillType);
        }

        public IEnumerable<FillElementBase<FillSegment>> EnumerateAll()
        {
            foreach (var loop in Loops)
                yield return loop;

            foreach (var curve in Curves)
                yield return curve;
        }

        public void Append(FillLoopBase<FillSegment> loop)
        {
            Loops.Add(loop);
        }

        public void Append(FillElementBase<FillSegment> element)
        {
            switch (element)
            {
                case FillLoopBase<FillSegment> loop:
                    Append(loop);
                    break;
                case FillCurveBase<FillSegment> curve:
                    Append(curve);
                    break;
            }
        }

        public void Append(List<FillLoopBase<FillSegment>> loops)
        {
            foreach (var l in loops)
                Loops.Add(l);
        }

        public void Append(FillCurveBase<FillSegment> curve)
        {
            Curves.Add(curve);
        }

        public void Append(List<FillCurveBase<FillSegment>> curves)
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