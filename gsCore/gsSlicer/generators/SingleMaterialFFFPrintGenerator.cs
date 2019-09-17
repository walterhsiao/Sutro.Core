using System;

namespace gs
{

    public class SingleMaterialFFFPrintGenerator : ThreeAxisPrintGenerator, IPrintGeneratorInitialize
    {
        GCodeFileAccumulator file_accumulator;
        GCodeBuilder builder;
        SingleMaterialFFFCompiler compiler;

        public SingleMaterialFFFPrintGenerator() { }

        public SingleMaterialFFFPrintGenerator(PrintMeshAssembly meshes, 
                                      PlanarSliceStack slices,
                                      SingleMaterialFFFSettings settings,
                                      AssemblerFactoryF overrideAssemblerF = null )
        {
            file_accumulator = new GCodeFileAccumulator();
            builder = new GCodeBuilder(file_accumulator);
            AssemblerFactoryF useAssembler = (overrideAssemblerF != null) ?
                overrideAssemblerF : settings.AssemblerType();
            compiler = new SingleMaterialFFFCompiler(builder, settings, useAssembler);
            base.Initialize(meshes, slices, settings, compiler);
        }

        public void Initialize(PrintMeshAssembly meshes, PlanarSliceStack slices, SingleMaterialFFFSettings settings, AssemblerFactoryF overrideAssemblerF)
        {
            throw new NotImplementedException();
        }

        protected override GCodeFile extract_result()
        {
            return file_accumulator.File;
        }


    }


}
