using Reports.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reports.File
{
    public class Excel : IFileWriter
    {
        public void Save(DataGridView gridview, TreeView tree, string filter, string info)
        {
            //gridview.Columns[0].Selected = true;
            gridview.SelectAll();
            DataObject dataObj = gridview.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
            else return;

            Microsoft.Office.Interop.Excel.Application xlexcel;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            try
            {
                xlexcel = new Microsoft.Office.Interop.Excel.Application
                {
                    Visible = true
                };
                xlWorkBook = xlexcel.Workbooks.Add(misValue);
                xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                xlWorkSheet.Cells[1, 1] = filter;
                xlWorkSheet.Cells[3, 1] = info;
                
                int index = 0;
                for (int i = 0; i < gridview.ColumnCount; i++)
                {
                    index = i + 1;
                    xlWorkSheet.Cells[5, index] = gridview.Columns[i].HeaderText;                    
                }

                Microsoft.Office.Interop.Excel.Range CR = (Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[6, 1];
                CR.Select();
                xlWorkSheet.PasteSpecial(CR, System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing, true);
                xlWorkSheet.ClearArrows();
            }
            catch (System.Exception msg)
            {
                MessageBox.Show(msg.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
