using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SeedTable {
    public class BasicOptions : CommonBasicOptions, IBasicOptions {
        public IEnumerable<string> yamlColumns { get; set; } = new List<string> { };

        public IEnumerable<string> primary { get; set; } = new List<string> { };

        public bool calcFormulas { get; set; } = false;

        public FromOptions FromOptions(IEnumerable<string> files = null, string input = ".", string output = ".") {
            return new FromOptions() {
                ignore = ignore,
                only = only,
                mapping = mapping,
                alias = alias,
                requireVersion = requireVersion,
                versionColumn = versionColumn,
                ignoreColumns = ignoreColumns,
                format = format,
                columnNamesRow = columnNamesRow,
                dataStartRow = dataStartRow,
                engine = engine,
                delete = delete,
                seedExtension = seedExtension,
                yamlColumns = yamlColumns,
                primary = primary,
                subdivide = subdivide,
                input = input,
                output = output,
                files = files,
            };
        }

        public ToOptions ToOptions(IEnumerable<string> files = null, string seedInput = ".", string xlsxInput = ".", string output = ".") {
            return new ToOptions() {
                ignore = ignore,
                only = only,
                mapping = mapping,
                alias = alias,
                requireVersion = requireVersion,
                versionColumn = versionColumn,
                ignoreColumns = ignoreColumns,
                format = format,
                columnNamesRow = columnNamesRow,
                dataStartRow = dataStartRow,
                engine = engine,
                delete = delete,
                seedExtension = seedExtension,
                calcFormulas = calcFormulas,
                subdivide = subdivide,
                seedInput = seedInput,
                xlsxInput = xlsxInput,
                output = output,
                files = files,
            };
        }

        public static BasicOptions Load(string filepath) {
            var yaml = File.ReadAllText(filepath);
            var builder = new DeserializerBuilder();
            builder.WithNamingConvention(new HyphenatedNamingConvention());
            builder.IgnoreUnmatchedProperties();
            var deserializer = builder.Build();
            var options = deserializer.Deserialize<BasicOptions>(yaml);
            return options ?? new BasicOptions();
        }

        public void Save(string filepath) {
            var builder = new SerializerBuilder();
            builder.WithNamingConvention(new HyphenatedNamingConvention());
            var serializer = builder.Build();
            var yaml = serializer.Serialize(this);
            File.WriteAllText(filepath, yaml);
        }
    }
}
