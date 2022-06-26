using System.Windows.Forms;

namespace Reports.Interfaces
{
    public interface IFileWriter
    {
        void Save(DataGridView gridview, TreeView tree, string info, string numberOfEvents);
        void Save2(DataGridView gridview, string info, string numberOfEvents);
    }
}
