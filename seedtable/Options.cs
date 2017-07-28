using System.Collections.Generic;
using CommandLine;

namespace SeedTable {
    public class CommonOptions {
        public enum Engine {
            OpenXml,
            ClosedXML,
            EPPlus,
        }

        [Value(0, Required = true, HelpText = "xlsx files")]
        public IEnumerable<string> files { get; set; }

        [Option('o', "output", Default = ".", HelpText = "output directory")]
        public string output { get; set; } = ".";

        [Option('S', "subdivide", Separator = ',', HelpText = "subdivide rules : [(pre cut):]sheet-name[:(post cut)]")]
        public IEnumerable<string> subdivide { get; set; } = new List<string> { };

        [Option('I', "ignore", Separator = ',', HelpText = "ignore sheet names")]
        public IEnumerable<string> ignore { get; set; } = new List<string> { };
        
        [Option('O', "only", Separator = ',', HelpText = "only sheet names")]
        public IEnumerable<string> only { get; set; } = new List<string> { };
        
        [Option('M', "mapping", Separator = ',', HelpText = "sheet names mapping : (seed table name):(excel sheet name)")]
        public IEnumerable<string> mapping { get; set; } = new List<string> { };

        [Option('A', "alias", Separator = ',', HelpText = "sheet names alias : (seed table name):(excel sheet name)")]
        public IEnumerable<string> alias { get; set; } = new List<string> { };

        [Option('R', "require-version", Default = "", HelpText = "require version (with version column)")]
        public string requireVersion { get; set; } = "";

        [Option('v', "version-column", HelpText = "version column")]
        public string versionColumn { get; set; }

        [Option('y', "yaml-columns", Separator = ',', HelpText = "yaml columns")]
        public IEnumerable<string> yamlColumns { get; set; } = new List<string> { };

        [Option('n', "ignore-columns", Separator = ',', HelpText = "ignore columns")]
        public IEnumerable<string> ignoreColumns { get; set; } = new List<string> { };

        [Option('f', "format", Separator = ',', HelpText = "format")]
        public SeedYamlFormat format { get; set; } = SeedYamlFormat.Hash;

        [Option("column-names-row", Default = 2, HelpText = "column names row index")]
        public int columnNamesRow { get; set; } = 2;

        [Option("data-start-row", Default = 3, HelpText = "data start row index")]
        public int dataStartRow { get; set; } = 3;

        [Option('e', "engine", Default = Engine.EPPlus, HelpText = "parser engine")]
        public virtual Engine engine { get; set; }

        [Option('d', "delete", Default = false, HelpText = "delete data which is not exists in source")]
        public bool delete { get; set; } = false;

        [Option("seed-extension", Default = ".yml", HelpText = "seed file extension")]
        public virtual string seedExtension { get; set; } = ".yml";
    }

    [Verb("from", HelpText ="Yaml from Excel")]
    public class FromOptions : CommonOptions {
        [Option('e', "engine", Default = Engine.EPPlus, HelpText = "parser engine")]
        public override Engine engine { get; set; } = Engine.EPPlus;

        // [Option('d', "stdout", Default = false, HelpText = "output one sheets to stdout")]
        // public bool stdout { get; set; }

        [Option('i', "input", Default = ".", HelpText = "input directory")]
        public string input { get; set; } = ".";
    }

    [Verb("to", HelpText = "Yaml to Excel")]
    public class ToOptions : CommonOptions {
        [Option('e', "engine", Default = Engine.EPPlus, HelpText = "parser engine")]
        public override Engine engine { get; set; } = Engine.EPPlus;

        [Option('s', "seed-input", Default = ".", HelpText = "seed input directory")]
        public string seedInput { get; set; } = ".";

        [Option('x', "xlsx-input", Default = ".", HelpText = "xlsx input directory")]
        public string xlsxInput { get; set; } = ".";

        [Option('c', "calc-formulas", Default = false, HelpText = "calculate all formulas and store results to cache fields")]
        public bool calcFormulas { get; set; } = false;
    }
}
