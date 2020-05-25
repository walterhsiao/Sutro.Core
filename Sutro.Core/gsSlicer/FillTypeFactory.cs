using gs.FillTypes;

namespace gs
{
    public class FillTypeFactory
    {
        private SingleMaterialFFFSettings settings;

        public FillTypeFactory(SingleMaterialFFFSettings settings)
        {
            this.settings = settings;
        }

        public IFillType Bridge()
        {
            return new BridgeFillType(settings.BridgeVolumeScale, settings.CarefulExtrudeSpeed * settings.BridgeExtrudeSpeedX);
        }

        public IFillType Default()
        {
            return new DefaultFillType();
        }

        public IFillType InnerPerimeter()
        {
            return new InnerPerimeterFillType(1, settings.InnerPerimeterSpeedX);
        }

        public IFillType InteriorShell()
        {
            return new InteriorShellFillType();
        }

        public IFillType OpenShellCurve()
        {
            return new OpenShellCurveFillType();
        }

        public IFillType OuterPerimeter()
        {
            return new OuterPerimeterFillType(1, settings.OuterPerimeterSpeedX);
        }

        public IFillType SkirtBrim(SingleMaterialFFFSettings settings)
        {
            return new SkirtBrimFillType();
        }

        public IFillType Solid()
        {
            return new SolidFillType(1, settings.SolidFillSpeedX);
        }

        public IFillType Sparse()
        {
            return new SparseFillType();
        }

        public IFillType Support()
        {
            return new SupportFillType(settings.SupportVolumeScale, settings.OuterPerimeterSpeedX);
        }
    }
}