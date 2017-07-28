using System;
using CommandLine;

namespace SeedTable {
    class MainClass {
        public static void Main(string[] args) {
            SeedTableInterface.InformationMessageEvent += (message) => Console.Error.WriteLine(message);
            var options = CommandLine.Parser.Default.ParseArguments<FromOptions, ToOptions>(args);
            try {
                options.MapResult(
                    (FromOptions opts) => SeedTableInterface.ExcelToSeed(opts),
                    (ToOptions opts) => SeedTableInterface.SeedToExcel(opts),
                    error => true
                    );
            } catch (SeedTableInterface.CannotContinueException) {
                Environment.Exit(1);
            }
        }
    }
}
