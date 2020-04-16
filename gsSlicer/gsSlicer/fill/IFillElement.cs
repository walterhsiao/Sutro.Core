using g3;
using System.Collections.Generic;

namespace gs
{
    /// <summary>
    /// Things that are common to FillPolylineGeneric and FillPolylineGeneric
    /// </summary>
    public interface IFillElement
    {
        int VertexCount { get; }

        double CustomThickness { get; set; }

        IFillType FillType { get; set; }

        IEnumerable<Vector2d> Vertices { get; }
        bool IsHoleShell { get; }
        int PerimOrder { get; }

        void CopyProperties(IFillElement other);
    }
}