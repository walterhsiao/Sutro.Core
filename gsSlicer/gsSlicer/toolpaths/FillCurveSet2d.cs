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
        public List<IFillLoop> Loops = new List<IFillLoop>();
        
        public List<IFillCurve> Curves = new List<IFillCurve>();

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

        public IEnumerable<IFill> EnumerateAll()
        {
            foreach (var loop in Loops)
                yield return loop;

            foreach (var curve in Curves)
                yield return curve;
        }

        //public void Append(IFill fill)
        //{
        //    if (!(fill is FillBase<FillSegment> f))
        //        throw new ArgumentException($"Can't append type {fill.GetType()}.");
        //    Append(f);
        //}

        public void Append(IFillLoop loop)
        {
            Loops.Add(loop);
        }

        public void Append(IFill element)
        {
            switch (element)
            {
                case IFillLoop loop:
                    Append(loop);
                    break;
                case IFillCurve curve:
                    Append(curve);
                    break;
                default:
                    throw new ArgumentException($"Can't append type {element.GetType()}.");
            }
        }

        public void Append(List<IFillLoop> loops)
        {
            foreach (var l in loops)
                Loops.Add(l);
        }

        public void Append(IFillCurve curve)
        {
            Curves.Add(curve);
        }

        public void Append(List<IFillCurve> curves)
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