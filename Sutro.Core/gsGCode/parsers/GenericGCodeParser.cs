using Sutro.Core.Models.GCode;
using System;
using System.IO;
using System.Linq;

namespace gs
{
    public class GenericGCodeParser
    {
        public Func<string, bool> IsMacroF = (token) => { return false; };

        public GCodeFile Parse(TextReader input)
        {
            GCodeFile file = new GCodeFile();

            int lines = 0;
            while (input.Peek() >= 0)
            {
                string line = input.ReadLine();
                int nLineNum = lines++;

                GCodeLine l = ParseLine(line, nLineNum);
                file.AppendLine(l);
            }

            return file;
        }

        // removes the comment from the input line (the line is modified) and returns the comment, including the semicolon
        virtual protected string removeComment(ref string line)
        {
            // strip off trailing comment
            string comment = null;
            int ci = line.IndexOf(';');
            if (ci < 0)
            {
                int bo = line.IndexOf('(');
                int bc = line.IndexOf(')');
                if (bo >= 0 && bc > 0)
                    ci = bo;
            }
            if (ci >= 1)
            {
                comment = line.Substring(ci);
                line = line.Substring(0, ci);
            }
            return comment;
        }

        virtual protected GCodeLine ParseLine(string line, int nLineNum)
        {
            if (line.Length == 0)
                return make_blank(nLineNum);
            if (line[0] == ';')
                return make_comment(line, nLineNum);

            string comment = removeComment(ref line);

            string[] tokens = line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);

            // handle extra spaces at start...?
            if (tokens.Length == 0)
                return make_blank(nLineNum);

            GCodeLine gcode = null;
            if (!IsMacroF(tokens[0]))
            {
                switch (tokens[0][0])
                {
                    case ';':
                        gcode = make_comment(line, nLineNum);
                        break;

                    case 'N':
                        gcode = make_N_code_line(line, tokens, nLineNum);
                        break;

                    case 'G':
                    case 'M':
                        gcode = make_GM_code_line(line, tokens, nLineNum);
                        break;

                    case ':':
                        gcode = make_control_line(line, tokens, nLineNum);
                        break;

                    default:
                        gcode = make_string_line(line, nLineNum);
                        break;
                }
            }
            else
            {
                gcode = make_string_line(line, nLineNum);
            }

            if (comment != null)
                gcode.Comment = comment;

            return gcode;
        }

        // G### and M### code lines
        virtual protected GCodeLine make_GM_code_line(string line, string[] tokens, int nLineNum)
        {
            GCodeLine.LType eType = GCodeLine.LType.UnknownCode;
            if (tokens[0][0] == 'G')
                eType = GCodeLine.LType.GCode;
            else if (tokens[0][0] == 'M')
                eType = GCodeLine.LType.MCode;

            GCodeLine l = new GCodeLine(nLineNum, eType);
            l.OriginalString = line;

            bool valid = int.TryParse(tokens[0].Substring(1), out l.N);

            // if we can't parse it as a valid G or M code, treat the entire line as an unknow string
            if (!valid) return make_string_line(line, nLineNum);

            // [TODO] comments

            if (eType == GCodeLine.LType.UnknownCode)
            {
                if (tokens.Length > 1)
                    l.Parameters = parse_parameters(tokens, 1);
            }
            else
            {
                l.Code = int.Parse(tokens[0].Substring(1));
                if (tokens.Length > 1)
                    l.Parameters = parse_parameters(tokens, 1);
            }

            return l;
        }

        // N### lines
        virtual protected GCodeLine make_N_code_line(string line, string[] tokens, int nLineNum)
        {
            GCodeLine.LType eType = GCodeLine.LType.UnknownCode;
            if (tokens[1][0] == 'G')
                eType = GCodeLine.LType.GCode;
            else if (tokens[1][0] == 'M')
                eType = GCodeLine.LType.MCode;

            GCodeLine l = new GCodeLine(nLineNum, eType);
            l.OriginalString = line;

            bool valid = int.TryParse(tokens[0].Substring(1), out l.N);

            // if we can't parse it as a valid N code, treat the entire line as an unknow string
            if (!valid) return make_string_line(line, nLineNum);

            // [TODO] comments

            if (eType == GCodeLine.LType.UnknownCode)
            {
                if (tokens.Length > 1)
                    l.Parameters = parse_parameters(tokens, 1);
            }
            else
            {
                l.Code = int.Parse(tokens[1].Substring(1));
                if (tokens.Length > 2)
                    l.Parameters = parse_parameters(tokens, 2);
            }

            return l;
        }

        // any line we can't understand
        virtual protected GCodeLine make_string_line(string line, int nLineNum)
        {
            GCodeLine l = new GCodeLine(nLineNum, GCodeLine.LType.UnknownString);
            l.OriginalString = line;
            return l;
        }

