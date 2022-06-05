using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reports.Interfaces
{
    public interface IFileWriter
    {
        void Save(DataGridView gridview, TreeView tree, string info, string numberOfEvents);
    }
}
