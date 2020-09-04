using System;
using System.Windows.Forms;

namespace Reports
{
    public partial class Form1 : BaseForms.BaseForm
    {
        public Form1()
        {
            InitializeComponent();            
            comboBox1.SelectedIndex = 0;
            myDatabase = new MyDatabase();
            //thread1 = new System.Threading.Thread(new System.Threading.ThreadStart(fillTree));
            //thread1.Start();
            fillTree();            
            dateTimePicker1.Text = DateTime.Now.ToString("yyyy-MM-dd");
            dateTimePicker2.Text = DateTime.Now.ToString("yyyy-MM-dd");
            programm_type = Convert.ToInt32((System.Configuration.ConfigurationManager.AppSettings["program_type"]));
        }

        private MyDatabase myDatabase;
        private readonly int programm_type;
        private System.Collections.Generic.List<EmployeeListbox> employeeListboxes;
        //private System.Threading.Thread thread1;

        private void pictureBox1_MouseEnter(object sender, System.EventArgs e)
        {
            pictureBox1.Image = Properties.Resources.excel_light;
        }

        private void pictureBox1_MouseLeave(object sender, System.EventArgs e)
        {
            pictureBox1.Image = Properties.Resources.excel;
        }

        private void btn_close_MouseEnter(object sender, System.EventArgs e)
        {
            btn_close.BackColor = System.Drawing.Color.Red;
        }

        private void btn_close_MouseLeave(object sender, System.EventArgs e)
        {
            btn_close.BackColor = System.Drawing.Color.FromArgb(47, 47, 47);
        }

        private void pictureBox2_MouseEnter(object sender, System.EventArgs e)
        {
            pictureBox2.Image = Properties.Resources.user_holiday;
        }

        private void pictureBox2_MouseLeave(object sender, System.EventArgs e)
        {
            pictureBox2.Image = Properties.Resources.user_holiday_light;
        }

        private void pictureBox4_MouseEnter(object sender, System.EventArgs e)
        {
            pictureBox4.Image = Properties.Resources.calendar;
        }

        private void pictureBox4_MouseLeave(object sender, System.EventArgs e)
        {
            pictureBox4.Image = Properties.Resources.calendar_light;
        }

        private void fillTree()
        {
            System.Collections.Generic.List<MyTree> myTrees = myDatabase.getTree("select ttext, mytree from department order by id asc");
            for (int i = 0; i < myTrees.Count; i++)
            {
                TreeNode tnode = new TreeNode
                {
                    Name = myTrees[i].Tname,
                    Text = myTrees[i].Ttext
                };
                FindByText(tnode, treeView1);
            }
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            MaximizedBounds = Screen.FromHandle(Handle).WorkingArea;
            WindowState = FormWindowState.Maximized;
        }

        private void btn_close_Click(object sender, System.EventArgs e)
        {
            Close();
        }       

        private void treeView1_AfterSelect_1(object sender, TreeViewEventArgs e)
        {
            employeeListboxes = myDatabase.GetEmployeeListbox(
                "select employeeid, familiya from employee where department <@ '" + treeView1.SelectedNode.Name
                + "' order by familiya");

            comboBox2.Items.Clear();
            foreach(EmployeeListbox employeeListbox in employeeListboxes)
            {
                comboBox2.Items.Add(employeeListbox.Familiya);
            }

            ClearBackColor(treeView1);            
            treeView1.SelectedNode.BackColor = System.Drawing.Color.FromArgb(0, 61, 100);
            treeView1.SelectedNode.ForeColor = System.Drawing.Color.White;
        }        

        private void copyAlltoClipboard()
        {            
            dataGridView1.SelectAll();
            DataObject dataObj = dataGridView1.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);            
        }

        private void pictureBox1_Click(object sender, System.EventArgs e)
        {
            if (dataGridView1.Rows.Count < 1)
                return;

            copyAlltoClipboard();
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
                xlWorkSheet.Cells[1, 1] = "Poytaxt Bank: Отчёт от " + dateTimePicker1.Text + ", до " +
                    dateTimePicker2.Text + ", фильтр по " + comboBox1.Text;
                xlWorkSheet.Cells[3, 1] = label5.Text;
                int index = 0;
               for(int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    index = i + 1;
                    xlWorkSheet.Cells[5, index] = dataGridView1.Columns[i].HeaderText;
                }
                Microsoft.Office.Interop.Excel.Range CR = (Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[6, 1];
                CR.Select();
                xlWorkSheet.PasteSpecial(CR, System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing, true);
                xlWorkSheet.ClearArrows();
            }
            catch (System.Exception msg)
            {
                MessageBox.Show(msg.ToString());
            }
            
            //xlWorkBook.Close();
        }

