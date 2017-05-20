using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;

namespace SeedTable {
    public class YamlData {
        public DataDictionaryList Data { get; private set; }
        public bool NeedSubdivide { get; set; }
        public int PreCut { get; set; }
        public int PostCut { get; set; }
        public SeedYamlFormat Format { get; set; }
        public bool DeletePrevious { get; set; }
        public IEnumerable<string> YamlColumnNames { get; set; }

        public YamlData(
            DataDictionaryList data,
            bool needSubdivide = false,
            int preCut = 0,
            int postCut = 0,
            SeedYamlFormat format = SeedYamlFormat.Hash,
            bool deletePrevious = false,
            IEnumerable<string> yamlColumnNames = null
        ) {
            Data = data;
            NeedSubdivide = needSubdivide;
            PreCut = preCut;
            PostCut = postCut;
            Format = format;
            DeletePrevious = deletePrevious;
            YamlColumnNames = yamlColumnNames;
        }

        public void WriteTo(string name, string directory = ".", string extension = ".yml") {
            if (!NeedSubdivide) {
                WriteToSingle(name, directory, extension);
            } else {
                WriteToMulti(name, directory, extension);
            }
        }

        private void WriteToSingle(string name, string directory = ".", string extension = ".yml") {
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            if (extension == null) extension = "";
            File.WriteAllText(Path.Combine(directory, name + extension), YamlData.DataToYaml(Data, Format, YamlColumnNames));
        }

        private void WriteToMulti(string name, string directory = ".", string extension = ".yml") {
            var named_directory = Path.Combine(directory, name);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            if (!Directory.Exists(named_directory)) {
                Directory.CreateDirectory(named_directory);
            } else if (DeletePrevious) {
                foreach (var file in Directory.EnumerateFiles(named_directory, $"*{extension}")) {
                    File.Delete(file);
                }
            }
            if (extension == null) extension = "";
            if (Format == SeedYamlFormat.Hash) {
                foreach (var part in Data.ToSeparatedDictionaryDictionary(PreCut, PostCut)) {
                    File.WriteAllText(Path.Combine(named_directory, part.Key + extension), YamlData.DataToYaml(part.Value, YamlColumnNames));
                }
            } else {
                foreach (var part in Data.ToSeparated(PreCut, PostCut)) {
                    File.WriteAllText(Path.Combine(named_directory, part.Key + extension), YamlData.DataToYaml(part.Value, YamlColumnNames));
                }
            }
        }

        public static YamlData ReadFrom(string name, string directory = ".", string extension = ".yml") {
            var namedDirectory = Path.Combine(directory, name);
            if (Directory.Exists(namedDirectory)) {
                return ReadFromMulti(name, directory, extension);
            } else {
                return ReadFromSingle(name, directory, extension);
            }
        }

        public static YamlData ReadFromSingle(string name, string directory = ".", string extension = ".yml") {
            var fstream = new FileStream(Path.Combine(directory, name + extension), FileMode.Open);
            return new YamlData(YamlData.YamlToData(new StreamReader(fstream)));
        }

        public static YamlData ReadFromMulti(string name, string directory = ".", string extension = ".yml") {
            var namedDirectory = Path.Combine(directory, name);
            return new YamlData(
                YamlData.YamlToData(
                    string.Join("\n", Directory.EnumerateFiles(namedDirectory, $"*{extension}").Select(file => File.ReadAllText(file)).ToArray())
                )
            );
        }

        public static DataDictionaryList YamlToData(TextReader stream) {
            var deserializer = new DeserializerBuilder().Build();
            var root = deserializer.Deserialize(stream);
            var jsonSerializer = new SerializerBuilder().JsonCompatible().Build();
            IEnumerable<Dictionary<object, object>> recordList;
            if (root == null) {
                recordList = new Dictionary<object, object>[] { };
            } else if (root is List<object>) {
                recordList = ((List<object>)root).Select(record => (Dictionary<object, object>)record);
            } else {
                recordList = ((Dictionary<object, object>)root).Select(pair => (Dictionary<object, object>)pair.Value);
            }
            var table = recordList.Select(row => row.ToDictionary(
                    pair => (string)pair.Key,
                    pair =>
                        pair.Value is string || pair.Value is long || pair.Value is double || pair.Value is bool || pair.Value == null ?
                        GetTypedYamlValue(pair.Value) :
                        jsonSerializer.Serialize(pair.Value)
                ));
            return new DataDictionaryList(table);
        }

