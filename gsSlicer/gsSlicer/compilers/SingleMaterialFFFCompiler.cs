using g3;
using System;
using System.Collections.Generic;

namespace gs
{
    // [TODO] be able to not hardcode this type?
    using LinearToolpath = LinearToolpath3<PrintVertex>;

    public interface ICNCCompiler
    {
    }

    public interface IThreeAxisPrinterCompiler : ICNCCompiler
    {
        // current nozzle position
        Vector3d NozzlePosition { get; }

        // compiler will call this to emit status messages / etc
        Action<string> EmitMessageF { get; set; }

        void Begin();

        void AppendPaths(ToolpathSet paths, SingleMaterialFFFSettings pathSettings);

        void AppendComment(string comment);

        void End();

        IEnumerable<string> GenerateTotalExtrusionReport(SingleMaterialFFFSettings settings);
    }

    public class SingleMaterialFFFCompiler : IThreeAxisPrinterCompiler
    {
        private SingleMaterialFFFSettings Settings;
        private GCodeBuilder Builder;
        protected BaseDepositionAssembler Assembler;

        protected AssemblerFactoryF AssemblerF;

        /// <summary>
        /// compiler will call this to emit status messages / etc
        /// </summary>
        public virtual Action<string> EmitMessageF { get; set; }

        private FeatureTypeLabeler featureTypeLabeler;

        public SingleMaterialFFFCompiler(GCodeBuilder builder, SingleMaterialFFFSettings settings, AssemblerFactoryF AssemblerF)
        {
            Builder = builder;
            Settings = settings;
            this.AssemblerF = AssemblerF;
            featureTypeLabeler = CreateFeatureTypeLabeler();
        }

        protected virtual FeatureTypeLabeler CreateFeatureTypeLabeler()
        {
            return FeatureTypeLabelerFFF.Value;
        }

        public Vector3d NozzlePosition
        {
            get { return Assembler.NozzlePosition; }
        }

        public double ExtruderA
        {
            get { return Assembler.ExtruderA; }
        }

        public bool InRetract
        {
            get { return Assembler.InRetract; }
        }

        public bool InTravel
        {
            get { return Assembler.InTravel; }
        }

        public virtual void Begin()
        {
            Assembler = AssemblerF(Builder, Settings);
            Assembler.AppendComment("---BEGIN HEADER");
            Assembler.AppendHeader();
            Assembler.AppendComment("---END HEADER");
        }

        public virtual void End()
        {
            Assembler.FlushQueues();

            Assembler.UpdateProgress(100);
            Assembler.AppendFooter();
        }

        public virtual void HandleDepositionPath(LinearToolpath path, SingleMaterialFFFSettings useSettings)
        {
            // end travel / retract if we are in that state
            if (Assembler.InTravel)
            {
                if (Assembler.InRetract)
                {
                    Assembler.EndRetract(path[0].Position, useSettings.RetractSpeed, path[0].Extrusion.x);
                }
                Assembler.EndTravel();
                Assembler.EnableFan();
            }
        }

        public virtual void HandleTravelAndPlaneChangePath(LinearToolpath path, int pathIndex, SingleMaterialFFFSettings useSettings)
        {
            if (Assembler.InTravel == false)
            {
                Assembler.DisableFan();

                // do retract cycle
                if (path[0].Extrusion.x < Assembler.ExtruderA)
                {
                    if (Assembler.InRetract)
                        throw new Exception("SingleMaterialFFFCompiler.AppendPaths: path " + pathIndex + ": already in retract!");
                    Assembler.BeginRetract(path[0].Position, useSettings.RetractSpeed, path[0].Extrusion.x);
                }
                Assembler.BeginTravel();
            }
        }

        public virtual void HandleDepositionEnd()
        {
        }

        private string PreviousTag = "";

