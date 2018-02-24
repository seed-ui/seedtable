using System.Collections.Generic;
using CommandLine;

namespace SeedTable {
    public interface ICommonIOOptions {
        IEnumerable<string> files { get; set; }

        string output { get; set; }
    }

    public interface ICommonBasicOptions {
        IEnumerable<string> ignore { get; set; }
        
        IEnumerable<string> only { get; set; }

        IEnumerable<string> mapping { get; set; }

        IEnumerable<string> alias { get; set; }

        string requireVersion { get; set; }

        string versionColumn { get; set; }

        IEnumerable<string> ignoreColumns { get; set; }

        int columnNamesRow { get; set; }

        int dataStartRow { get; set; }

        CommonOptions.Engine engine { get; set; }

        bool delete { get; set; }

        string seedExtension { get; set; }
    }

    public interface IFromIOOptions : ICommonIOOptions {
        string input { get; set; }
    }

    public interface IFromBasicOptions : ICommonBasicOptions {
        // bool stdout { get; set; }

        IEnumerable<string> subdivide { get; set; }

        IEnumerable<string> primary { get; set; }

        IEnumerable<string> yamlColumns { get; set; }

        SeedYamlFormat format { get; set; }
    }

    public interface IFromOptions : IFromBasicOptions, IFromIOOptions { }

    public interface IToIOOptions : ICommonIOOptions {
        string seedInput { get; set; }

        string xlsxInput { get; set; }
    }

    public interface IToBasicOptions : ICommonBasicOptions {
        bool calcFormulas { get; set; }
    }

    public interface IToOptions : IToBasicOptions, IToIOOptions { }

    public interface IBasicOptions : IFromBasicOptions, IToBasicOptions { }

    public class CommonBasicOptions {
        public enum Engine {
            OpenXml,
            ClosedXML,
            EPPlus,
        }

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

        [Option('n', "ignore-columns", Separator = ',', HelpText = "ignore columns")]
        public IEnumerable<string> ignoreColumns { get; set; } = new List<string> { };

        [Option('f', "format", Separator = ',', HelpText = "format")]
        public SeedYamlFormat format { get; set; } = SeedYamlFormat.Hash;

        [Option("column-names-row", Default = 2, HelpText = "column names row index")]
        public int columnNamesRow { get; set; } = 2;

        [Option("data-start-row", Default = 3, HelpText = "data start row index")]
        public int dataStartRow { get; set; } = 3;

        [Option('e', "engine", Default = Engine.EPPlus, HelpText = "parser engine")]
        public virtual Engine engine { get; set; } = Engine.EPPlus;

        [Option('d', "delete", Default = false, HelpText = "delete data which is not exists in source")]
        public bool delete { get; set; } = false;

        [Option("seed-extension", Default = ".yml", HelpText = "seed file extension")]
        public virtual string seedExtension { get; set; } = ".yml";
    }

    public class CommonOptions : CommonBasicOptions {
        [Value(0, Required = true, HelpText = "xlsx files")]
        public IEnumerable<string> files { get; set; }

        [Option('o', "output", Default = ".", HelpText = "output directory")]
        public virtual string output { get; set; } = ".";
    }

    [Verb("from", HelpText ="Yaml from(<-) Excel")]
    public class FromOptions : CommonOptions, IFromOptions {
        [Option('i', "input", Default = ".", HelpText = "input directory")]
        public string input { get; set; } = ".";

        [Option('o', "output", Default = ".", HelpText = "output directory")]
        public override string output { get; set; } = ".";

        // [Option('d', "stdout", Default = false, HelpText = "output one sheets to stdout")]
        // public bool stdout { get; set; }

        [Option('y', "yaml-columns", Separator = ',', HelpText = "yaml columns")]
        public IEnumerable<string> yamlColumns { get; set; } = new List<string> { };

        [Option('P', "primary", Separator = ',', HelpText = "primary file for sheet names that exists in multiple files")]
        public IEnumerable<string> primary { get; set; } = new List<string> { };

        [Option('S', "subdivide", Separator = ',', HelpText = "subdivide rules : [(pre cut):]sheet-name[:(post cut)]")]
        public IEnumerable<string> subdivide { get; set; } = new List<string> { };
    }

    [Verb("to", HelpText = "Yaml to(->) Excel")]
    public class ToOptions : CommonOptions, IToOptions {
        [Option('s', "seed-input", Default = ".", HelpText = "seed input directory")]
        public string seedInput { get; set; } = ".";

        [Option('x', "xlsx-input", Default = ".", HelpText = "xlsx input directory")]
        public string xlsxInput { get; set; } = ".";

        [Option('o', "output", Default = ".", HelpText = "output directory")]
        public override string output { get; set; } = ".";

        [Option('c', "calc-formulas", Default = false, HelpText = "calculate all formulas and store results to cache fields")]
        public bool calcFormulas { get; set; } = false;
    }

    public class BasicOptions : CommonBasicOptions, IBasicOptions {
        public IEnumerable<string> yamlColumns { get; set; } = new List<string> { };

        public IEnumerable<string> primary { get; set; } = new List<string> { };

        public IEnumerable<string> subdivide { get; set; } = new List<string> { };

        public bool calcFormulas { get; set; } = false;
    }
}
