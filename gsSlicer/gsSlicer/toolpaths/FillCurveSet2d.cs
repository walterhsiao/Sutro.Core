using g3;
using System;
using System.Collections.Generic;
using System.Linq;
using gs.FillTypes;

namespace gs
{
    /// <summary>
    /// Collection of loop and curve centerlines of fill curves
    ///
    /// [TODO] support thickness variation along curves?
    /// </summary>
	public class FillCurveSet2d
    {
        public List<FillLoop<FillSegment>> Loops = 
            new List<FillLoop<FillSegment>>();
        
        public List<FillCurve<FillSegment>> Curves = 
            new List<FillCurve<FillSegment>>();

        public void Append(GeneralPolygon2d poly, IFillType fillType)
        {
            Loops.Add(new FillLoop<FillSegment>(poly.Outer.VerticesItr(false).ToList()) { FillType = fillType });
            foreach (var h in poly.Holes)
                Loops.Add(new FillLoop<FillSegment>(h.VerticesItr(false).ToList()) { FillType = fillType });
        }

        public void Append(List<GeneralPolygon2d> polys, IFillType fillType)
        {
            foreach (var p in polys)
                Append(p, fillType);
        }

        public void Append(Polygon2d poly, IFillType fillType)
        {
            Loops.Add(new FillLoop<FillSegment>(poly.VerticesItr(false).ToList()) { FillType = fillType });
        }

        public void Append(List<Polygon2d> polys, IFillType fillType)
        {
            foreach (var p in polys)
                Append(p, fillType);
        }

        public IEnumerable<FillBase<FillSegment>> EnumerateAll()
        {
            foreach (var loop in Loops)
                yield return loop;

            foreach (var curve in Curves)
                yield return curve;
        }

        public void Append(FillLoop<FillSegment> loop)
        {
            Loops.Add(loop);
        }

        public void Append(FillBase<FillSegment> element)
        {
            switch (element)
            {
                case FillLoop<FillSegment> loop:
                    Append(loop);
                    break;
                case FillCurve<FillSegment> curve:
                    Append(curve);
                    break;
            }
        }

        public void Append(List<FillLoop<FillSegment>> loops)
        {
            foreach (var l in loops)
                Loops.Add(l);
        }

        public void Append(FillCurve<FillSegment> curve)
        {
            Curves.Add(curve);
        }

        public void Append(List<FillCurve<FillSegment>> curves)
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
                len += loop.TotalLength();
            foreach (var curve in Curves)
                len += curve.TotalLength();
            return len;
        }
    }
}