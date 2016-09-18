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

        public void WriteTo(string name, string directory = ".", int pre_cut = 0, int post_cut = 0, string extension = ".yml") {
            if (pre_cut + post_cut == 0) {
                WriteToSingle(name, directory, extension);
            } else {
                WriteToMulti(name, directory, pre_cut, post_cut, extension);
            }
        }

        public void WriteToSingle(string name, string directory = ".", string extension = ".yml") {
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            File.WriteAllText(Path.Combine(directory, name + extension), YamlData.DataToYaml(data));
        }

        public void WriteToMulti(string name, string directory = ".", int pre_cut = 0, int post_cut = 0, string extension = ".yml") {
            var named_directory = Path.Combine(directory, name);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            if (!Directory.Exists(named_directory)) Directory.CreateDirectory(named_directory);
            foreach (var part in data.ToSeparatedDictionaryDictionary(pre_cut, post_cut)) {
                File.WriteAllText(Path.Combine(named_directory, part.Key + extension), YamlData.DataToYaml(part.Value));
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
                    string.Join("\n", Directory.EnumerateFiles(named_directory).Select(file => File.ReadAllText(Path.Combine(named_directory, file))).ToArray())
                    )
                );
        }

        public static DataDictionaryList YamlToData(TextReader stream) {
            var yaml_stream = new YamlStream();
            yaml_stream.Load(stream);
            var root = (YamlMappingNode)yaml_stream.Documents[0].RootNode;
            var table = root.Children.Select(child => (YamlMappingNode)child.Value).Select(row => row.ToDictionary(pair => ((YamlScalarNode)pair.Key).Value, pair => (object)((YamlScalarNode)pair.Value).Value));
            return new DataDictionaryList(table);
        }

        public static DataDictionaryList YamlToData(string yaml) {
            return YamlToData(new StringReader(yaml));
        }

        public static void DataToYaml(TextWriter writer, Dictionary<string, Dictionary<string, object>> datatable) {
            var serializer = new Serializer(SerializationOptions.EmitDefaults);
            serializer.Serialize(writer, datatable);
        }

        public static void DataToYaml(TextWriter writer, DataDictionaryList datatable) {
            DataToYaml(writer, datatable.ToDictionaryDictionary());
        }

        public static string DataToYaml(Dictionary<string, Dictionary<string, object>> datatable) {
            var writer = new StringWriter();
            DataToYaml(writer, datatable);
            return writer.ToString();
        }

        public static string DataToYaml(DataDictionaryList datatable) {
            var writer = new StringWriter();
            DataToYaml(writer, datatable);
            return writer.ToString();
        }
    }
}
