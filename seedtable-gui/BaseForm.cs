using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace seedtable_gui {
    public abstract class BaseForm : Form {
        public BaseForm() {
            var font = Environment.GetEnvironmentVariable("GUIFONT");
            if (font != null) Font = new Font(font, Font.Size);
        }
    }
}
