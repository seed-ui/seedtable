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
            if (Environment.OSVersion.Platform == PlatformID.MacOSX) Font = new Font("Hiragino Kaku Gothic Pro", Font.Size);
        }
    }
}
