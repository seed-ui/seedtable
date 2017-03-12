using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;

namespace SeedTable {
    class YamlData {
        public DataDictionaryList data { get; private set; }

        public YamlData(DataDictionaryList data) {
            this.data = data;
        }

        public void WriteTo(string name, string directory = ".", bool needSubdivide = false, int pre_cut = 0, int post_cut = 0, string extension = ".yml", IEnumerable<string> yamlColumnNames = null, bool deletePrevious = false) {
            if (!needSubdivide) {
                WriteToSingle(name, directory, extension, yamlColumnNames);
            } else {
                WriteToMulti(name, directory, pre_cut, post_cut, extension, yamlColumnNames, deletePrevious);
            }
        }

        public void WriteToSingle(string name, string directory = ".", string extension = ".yml", IEnumerable<string> yamlColumnNames = null) {
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            if (extension == null) extension = "";
            File.WriteAllText(Path.Combine(directory, name + extension), YamlData.DataToYaml(data, yamlColumnNames));
        }

        public void WriteToMulti(string name, string directory = ".", int pre_cut = 0, int post_cut = 0, string extension = ".yml", IEnumerable<string> yamlColumnNames = null, bool deletePrevious = false) {
            var named_directory = Path.Combine(directory, name);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            if (!Directory.Exists(named_directory)) {
                Directory.CreateDirectory(named_directory);
            } else if (deletePrevious) {
                foreach (var file in Directory.EnumerateFiles(named_directory, $"*{extension}")) {
                    File.Delete(file);
                }
            }
            if (extension == null) extension = "";
            foreach (var part in data.ToSeparatedDictionaryDictionary(pre_cut, post_cut)) {
                File.WriteAllText(Path.Combine(named_directory, part.Key + extension), YamlData.DataToYaml(part.Value, yamlColumnNames));
            }
        }

        public static YamlData ReadFrom(string name, string directory = ".", string extension = ".yml") {
            var named_directory = Path.Combine(directory, name);
            if (Directory.Exists(named_directory)) {
                return ReadFromMulti(name, directory, extension);
            } else {
                return ReadFromSingle(name, directory, extension);
            }
        }

        public static YamlData ReadFromSingle(string name, string directory = ".", string extension = ".yml") {
            return new YamlData(YamlData.YamlToData(File.ReadAllText(Path.Combine(directory, name + extension))));
        }

        public static YamlData ReadFromMulti(string name, string directory = ".", string extension = ".yml") {
            var named_directory = Path.Combine(directory, name);
            return new YamlData(
                YamlData.YamlToData(
                    string.Join("\n", Directory.EnumerateFiles(named_directory, $"*{extension}").Select(file => File.ReadAllText(file)).ToArray())
                    )
                );
        }

        public static DataDictionaryList YamlToData(TextReader stream) {
            var yaml_stream = new YamlStream();
            yaml_stream.Load(stream);
            var root = (YamlMappingNode)yaml_stream.Documents[0].RootNode;
            var jsonSerializer = new SerializerBuilder().JsonCompatible().Build();
            var table = root.Children
                .Select(child => (YamlMappingNode)child.Value)
                .Select(row => row.ToDictionary(
                    pair => ((YamlScalarNode)pair.Key).Value,
                    pair => pair.Value is YamlScalarNode ? GetTypedYamlValue(((YamlScalarNode)pair.Value).Value) : jsonSerializer.Serialize(pair.Value)
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

        public static void DataToYaml(TextWriter writer, DataDictionaryList datatable, IEnumerable<string> yamlColumnNames = null) {
            DataToYaml(writer, datatable.ToDictionaryDictionary(), yamlColumnNames);
        }

        public static string DataToYaml(Dictionary<string, Dictionary<string, object>> datatable, IEnumerable<string> yamlColumnNames = null) {
            var writer = new StringWriter();
            DataToYaml(writer, datatable, yamlColumnNames);
            return writer.ToString();
        }

        public static string DataToYaml(DataDictionaryList datatable, IEnumerable<string> yamlColumnNames = null) {
            var writer = new StringWriter();
            DataToYaml(writer, datatable, yamlColumnNames);
            return writer.ToString();
        }

        private static object GetTypedYamlValue(string value) {
            if (value == null || value == "null") return "";
            long longValue;
            if (long.TryParse(value, out longValue)) return longValue;
            double doubleValue;
            if (double.TryParse(value, out doubleValue)) return doubleValue;
            return value;
        }

        private static object ConvertDataTableWithYamlColumns(Dictionary<string, Dictionary<string, object>> datatable, IEnumerable<string> yamlColumnNames = null) {
            var builder = new DeserializerBuilder();
            var deserializer = builder.Build();
            return datatable.ToDictionary(
                pair => pair.Key,
                pair => pair.Value.ToDictionary(
                    _pair => _pair.Key,
                    _pair => {
                        if (_pair.Value == null) {
                            return null;
                        } else if (yamlColumnNames.Contains(_pair.Key)) {
                            return deserializer.Deserialize(new StringReader(_pair.Value.ToString()));
                        } else {
                            return _pair.Value;
                        }
                    }
                )
            );
        }
    }
}