        // :IF, :ENDIF, :ELSE
        virtual protected GCodeLine make_control_line(string line, string[] tokens, int nLineNum)
        {
            // figure out command type
            string command = tokens[0].Substring(1);
            GCodeLine.LType eType = GCodeLine.LType.UnknownControl;
            if (command.Equals("if", StringComparison.OrdinalIgnoreCase))
                eType = GCodeLine.LType.If;
            else if (command.Equals("else", StringComparison.OrdinalIgnoreCase))
                eType = GCodeLine.LType.Else;
            else if (command.Equals("endif", StringComparison.OrdinalIgnoreCase))
                eType = GCodeLine.LType.EndIf;

            GCodeLine l = new GCodeLine(nLineNum, eType);
            l.OriginalString = line;

            if (tokens.Length > 1)
                l.Parameters = parse_parameters(tokens, 1);

            return l;
        }

        // line starting with ;
        virtual protected GCodeLine make_comment(string line, int nLineNum)
        {
            GCodeLine l = new GCodeLine(nLineNum, GCodeLine.LType.Comment);

            l.OriginalString = line;
            int iStart = line.IndexOf(';');
            l.Comment = line.Substring(iStart);
            return l;
        }

        // line with no text at all
        virtual protected GCodeLine make_blank(int nLineNum)
        {
            return new GCodeLine(nLineNum, GCodeLine.LType.Blank);
        }

        virtual protected GCodeParam[] parse_parameters(string[] tokens, int iStart, int iEnd = -1)
        {
            if (iEnd == -1)
                iEnd = tokens.Length;

            int N = iEnd - iStart;
            GCodeParam[] paramList = new GCodeParam[N];

            for (int ti = iStart; ti < iEnd; ++ti)
            {
                int pi = ti - iStart;

                bool bHandled = false;
                if (tokens[ti].Contains('='))
                {
                    parse_equals_parameter(tokens[ti], ref paramList[pi]);
                    bHandled = true;
                }
                else if (tokens[ti][0] == 'G' || tokens[ti][0] == 'M')
                {
                    parse_code_parameter(tokens[ti], ref paramList[pi]);
                    bHandled = true;
                }
                else if (is_num_parameter(tokens[ti]) > 0)
                {
                    parse_noequals_num_parameter(tokens[ti], ref paramList[pi]);
                    bHandled = true;
                }
                else if (tokens[ti].Length == 1)
                {
                    paramList[pi].Type = GCodeParamTypes.NoValue;
                    paramList[pi].Identifier = tokens[ti];
                    bHandled = true;
                }

                if (!bHandled)
                {
                    paramList[pi].Type = GCodeParamTypes.TextValue;
                    paramList[pi].TextValue = tokens[ti];
                }
            }

            return paramList;
        }

        virtual protected bool parse_code_parameter(string token, ref GCodeParam param)
        {
            param.Type = GCodeParamTypes.Code;
            param.Identifier = token.Substring(0, 1);

            string value = token.Substring(1);
            GCodeUtil.NumberType numType = GCodeUtil.GetNumberType(value);
            if (numType == GCodeUtil.NumberType.Integer)
                param.IntegerValue = int.Parse(value);

            return true;
        }

        virtual protected int is_num_parameter(string token)
        {
            int N = token.Length;

            bool contains_number = false;
            for (int i = 0; i < N && contains_number == false; ++i)
            {
                if (Char.IsDigit(token[i]))
                    contains_number = true;
            }
            if (!contains_number)
                return -1;

            for (int i = 1; i < N; ++i)
            {
                string sub = token.Substring(i);
                GCodeUtil.NumberType numtype = GCodeUtil.GetNumberType(sub);
                if (numtype != GCodeUtil.NumberType.NotANumber)
                {
                    return i;
                }
            }
            return -1;
        }

        virtual protected bool parse_noequals_num_parameter(string token, ref GCodeParam param)
        {
            int i = is_num_parameter(token);
            if (i >= 0)
                return parse_value_param(token, i, 0, ref param);
            return false;
        }

        virtual protected bool parse_equals_parameter(string token, ref GCodeParam param)
        {
            int i = token.IndexOf('=');
            return parse_value_param(token, i, 1, ref param);
        }

        virtual protected bool parse_value_param(string token, int split, int skip, ref GCodeParam param)
        {
            param.Identifier = token.Substring(0, split);

            string value = token.Substring(split + skip, token.Length - (split + skip));

            try
            {
                GCodeUtil.NumberType numType = GCodeUtil.GetNumberType(value);
                if (numType == GCodeUtil.NumberType.Decimal)
                {
                    param.Type = GCodeParamTypes.DoubleValue;
                    param.DoubleValue = double.Parse(value);
                    return true;
                }
                else if (numType == GCodeUtil.NumberType.Integer)
                {
                    param.Type = GCodeParamTypes.IntegerValue;
                    param.IntegerValue = int.Parse(value);
                    return true;
                }
            }
            catch
            {
                // just continue on and do generic string param
            }

            param.Type = GCodeParamTypes.TextValue;
            param.TextValue = value;
            return true;
        }
    }
}