        public static DataDictionaryList YamlToData(string yaml) {
            return YamlToData(new StringReader(yaml));
        }

        public static void DataToYaml(TextWriter writer, Dictionary<string, Dictionary<string, object>> datatable, IEnumerable<string> yamlColumnNames = null) {
            var builder = new SerializerBuilder();
            builder.EmitDefaults();
            var serializer = builder.Build();
            var tree = yamlColumnNames == null ? datatable : ConvertDataTableWithYamlColumns(datatable, yamlColumnNames);
            serializer.Serialize(writer, tree);
        }

        public static void DataToYaml(TextWriter writer, IEnumerable<Dictionary<string, object>> datatable, IEnumerable<string> yamlColumnNames = null) {
            var builder = new SerializerBuilder();
            builder.EmitDefaults();
            var serializer = builder.Build();
            var tree = yamlColumnNames == null ? datatable : ConvertDataTableWithYamlColumns(datatable, yamlColumnNames);
            serializer.Serialize(writer, tree);
        }

        public static void DataToYaml(TextWriter writer, DataDictionaryList datatable, SeedYamlFormat format = SeedYamlFormat.Hash, IEnumerable<string> yamlColumnNames = null) {
            if (format == SeedYamlFormat.Hash) {
                DataToYaml(writer, datatable.ToDictionaryDictionary(), yamlColumnNames);
            } else {
                DataToYaml(writer, datatable.Table, yamlColumnNames);
            }
        }

        public static string DataToYaml(Dictionary<string, Dictionary<string, object>> datatable, IEnumerable<string> yamlColumnNames = null) {
            var writer = new StringWriter();
            DataToYaml(writer, datatable, yamlColumnNames);
            return writer.ToString();
        }

        public static string DataToYaml(List<Dictionary<string, object>> datatable, IEnumerable<string> yamlColumnNames = null) {
            var writer = new StringWriter();
            DataToYaml(writer, datatable, yamlColumnNames);
            return writer.ToString();
        }

        public static string DataToYaml(DataDictionaryList datatable, SeedYamlFormat format = SeedYamlFormat.Hash, IEnumerable<string> yamlColumnNames = null) {
            var writer = new StringWriter();
            DataToYaml(writer, datatable, format, yamlColumnNames);
            return writer.ToString();
        }

        private static object GetTypedYamlValue(object value) {
            if (value == null) return "";
            if (value is string) {
                if ((string)value == "null") return "";
                long longValue;
                if (long.TryParse((string)value, out longValue)) return longValue;
                double doubleValue;
                if (double.TryParse((string)value, out doubleValue)) return doubleValue;
                return value;
            } else {
                return value;
            }
        }

        private static object ConvertDataTableWithYamlColumns(Dictionary<string, Dictionary<string, object>> datatable, IEnumerable<string> yamlColumnNames) {
            var builder = new DeserializerBuilder();
            var deserializer = builder.Build();
            return datatable.ToDictionary(
                pair => pair.Key,
                pair => ConvertValueWithYamlColumns(deserializer, pair.Value, yamlColumnNames)
            );
        }

        private static object ConvertDataTableWithYamlColumns(IEnumerable<Dictionary<string, object>> datatable, IEnumerable<string> yamlColumnNames) {
            var builder = new DeserializerBuilder();
            var deserializer = builder.Build();
            return datatable.Select(
                value => ConvertValueWithYamlColumns(deserializer, value, yamlColumnNames)
            );
        }

        private static object ConvertValueWithYamlColumns(Deserializer deserializer, Dictionary<string, object> value, IEnumerable<string> yamlColumnNames) {
            return value.ToDictionary(
                pair => pair.Key,
                pair => {
                    if (pair.Value == null) {
                        return null;
                    } else if (yamlColumnNames.Contains(pair.Key)) {
                        return deserializer.Deserialize(new StringReader(pair.Value.ToString()));
                    } else {
                        return pair.Value;
                    }
                }
            );
        }
    }

    public enum SeedYamlFormat {
        Hash,
        Array,
    }
}
