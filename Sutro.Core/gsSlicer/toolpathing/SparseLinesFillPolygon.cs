﻿using g3;
using gs.FillTypes;

namespace gs
{
    /// <summary>
    /// configure dense-fill for sparse infill
    /// </summary>
    public class SparseLinesFillPolygon : ParallelLinesFillPolygon
    {
        public SparseLinesFillPolygon(GeneralPolygon2d poly, IFillType fillType) : base(poly, fillType)
        {
            SimplifyAmount = SimplificationLevel.Moderate;
        }
    }

    /// <summary>
    /// configure dense-fill for support fill
    /// </summary>
    public class SupportLinesFillPolygon : ParallelLinesFillPolygon
    {
        public SupportLinesFillPolygon(GeneralPolygon2d poly, SingleMaterialFFFSettings settings) : base(poly, settings.FillTypeFactory.Support())
        {
            SimplifyAmount = SimplificationLevel.Aggressive;
        }
    }

    /// <summary>
    /// configure dense-fill for bridge fill
    /// </summary>
    public class BridgeLinesFillPolygon : ParallelLinesFillPolygon
    {
        public BridgeLinesFillPolygon(GeneralPolygon2d poly, SingleMaterialFFFSettings settings) : base(poly, settings.FillTypeFactory.Bridge())
        {
            SimplifyAmount = SimplificationLevel.Minor;
        }
    }
}