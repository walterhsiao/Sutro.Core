using Sutro.Core.Models.GCode;
using System;
using System.Collections.Generic;

namespace gs
{
    public class GCodeBuilder
    {
        public IGCodeAccumulator Target;

        public GCodeBuilder(IGCodeAccumulator target)
        {
            Target = target;
        }

        public virtual GCodeBuilder AddLine(GCodeLine line, bool bClone = true)
        {
            close_current_line();
            if (bClone)
            {
                GCodeLine clone = line.Clone();
                clone.LineNumber = next_line_number();
                Target.AddLine(clone);
            }
            else
            {
                line.LineNumber = next_line_number();
                Target.AddLine(line);
            }
            return this;
        }

        /// <summary>
        /// Add a comment to output. Will add necessary comment prefix if not provided.
        /// </summary>
        public virtual GCodeBuilder AddCommentLine(string comment)
        {
            //if ( comment[0] != ';' && comment[0] != '(' )
            //	comment = ";" + comment;
            AddLine(
                new GCodeLine(next_line_number(), GCodeLine.LType.Comment, comment));
            return this;
        }

        /// <summary>
        /// Add this exact string to output. not a good idea.
        /// </summary>
        public virtual GCodeBuilder AddExplicitLine(string line)
        {
            AddLine(
                new GCodeLine(next_line_number(), GCodeLine.LType.UnknownString, line));
            return this;
        }

        /// <summary>
        /// Add empty line to output.
        /// </summary>
        public virtual GCodeBuilder AddBlankLine()
        {
            AddLine(
                new GCodeLine(next_line_number(), GCodeLine.LType.Blank));
            return this;
        }

        /// <summary>
        /// Open a G code line. Use AppendXParameter to add more info
        /// </summary>
        public virtual GCodeBuilder BeginGLine(int Gcode, string comment = null)
        {
            begin_new_line(GCodeLine.LType.GCode);
            next_line.Code = Gcode;
            if (comment != null)
                next_line.Comment = comment;
            return this;
        }

        /// <summary>
        /// Open a M code line. Use AppendXParameter to add more info
        /// </summary>
        public virtual GCodeBuilder BeginMLine(int Mcode, string comment = null)
        {
            begin_new_line(GCodeLine.LType.MCode);
            next_line.Code = Mcode;
            if (comment != null)
                next_line.Comment = comment;
            return this;
        }

        /// <summary>
        /// close and append current line. In most cases this does not have to be explicitly called.
        /// </summary>
        public virtual GCodeBuilder EndLine()
        {
            close_current_line();
            return this;
        }

        public virtual GCodeBuilder AppendComment(string comment)
        {
            if (next_line == null)
                throw new Exception("GCodeBuilder.AppendComment: next_line was null!?");
            next_line.Comment = comment;
            return this;
        }

        /// <summary>
        /// Add an integer-value parameter to the current line
        /// </summary>
        public virtual GCodeBuilder AppendI(string identifier, int value)
        {
            GCodeParam p = new GCodeParam()
            {
                Type = GCodeParamTypes.IntegerValue,
                Identifier = identifier,
                IntegerValue = value
            };
            next_params.Add(p);
            return this;
        }

        /// <summary>
        /// Add a float-value parameter to the current line
        /// </summary>
        public virtual GCodeBuilder AppendF(string identifier, double value)
        {
            GCodeParam p = new GCodeParam()
            {
                Type = GCodeParamTypes.DoubleValue,
                Identifier = identifier,
                DoubleValue = value
            };
            next_params.Add(p);
            return this;
        }

        /// <summary>
        /// Add a text-value parameter to the current line
        /// (these are used in Biesse gcode...can also use this to string-format values yourself)
        /// </summary>
        public virtual GCodeBuilder AppendS(string identifier, string value)
        {
            GCodeParam p = new GCodeParam()
            {
                Type = GCodeParamTypes.TextValue,
                Identifier = identifier,
                TextValue = value
            };
            next_params.Add(p);
            return this;
        }

        /// <summary>
        /// Append a label-only parameter to the current line (eg like G162 X Y)
        /// </summary>
        /// <param name="identifier">Identifier.</param>
        public virtual GCodeBuilder AppendL(string identifier)
        {
            GCodeParam p = new GCodeParam()
            {
                Type = GCodeParamTypes.NoValue,
                Identifier = identifier
            };
            next_params.Add(p);
            return this;
        }

        private int line_number = 0;

        protected virtual int next_line_number()
        {
            return line_number++;
        }

        private GCodeLine next_line;
        private List<GCodeParam> next_params;

        protected virtual void close_current_line()
        {
            if (next_line != null)
            {
                if (next_params.Count > 0)
                    next_line.Parameters = next_params.ToArray();
                Target.AddLine(next_line);
                next_line = null;
            }
        }

        protected virtual void begin_new_line(GCodeLine.LType type)
        {
            if (next_line != null)
                close_current_line();

            next_line = new GCodeLine(next_line_number(), type);
            if (next_params == null || next_params.Count > 0)
                next_params = new List<GCodeParam>();
        }
    }
}