        /// <summary>
        /// Compile this set of toolpaths and pass to assembler.
        /// Settings are optional, pass null to ignore
        /// </summary>
		public virtual void AppendPaths(ToolpathSet paths, SingleMaterialFFFSettings pathSettings)
        {
            Assembler.FlushQueues();

            SingleMaterialFFFSettings useSettings = (pathSettings == null) ? Settings : pathSettings;

            CalculateExtrusion calc = new CalculateExtrusion(paths, useSettings);
            calc.Calculate(Assembler.NozzlePosition, Assembler.ExtruderA, Assembler.InRetract);

            int path_index = 0;
            foreach (var gpath in paths)
            {
                path_index++;

                if (IsCommandToolpath(gpath))
                {
                    ProcessCommandToolpath(gpath);
                    continue;
                }

                LinearToolpath p = gpath as LinearToolpath;

                if (p[0].Position.Distance(Assembler.NozzlePosition) > 0.00001)
                    throw new Exception("SingleMaterialFFFCompiler.AppendPaths: path " + path_index + ": Start of path is not same as end of previous path!");

                int i = 0;
                if (p.Type == ToolpathTypes.Travel || p.Type == ToolpathTypes.PlaneChange)
                {
                    HandleTravelAndPlaneChangePath(p, path_index, useSettings);
                }
                else if (p.Type == ToolpathTypes.Deposition)
                {
                    HandleDepositionPath(p, useSettings);
                }

                i = 1;      // do not need to emit code for first point of path,
                            // we are already at this pos

                var currentDimensions = p[1].Dimensions;
                if (p.Type == ToolpathTypes.Deposition)
                {
                    AddFeatureTypeLabel(p.TypeModifiers);
                    AppendDimensions(currentDimensions);
                }

                for (; i < p.VertexCount; ++i)
                {
                    if (p.Type == ToolpathTypes.Travel)
                    {
                        Assembler.AppendMoveTo(p[i].Position, p[i].FeedRate, "Travel");
                    }
                    else if (p.Type == ToolpathTypes.PlaneChange)
                    {
                        Assembler.AppendMoveTo(p[i].Position, p[i].FeedRate, "Plane Change");
                    }
                    else
                    {
                        if (p.Type == ToolpathTypes.Deposition && !p[i].Dimensions.EpsilonEqual(currentDimensions, 1e-6))
                        {
                            currentDimensions = p[i].Dimensions;
                            AppendDimensions(p[i].Dimensions);
                        }
                        Assembler.AppendExtrudeTo(p[i].Position, p[i].FeedRate, p[i].Extrusion.x, null);
                    }
                }
            }

            /*
             * TODO: Should there be an EndTravel() call here?
             */
            HandleDepositionEnd();
            Assembler.FlushQueues();
        }

        // annotate paths with S3D compatible (mostly) comments
        private void AddFeatureTypeLabel(FillTypeFlags typeModifier)
        {
            var featureLabel = featureTypeLabeler.FeatureLabelFromFillTypeFlag(typeModifier);

            Builder.AddExplicitLine("");
            Builder.AddCommentLine(" feature " + featureLabel);
        }

        private void AppendDimensions(Vector2d dimensions)
        {
            if (dimensions.x == GCodeUtil.UnspecifiedDimensions.x)
                dimensions.x = Settings.Machine.NozzleDiamMM;
            if (dimensions.y == GCodeUtil.UnspecifiedDimensions.y)
                dimensions.y = Settings.LayerHeightMM;
            Assembler.AppendComment(" tool H" + dimensions.y + " W" + dimensions.x);
        }

        public virtual void AppendComment(string comment)
        {
            Assembler.AppendComment(comment);
        }

        /// <summary>
        /// Command toolpaths are used to pass special commands/etc to the Assembler.
        /// The positions will be ignored
        /// </summary>
        protected virtual bool IsCommandToolpath(IToolpath toolpath)
        {
            return toolpath.Type == ToolpathTypes.Custom
                || toolpath.Type == ToolpathTypes.CustomAssemblerCommands;
        }

        /// <summary>
        /// Called on toolpath if IsCommandToolpath() returns true
        /// </summary>
        protected virtual void ProcessCommandToolpath(IToolpath toolpath)
        {
            if (toolpath.Type == ToolpathTypes.CustomAssemblerCommands)
            {
                AssemblerCommandsToolpath assembler_path = toolpath as AssemblerCommandsToolpath;
                if (assembler_path != null && assembler_path.AssemblerF != null)
                {
                    assembler_path.AssemblerF(Assembler, this);
                }
                else
                {
                    emit_message("ProcessCommandToolpath: invalid " + toolpath.Type.ToString());
                }
            }
            else
            {
                emit_message("ProcessCommandToolpath: unhandled type " + toolpath.Type.ToString());
            }
        }

        protected virtual void emit_message(string text, params object[] args)
        {
            if (EmitMessageF != null)
                EmitMessageF(string.Format(text, args));
        }

        public IEnumerable<string> GenerateTotalExtrusionReport(SingleMaterialFFFSettings settings)
        {
            return Assembler.GenerateTotalExtrusionReport(settings);
        }
    }
}