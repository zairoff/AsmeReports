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

        public void Save2(DataGridView gridview, string filter, string info)
        {
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
                    index = i + 2;
                    xlWorkSheet.Cells[5, index] = gridview.Columns[i].HeaderText;
                }

                string otdel = "";
                for(int i = 0; i < gridview.RowCount; i++)
                {
                   if(!otdel.Equals(gridview[3, i].Value.ToString()))
                    {
                        xlWorkSheet.Cells[(i + 6), 1] = $"{gridview[3, i].Value.ToString()} : {RowCount(gridview, gridview[3, i].Value.ToString())}";
                        otdel = gridview[3, i].Value.ToString();
                    }
                    for(int j = 0; j < gridview.ColumnCount; j++)
                    {
                        xlWorkSheet.Cells[(i + 6), (j + 2)] = gridview[j, i].Value;
                    }
                }

                xlWorkBook.Close();
            }
            catch (Exception msg)
            {
                MessageBox.Show(msg.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int RowCount(DataGridView dataGridView, string value)
        {
            int count = 0;
            foreach(DataGridViewRow row in dataGridView.Rows)
            {
                if (row.Cells[3].Value.Equals(value))
                {
                    count++;
                }
            }
            return count;
        }
    }
}
