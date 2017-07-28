using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SeedTable;

namespace seedtable_gui {
    public partial class SettingDialog : Form {
        public SettingDialog(ToOptions options, bool changeable = true) {
            InitializeComponent();
            foreach (Control controll in Controls) {
                controll.Enabled = changeable;
            }

            var toOptionsStringListDataSource = new ToOptionsStringListDataSource(options);
            subdivideTextBox.DataBindings.Add("Text", toOptionsStringListDataSource, "subdivide", true, DataSourceUpdateMode.OnPropertyChanged);
            onlyTextBox.DataBindings.Add("Text", toOptionsStringListDataSource, "only", true, DataSourceUpdateMode.OnPropertyChanged);
            ignoreTextBox.DataBindings.Add("Text", toOptionsStringListDataSource, "ignore", true, DataSourceUpdateMode.OnPropertyChanged);
            primaryTextBox.DataBindings.Add("Text", toOptionsStringListDataSource, "primary", true, DataSourceUpdateMode.OnPropertyChanged);
            mappingTextBox.DataBindings.Add("Text", toOptionsStringListDataSource, "mapping", true, DataSourceUpdateMode.OnPropertyChanged);
            aliasTextBox.DataBindings.Add("Text", toOptionsStringListDataSource, "alias", true, DataSourceUpdateMode.OnPropertyChanged);
            ignoreColumnsTextBox.DataBindings.Add("Text", toOptionsStringListDataSource, "ignoreColumns", true, DataSourceUpdateMode.OnPropertyChanged);
            yamlColumnsTextBox.DataBindings.Add("Text", toOptionsStringListDataSource, "yamlColumns", true, DataSourceUpdateMode.OnPropertyChanged);

            engineComboBox.DataSource = Enum.GetValues(typeof(ToOptions.Engine));
            formatComboBox.DataSource = Enum.GetValues(typeof(SeedYamlFormat));
            toOptionsBindingSource.DataSource = options;
        }
    }

    class ToOptionsStringListDataSource {
        ToOptions Options;

        public ToOptionsStringListDataSource(ToOptions options) {
            Options = options;
        }

        [TypeConverter(typeof(StringListToMultiLineStringConverter))]
        public IEnumerable<string> subdivide {
            get { return Options.subdivide; }
            set { Options.subdivide = value; }
        }

        [TypeConverter(typeof(StringListToMultiLineStringConverter))]
        public IEnumerable<string> only {
            get { return Options.only; }
            set { Options.only = value; }
        }

        [TypeConverter(typeof(StringListToMultiLineStringConverter))]
        public IEnumerable<string> ignore {
            get { return Options.ignore; }
            set { Options.ignore = value; }
        }

        [TypeConverter(typeof(StringListToMultiLineStringConverter))]
        public IEnumerable<string> primary {
            get { return Options.primary; }
            set { Options.primary = value; }
        }

        [TypeConverter(typeof(StringListToMultiLineStringConverter))]
        public IEnumerable<string> mapping {
            get { return Options.mapping; }
            set { Options.mapping = value; }
        }

        [TypeConverter(typeof(StringListToMultiLineStringConverter))]
        public IEnumerable<string> alias {
            get { return Options.alias; }
            set { Options.alias = value; }
        }

        [TypeConverter(typeof(StringListToMultiLineStringConverter))]
        public IEnumerable<string> ignoreColumns {
            get { return Options.ignoreColumns; }
            set { Options.ignoreColumns = value; }
        }

        [TypeConverter(typeof(StringListToMultiLineStringConverter))]
        public IEnumerable<string> yamlColumns {
            get { return Options.yamlColumns; }
            set { Options.yamlColumns = value; }
        }
    }

    class StringListToMultiLineStringConverter : TypeConverter {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
            return ((string)value).Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
            return destinationType == typeof(string);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
            return string.Join(Environment.NewLine, (IEnumerable<string>)value);
        }
    }
}