        //bystaff
        private void button2_Click(object sender, System.EventArgs e)
        {
            if (treeView1 == null || string.IsNullOrEmpty(comboBox2.Text))
                return;

            label5.Text = "";

            try
            {
                switch (programm_type)
                {
                    case 1:
                        var single = new CustomClasses.SingleShift(dataGridView1, label5);
                        single.ReportByPerson(comboBox1.SelectedIndex, employeeListboxes[comboBox2.SelectedIndex].ID,
                        dateTimePicker1.Text, dateTimePicker2.Text);
                        break;
                    case 2:
                        var single_temp = new CustomClasses.SingleShiftTemp(dataGridView1, label5);
                        single_temp.ReportByPerson(comboBox1.SelectedIndex, employeeListboxes[comboBox2.SelectedIndex].ID,
                        dateTimePicker1.Text, dateTimePicker2.Text);
                        break;
                    case 3:
                        var multiple = new CustomClasses.MultipleShift(dataGridView1, label5);
                        multiple.ReportByPerson(comboBox1.SelectedIndex, employeeListboxes[comboBox2.SelectedIndex].ID,
                        dateTimePicker1.Text, dateTimePicker2.Text);
                        break;
                    case 4:
                        var multiple_temp = new CustomClasses.MultipleShiftTemp(dataGridView1, label5);
                        multiple_temp.ReportByPerson(comboBox1.SelectedIndex, employeeListboxes[comboBox2.SelectedIndex].ID,
                        dateTimePicker1.Text, dateTimePicker2.Text);
                        break;
                    default:
                        var single_default = new CustomClasses.SingleShift(dataGridView1, label5);
                        single_default.ReportByPerson(comboBox1.SelectedIndex, employeeListboxes[comboBox2.SelectedIndex].ID,
                        dateTimePicker1.Text, dateTimePicker2.Text);
                        break;
                }
            }
            catch (Exception msg)
            {
                MessageBox.Show(msg.ToString());
            }
            
        }

        //bydepartment
        private void button1_Click(object sender, System.EventArgs e)
        {
            if (treeView1.Nodes.Count == 0 || string.IsNullOrEmpty(comboBox1.Text))
                return;

            if (treeView1.SelectedNode == treeView1.Nodes[0])
                label5.Text = treeView1.SelectedNode.Text + " | ";
            else
                label5.Text = "Отдел: " + treeView1.SelectedNode.Text + " | ";

            try
            {              
                switch (programm_type)
                {
                    case 1:
                        var single = new CustomClasses.SingleShift(dataGridView1, label5);
                        single.ReportByOtdel(comboBox1.SelectedIndex, treeView1.SelectedNode.Name, dateTimePicker1.Text,
                            dateTimePicker2.Text);
                        break;
                    case 2:
                        var single_temp = new CustomClasses.SingleShiftTemp(dataGridView1, label5);
                        single_temp.ReportByOtdel(comboBox1.SelectedIndex, treeView1.SelectedNode.Name, dateTimePicker1.Text,
                            dateTimePicker2.Text);
                        break;
                    case 3:
                        var multiple = new CustomClasses.MultipleShift(dataGridView1, label5);
                        multiple.ReportByOtdel(comboBox1.SelectedIndex, treeView1.SelectedNode.Name, dateTimePicker1.Text,
                            dateTimePicker2.Text);
                        break;
                    case 4:
                        var multiple_temp = new CustomClasses.MultipleShiftTemp(dataGridView1, label5);
                        multiple_temp.ReportByOtdel(comboBox1.SelectedIndex, treeView1.SelectedNode.Name, dateTimePicker1.Text,
                            dateTimePicker2.Text);
                        break;
                    default:
                        var single_default = new CustomClasses.SingleShift(dataGridView1, label5);
                        single_default.ReportByOtdel(comboBox1.SelectedIndex, treeView1.SelectedNode.Name, dateTimePicker1.Text,
                            dateTimePicker2.Text);
                        break;
                }
            }
            catch (Exception msg)
            {
                MessageBox.Show(msg.ToString());
            }
            
        }

        private void pictureBox2_Click(object sender, System.EventArgs e)
        {
            new UserSettings().ShowDialog();
        }

        private void dataGridView1_SelectionChanged(object sender, System.EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
                return;                    
            dataGridView1.Rows[dataGridView1.CurrentRow.Index].Selected = true;
        }

        private void pictureBox4_Click(object sender, System.EventArgs e)
        {
            new Holiday().ShowDialog();
            //new BaseForms.HolidayBase(3, 0).ShowDialog();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
