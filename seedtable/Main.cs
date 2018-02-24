using System;
using CommandLine;

namespace SeedTable {
    class MainClass {
        public static void Main(string[] args) {
            SeedTableInterface.InformationMessageEvent += (message) => Console.Error.WriteLine(message);
            var options = CommandLine.Parser.Default.ParseArguments<FromOptions, ToOptions>(args);
            try {
                options.MapResult(
                    (FromOptions opts) => ExcelToSeed(opts),
                    (ToOptions opts) => SeedToExcel(opts),
                    error => true
                    );
            } catch (SeedTableInterface.CannotContinueException) {
                Environment.Exit(1);
            }
        }

        static bool ExcelToSeed(FromOptions opts) {
            if (opts.config != null && opts.config.Length > 0) {
                opts = BasicOptions.Load(opts.config).FromOptions(
                    files: opts.files,
                    input: opts.input,
                    output: opts.output
                );
            }
            return SeedTableInterface.ExcelToSeed(opts);
        }

        static bool SeedToExcel(ToOptions opts) {
            if (opts.config != null && opts.config.Length > 0) {
                opts = BasicOptions.Load(opts.config).ToOptions(
                    files: opts.files,
                    seedInput: opts.seedInput,
                    xlsxInput: opts.xlsxInput,
                    output: opts.output
                );
            }
            return SeedTableInterface.SeedToExcel(opts);
        }
    }
}
