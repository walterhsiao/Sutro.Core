using g3;
using gs.FillTypes;
using System;
using System.Collections.Generic;

namespace gs
{
    public abstract class FillBase
    {
        // General Properties
        public IFillType FillType { get; set; } = new DefaultFillType();
        public double FillThickness { get; set; }
        public bool IsHoleShell { get; set; } = false;
        public int PerimOrder { get; set; } = -1;

        public virtual void CopyProperties(FillBase other)
        {
            FillType = other.FillType;
            FillThickness = other.FillThickness;
            IsHoleShell = other.IsHoleShell;
            PerimOrder = other.PerimOrder;
        }


        public abstract Vector2d Entry { get; }
        public abstract Vector2d Exit { get; }
        public abstract int ElementCount { get; }

        public abstract double TotalLength();
        public abstract double FindClosestElementToPoint(Vector2d point, out ElementLocation location);
    }
}