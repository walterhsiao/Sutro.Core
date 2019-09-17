using g3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;

namespace gs
{
    public interface IGenerator
    {
        //void LoadSettings(IEnumerable<string> settingFilesPaths);
        GCodeFile Generate(IEnumerable<Tuple<DMesh3, object>> meshPartPairs);
        void Write(string path, GCodeFile file);
    }

    public interface IGeneratorData
    {
        string Name { get; }
        string Description { get; }
    }


    public abstract class BasicGenerator<TPrintGenerator, TPrintSettings>
            where TPrintGenerator : ThreeAxisPrintGenerator, IPrintGeneratorInitialize, new()
            where TPrintSettings : SingleMaterialFFFSettings, new()
    {

        public static GCodeFile Convert(DMesh3 mesh, TPrintSettings settings, out TPrintGenerator printGenerator, bool silent = false)
        {

            if (!silent) Console.WriteLine("\tCentering mesh above origin...");
            // center mesh above origin
            AxisAlignedBox3d bounds = mesh.CachedBounds;
            Vector3d baseCenterPt = bounds.Center - bounds.Extents.z * Vector3d.AxisZ;
            MeshTransforms.Translate(mesh, -baseCenterPt);

            if (!silent) Console.WriteLine("\tCreating print mesh set...");
            // create print mesh set
            PrintMeshAssembly meshes = new PrintMeshAssembly();
            meshes.AddMesh(mesh, PrintMeshOptions.Default());

            if (!silent) Console.WriteLine("\tSlicing...");
            // do slicing
            MeshPlanarSlicer slicer = new MeshPlanarSlicer()
            {
                LayerHeightMM = settings.LayerHeightMM
            };
            slicer.Add(meshes);
            PlanarSliceStack slices = slicer.Compute();

            if (!silent) Console.WriteLine("\tRunning print generator...");
            // run print generator
            printGenerator = new TPrintGenerator();
            AssemblerFactoryF overrideAssemblerF = null;
            printGenerator.Initialize(meshes, slices, settings, overrideAssemblerF);
            if (printGenerator.Generate())
                return printGenerator.Result;
            else
                throw new Exception("PrintGenerator failed to generate gcode!");
        }

        public static TPrintSettings LoadSettings(IEnumerable<string> settingFilesPaths)
        {

            var settings = new TPrintSettings();

            //JsonSerializerSettings jsonSerializeSettings = new JsonSerializerSettings
            //{
            //    MissingMemberHandling = MissingMemberHandling.Error,

            //};

            //// load settings from files
            //foreach (string s in settingFilesPaths)
            //{
            //    try
            //    {
            //        string settingsText = File.ReadAllText(s);
            //        // TODO: Make this more strict to avoid converting values unintentionally
            //        JsonConvert.PopulateObject(settingsText, settings, jsonSerializeSettings);
            //    }
            //    catch (Exception e)
            //    {
            //        throw new Exception("Error processing settings file: " + Path.GetFullPath(s), e);
            //    }
            //}

            return settings;
        }

        public static GCodeFile LoadGCode(string path)
        {
            //GenericGCodeParser parser = new GenericGCodeParser();
            //using (StreamReader fileReader = File.OpenText(path))
            //    return parser.Parse(fileReader);
            return new GCodeFile();
        }

        public static void WriteGCode(string path, GCodeFile file)
        {
            //using (StreamWriter w = new StreamWriter(path))
            //{
            //    StandardGCodeWriter writer = new StandardGCodeWriter();
            //    writer.WriteFile(file, w);
            //}
        }
    }

    [Export(typeof(IGenerator))]
    [ExportMetadata("Name", "basic")]
    [ExportMetadata("Description", "Provides access to the basic print generator included in gsCore. Can only create gcode for a single mesh with single material.")]
    public class gsCoreGenerator :
        BasicGenerator<SingleMaterialFFFPrintGenerator, SingleMaterialFFFSettings>, IGenerator
    {
        public GCodeFile Generate(IEnumerable<Tuple<DMesh3, object>> meshPartPairs)
        {
            throw new NotImplementedException();
        }

        public void Write(string path, GCodeFile file)
        {
            throw new NotImplementedException();
        }
    }
}
