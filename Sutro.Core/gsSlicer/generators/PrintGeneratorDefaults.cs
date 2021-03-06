﻿namespace gs
{
    /// <summary>
    /// Default implementations of "pluggable" ThreeAxisPrintGenerator functions
    /// </summary>
    public static class PrintGeneratorDefaults
    {
        /*
         * Compiler Post-Processors
         */

        public static void AppendPrintStatistics(
            IThreeAxisPrinterCompiler compiler, ThreeAxisPrintGenerator printgen)
        {
            compiler.AppendComment("".PadRight(79, '-'));
            foreach (string line in printgen.TotalPrintTimeStatistics.ToStringList())
            {
                compiler.AppendComment(" " + line);
            }
            compiler.AppendComment("".PadRight(79, '-'));
            foreach (string line in printgen.TotalExtrusionReport)
            {
                compiler.AppendComment(" " + line);
            }
            compiler.AppendComment("".PadRight(79, '-'));
        }
    }
}