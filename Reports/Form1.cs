using Reports.File;
using Reports.Interfaces;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Text;

namespace Reports
{
    public partial class Form1 : BaseForms.BaseForm
    {
        public Form1()
        {
            InitializeComponent();
            _department = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(System.Configuration.ConfigurationManager.AppSettings["department"]));
            comboBox1.SelectedIndex = 0;
            _dataBase = new DataBase();
            FillTree();            
            fileWriter = new Excel();
            dateTimePicker1.Text = DateTime.Now.ToString("yyyy-MM-dd");
            dateTimePicker2.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

        private DataBase _dataBase;
        private System.Collections.Generic.List<EmployeeListbox> employeeListboxes;
        private bool max_check;
        private System.Drawing.Point lastLocation;
        private bool mouseDown = false;
        private readonly IFileWriter fileWriter;
        private readonly string _department;

        private void MaximizeCheck()
        {
            if (max_check)
            {
                WindowState = FormWindowState.Normal;
                max_check = false;
            }
            else
            {
                MaximizedBounds = Screen.FromHandle(Handle).WorkingArea;
                WindowState = FormWindowState.Maximized;
                max_check = true;
            }
        }

        private void FillTree()
        {
            System.Collections.Generic.List<MyTree> myTrees = _dataBase.GetTree("select ttext, mytree from department where mytree <@ '" + _department + "' order by id asc");
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
            //MaximizedBounds = Screen.FromHandle(Handle).WorkingArea;
            //WindowState = FormWindowState.Maximized;
        }   

        private void TreeView1_AfterSelect_1(object sender, TreeViewEventArgs e)
        {
            employeeListboxes = _dataBase.GetEmployeeListbox(
                "select employeeid, familiya, ism from employee where department <@ '" + treeView1.SelectedNode.Name
                + "' and status = true order by familiya");

            comboBox2.Items.Clear();
            foreach(EmployeeListbox employeeListbox in employeeListboxes)
            {
                comboBox2.Items.Add(employeeListbox.Familiya + " " + employeeListbox.Ism);
            }

            ClearBackColor(treeView1);            
            treeView1.SelectedNode.BackColor = System.Drawing.Color.FromArgb(0, 61, 100);
            treeView1.SelectedNode.ForeColor = System.Drawing.Color.White;
        }        

        //bystaff
        private async void Button2_Click(object sender, System.EventArgs e)
        {
            if (treeView1 == null || string.IsNullOrEmpty(comboBox2.Text) || treeView1.SelectedNode == null)
                return;

            label5.Text = Properties.Resources.LOADING + " ";
            StartProgress();
            button1.Enabled = false;
            button2.Enabled = false;
            try
            {
                dataGridView1.DataSource = null;
                dataGridView1.Columns.Clear();
                dataGridView1.Rows.Clear();

                switch (comboBox1.SelectedIndex)
                {
                    case 0:
                        dataGridView1.DataSource = await _dataBase.GetRecords("select t2.employeeid, t2.familiya, t2.ism, t2.otchestvo, t2.otdel, t2.lavozim, t1.door_name, t1.kirish, t1.chiqish " +
                                            "from reports t1 " +
                                            "inner join employee t2 " +
                                            "on t1.employeeid = t2.employeeid " +
                                            "where t1.kirish::date >= '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") +
                                            "' and t1.kirish::date <= '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") +
                                            "' and t2.employeeid = " + employeeListboxes[comboBox2.SelectedIndex].ID
                                            );

                        //dataGridView1.DataSource = await _dataBase.GetRecords("select t2.employeeid, t2.familiya, t2.ism, t2.otchestvo, t2.otdel, t2.lavozim, t1.kirish, t1.chiqish " +
                        //                    "from reports t1 " +
                        //                    "inner join employee t2 " +
                        //                    "on t1.employeeid = t2.employeeid " +
                        //                    "where t1.kirish::date >= '" + dateTimePicker1.Text +
                        //                    "' and t1.kirish::date <= '" + dateTimePicker2.Text +
                        //                    "' and t2.employeeid = " + employeeListboxes[comboBox2.SelectedIndex].ID
                        //                    );                        
                        break;
                    case 1:
                        dataGridView1.DataSource = await _dataBase.GetRecords("select *from getallevents_by_person(" +
                                            employeeListboxes[comboBox2.SelectedIndex].ID + ",'" +
                                            dateTimePicker1.Value.ToString("yyyy-MM-dd") + "','" +
                                            dateTimePicker2.Value.ToString("yyyy-MM-dd") + "')"
                                            );
                        break;
                    case 2:
                        dataGridView1.DataSource = await _dataBase.GetRecords("select *from getallevents_by_person_total(" +
                                            employeeListboxes[comboBox2.SelectedIndex].ID + ",'" +
                                            dateTimePicker1.Value.ToString("yyyy-MM-dd") + "','" +
                                            dateTimePicker2.Value.ToString("yyyy-MM-dd") + "')"
                                            );
                        break;
                    case 3:
                        dataGridView1.DataSource = await _dataBase.GetRecords("select *from getlate_by_person(" + 
                                            employeeListboxes[comboBox2.SelectedIndex].ID + ",'" + 
                                            dateTimePicker1.Value.ToString("yyyy-MM-dd") + "','" + 
                                            dateTimePicker2.Value.ToString("yyyy-MM-dd") + "')"
                                            );
                        break;
                    case 4:
                        dataGridView1.DataSource = await _dataBase.GetRecords("select *from getlate_by_person_total(" +
                                            employeeListboxes[comboBox2.SelectedIndex].ID + ",'" +
                                            dateTimePicker1.Value.ToString("yyyy-MM-dd") + "','" +
                                            dateTimePicker2.Value.ToString("yyyy-MM-dd") + "')"
                                            );
                        break;
                    case 5:
                        dataGridView1.DataSource = await _dataBase.GetRecords("select *from getearly_by_person(" + 
                                            employeeListboxes[comboBox2.SelectedIndex].ID + ",'" +
                                            dateTimePicker1.Value.ToString("yyyy-MM-dd") + "','" + 
                                            dateTimePicker2.Value.ToString("yyyy-MM-dd") + "')"
                                            );
                        break;
                    case 6:
                        dataGridView1.DataSource = await _dataBase.GetRecords("select *from getearly_by_person_total(" +
                                            employeeListboxes[comboBox2.SelectedIndex].ID + ",'" +
                                            dateTimePicker1.Value.ToString("yyyy-MM-dd") + "','" +
                                            dateTimePicker2.Value.ToString("yyyy-MM-dd") + "')"
                                            );
                        break;
                    case 7:
                        dataGridView1.DataSource = await _dataBase.GetRecords("select *from getmissed_by_person(" + 
                                            employeeListboxes[comboBox2.SelectedIndex].ID + ",'" +
                                            dateTimePicker1.Value.ToString("yyyy-MM-dd") + "','" + 
                                            dateTimePicker2.Value.ToString("yyyy-MM-dd") + "')"
                                            );
                        break;
                    case 8:
                        dataGridView1.DataSource = await _dataBase.GetRecords("select *from getmissed_by_person_total(" +
                                            employeeListboxes[comboBox2.SelectedIndex].ID + ",'" +
                                            dateTimePicker1.Value.ToString("yyyy-MM-dd") + "','" +
                                            dateTimePicker2.Value.ToString("yyyy-MM-dd") + "')"
                                            );
                        break;
                    case 9:
                        dataGridView1.DataSource = await _dataBase.GetRecords("select *from getworked_hours_by_person(" +
                                            employeeListboxes[comboBox2.SelectedIndex].ID + ",'" +
                                            dateTimePicker1.Value.ToString("yyyy-MM-dd") + "','" +
                                            dateTimePicker2.Value.ToString("yyyy-MM-dd") + "')"
                                            );                        
                        break;
                    case 10:
                        dataGridView1.DataSource = await _dataBase.GetRecords("select *from getworked_hours_total_by_person(" +
                                            employeeListboxes[comboBox2.SelectedIndex].ID + ",'" +
                                            dateTimePicker1.Value.ToString("yyyy-MM-dd") + "','" +
                                            dateTimePicker2.Value.ToString("yyyy-MM-dd") + "')"
                                            );
                        break;
                    case 11:
                        dataGridView1.DataSource = await _dataBase.GetRecords("select *from get_extrawork_by_person(" +
                                            employeeListboxes[comboBox2.SelectedIndex].ID + ",'" +
                                            dateTimePicker1.Value.ToString("yyyy-MM-dd") + "','" +
                                            dateTimePicker2.Value.ToString("yyyy-MM-dd") + "')"
                                            );
                        
                        break;
                    case 12:
                        dataGridView1.DataSource = await _dataBase.GetRecords("select *from get_extra_worked_hours_total_by_person(" +
                                            employeeListboxes[comboBox2.SelectedIndex].ID + ",'" +
                                            dateTimePicker1.Value.ToString("yyyy-MM-dd") + "','" +
                                            dateTimePicker2.Value.ToString("yyyy-MM-dd") + "')"
                                            );
                        break;
                    case 13:
                        dataGridView1.DataSource = await _dataBase.GetRecords("select *from get_dayoff_worked_hours_by_person_total(" +
                                            employeeListboxes[comboBox2.SelectedIndex].ID + ",'" +
                                            dateTimePicker1.Value.ToString("yyyy-MM-dd") + "','" +
                                            dateTimePicker2.Value.ToString("yyyy-MM-dd") + "')"
                                            );
                        break;
                    case 14:
                        dataGridView1.DataSource = await _dataBase.GetRecords("select *from getbeing_factory_by_person(" + 
                                            employeeListboxes[comboBox2.SelectedIndex].ID + ",'" + 
                                            dateTimePicker1.Value.ToString("yyyy-MM-dd") + "','" +
                                            dateTimePicker2.Value.ToString("yyyy-MM-dd") + "')"
                                            );
                        break;
                    case 16:
                        dataGridView1.DataSource = await _dataBase.GetRecords("select t2.employeeid, t2.familiya, t2.ism, t2.otchestvo, t2.otdel, t2.lavozim, t1.sabab, t1.dan, t1.gacha " +
                                            "from otpusk t1 " +
                                            "inner join employee t2 " +
                                            "on t1.employeeid = t2.employeeid " +
                                            "where (t1.employeeid = " + employeeListboxes[comboBox2.SelectedIndex].ID + 
                                            " and dan >= '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + 
                                            "' and dan <= '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + 
                                            "') or (t1.employeeid = " + employeeListboxes[comboBox2.SelectedIndex].ID +
                                            " and gacha >= '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and gacha <= '" +
                                            dateTimePicker2.Value.ToString("yyyy-MM-dd") + "')"
                                            );
                        break;
                    case 17:
                        dataGridView1.DataSource = await _dataBase.GetRecords("select t2.employeeid, t2.familiya, t2.ism, t2.otchestvo, t2.otdel, t2.lavozim, t1.door, t1.sana, t1.temperature " +
                                            "from temperature t1 " +
                                            "inner join employee t2 " +
                                            "on t1.employeeid = t2.employeeid " +
                                            "where t1.employeeid = " + employeeListboxes[comboBox2.SelectedIndex].ID + 
                                            " and t1.sana >= '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + 
                                            "' and t1.sana <= '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "'"
                                            );
                        break;
                    case 18:
                        dataGridView1.DataSource = await _dataBase.GetRecords("select *from get_all_violation_by_person(" +
                                            employeeListboxes[comboBox2.SelectedIndex].ID + ",'" +
                                            dateTimePicker1.Value.ToString("yyyy-MM-dd") + "','" +
                                            dateTimePicker2.Value.ToString("yyyy-MM-dd") + "')"
                                            );
                        break;
                    case 19:
                        var dataTable = await _dataBase.GetRecords("select e.employeeid, e.ism, e.familiya, e.lavozim, r.kirish::date as kun, r.kirish, r.chiqish from employee e " +
                                                    "left join reports r " +
                                                    "on e.employeeid = r.employeeid " +
                                                    "where e.employeeid = " + employeeListboxes[comboBox2.SelectedIndex].ID +
                                                    " and r.kirish::date >= '" + dateTimePicker1.Text + "' " + 
                                                    "and r.kirish::date <= '" + dateTimePicker2.Text + "' " +
                                                    "group by e.employeeid, e.ism, e.familiya, e.lavozim, r.kirish, r.chiqish " +
                                                    "order by e.employeeid"
                                            );

                        if (dataTable == null || dataTable.Rows.Count < 1)
                            break;

                        var grouped = dataTable.AsEnumerable()
                                            .GroupBy(group => (group.Field<int>("employeeid"), group.Field<DateTime?>("kun")))
                                            .Select(group => group.OrderBy(x => x.Field<DateTime?>("kirish")));

                        var maxColumn = grouped.Select(group => group.Count()).Max();

                        var columns = maxColumn * 2;

                        dataGridView1.ColumnCount = 5 + columns;
                        dataGridView1.RowCount = grouped.Count();

                        dataGridView1.Columns[0].HeaderText = "ID";
                        dataGridView1.Columns[1].HeaderText = Properties.Resources.GRIDVIEW_SURNAME;
                        dataGridView1.Columns[2].HeaderText = Properties.Resources.GRIDVIEW_NAME;
                        dataGridView1.Columns[3].HeaderText = Properties.Resources.GRIDVIEW_POSITION;
                        dataGridView1.Columns[4].HeaderText = Properties.Resources.GRIDVIEW_DATE;
                        for (int i = 1; i <= columns; i++)
                        {
                            dataGridView1.Columns[i + 4].HeaderText = i % 2 != 0 ? Properties.Resources.GRIDVIEW_ENTER :
                                                                                   Properties.Resources.GRIDVIEW_EXIT;
                        }

                        int rowIndex = 0;
                        foreach (var group in grouped)
                        {
                            dataGridView1[0, rowIndex].Value = group.Select(s => s.Field<int>("employeeid")).First().ToString();
                            dataGridView1[1, rowIndex].Value = group.Select(s => s.Field<string>("familiya")).First().ToString();
                            dataGridView1[2, rowIndex].Value = group.Select(s => s.Field<string>("ism")).First().ToString();
                            dataGridView1[3, rowIndex].Value = group.Select(s => s.Field<string>("lavozim")).First().ToString();
                            dataGridView1[4, rowIndex].Value = group.Select(s => s.Field<DateTime?>("kun")).First()?.ToString("yyyy-MM-dd");

                            int columnIndex = 5;
                            foreach (var row in group)
                            {
                                dataGridView1[columnIndex, rowIndex].Value = row.Field<DateTime?>("kirish")?.ToString("HH:mm:ss");
                                columnIndex++;
                                dataGridView1[columnIndex, rowIndex].Value = row.Field<DateTime?>("chiqish")?.ToString("HH:mm:ss");
                                columnIndex++;
                            }
                            rowIndex++;
                        }

                        break;

                    case 20:
                        dataGridView1.DataSource = await _dataBase.GetRecords("select *from guest where kirish::date >= '"
                            + dateTimePicker1.Text + "' and kirish::date <= '" + dateTimePicker2.Text + "'");

                        break;
                    default: break;
                }
                StopProgress();
                label5.Text = "";
                button1.Enabled = true;
                button2.Enabled = true;
                GridHeaders(comboBox1.SelectedIndex);
            }
            catch (Exception msg)
            {
                MessageBox.Show(msg.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StopProgress();
                label5.Text = "";
                button1.Enabled = true;
                button2.Enabled = true;
            }
        }

        //bydepartment
        private async void Button1_Click(object sender, System.EventArgs e)
        {
            if (treeView1.Nodes.Count == 0 || string.IsNullOrEmpty(comboBox1.Text) || treeView1.SelectedNode == null)
                return;

            label5.Text = Properties.Resources.LOADING + " ";
            StartProgress();
            button1.Enabled = false;
            button2.Enabled = false;
            try
            {
                dataGridView1.DataSource = null;
                dataGridView1.Columns.Clear();
                dataGridView1.Rows.Clear();

                switch (comboBox1.SelectedIndex)
                {
                    case 0:
                        dataGridView1.DataSource = await _dataBase.GetRecords("select t2.employeeid, t2.familiya, t2.ism, t2.otchestvo, t2.otdel," +
                            " t2.lavozim, t1.door_name, t1.kirish, t1.chiqish " +
                                             "from reports t1 " +
                                             "inner join employee t2 " +
                                             "on t1.employeeid = t2.employeeid " +
                                             "where t1.kirish::date >= '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") +
                                             "' and t1.kirish::date <= '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") +
                                             "' and t2.department  <@ '" + treeView1.SelectedNode.Name + "'"
                                             );

                        //dataGridView1.DataSource = await _dataBase.GetRecords("select t2.employeeid, t2.familiya, t2.ism, t2.otchestvo, t2.otdel, t2.lavozim, t1.kirish, t1.chiqish " +
                        //                     "from reports t1 " +
                        //                     "inner join employee t2 " +
                        //                     "on t1.employeeid = t2.employeeid " +
                        //                     "where t1.kirish::date >= '" + dateTimePicker1.Text +
                        //                     "' and t1.kirish::date <= '" + dateTimePicker2.Text +
                        //                     "' and t2.department  <@ '" + treeView1.SelectedNode.Name + "'"
                        //                     );
                        break;
                    case 1:
                        dataGridView1.DataSource = await _dataBase.GetRecords("select *from getallevents_by_otdel('" +
                                            treeView1.SelectedNode.Name + "','" +
                                            dateTimePicker1.Value.ToString("yyyy-MM-dd") + "','" +
                                            dateTimePicker2.Value.ToString("yyyy-MM-dd") + "')" 
                                            );                        
                        break;
                    case 2:
                        dataGridView1.DataSource = await _dataBase.GetRecords("select *from getallevents_by_otdel_total('" +
                                            treeView1.SelectedNode.Name + "','" +
                                            dateTimePicker1.Value.ToString("yyyy-MM-dd") + "','" +
                                            dateTimePicker2.Value.ToString("yyyy-MM-dd") + "')"
                                            );
                        break;
                    case 3:
                        dataGridView1.DataSource = await _dataBase.GetRecords("select *from getlate_by_otdel('" +
                                            treeView1.SelectedNode.Name + "','" +
                                            dateTimePicker1.Value.ToString("yyyy-MM-dd") + "','" +
                                            dateTimePicker2.Value.ToString("yyyy-MM-dd") + "')");
                        break;
                    case 4:
                        dataGridView1.DataSource = await _dataBase.GetRecords("select *from getlate_by_otdel_total('" +
                                            treeView1.SelectedNode.Name + "','" +
                                            dateTimePicker1.Value.ToString("yyyy-MM-dd") + "','" +
                                            dateTimePicker2.Value.ToString("yyyy-MM-dd") + "')");
                        break;
                    case 5:
                        dataGridView1.DataSource = await _dataBase.GetRecords("select *from getearly_by_otdel('" +
                                            treeView1.SelectedNode.Name + "','" +
                                            dateTimePicker1.Value.ToString("yyyy-MM-dd") + "','" +
                                            dateTimePicker2.Value.ToString("yyyy-MM-dd") + "')"
                                            );
                        break;
                    case 6:
                        dataGridView1.DataSource = await _dataBase.GetRecords("select *from getearly_by_otdel_total('" +
                                            treeView1.SelectedNode.Name + "','" +
                                            dateTimePicker1.Value.ToString("yyyy-MM-dd") + "','" +
                                            dateTimePicker2.Value.ToString("yyyy-MM-dd") + "')"
                                            );
                        break;
                    case 7:
                        dataGridView1.DataSource = await _dataBase.GetRecords("select *from getmissed_by_otdel('" +
                                            treeView1.SelectedNode.Name + "','" +
                                            dateTimePicker1.Value.ToString("yyyy-MM-dd") + "','" +
                                            dateTimePicker2.Value.ToString("yyyy-MM-dd") + "')"
                                            );
                        break;
                    case 8:
                        dataGridView1.DataSource = await _dataBase.GetRecords("select *from getmissed_by_otdel_total('" +
                                            treeView1.SelectedNode.Name + "','" +
                                            dateTimePicker1.Value.ToString("yyyy-MM-dd") + "','" +
                                            dateTimePicker2.Value.ToString("yyyy-MM-dd") + "')"
                                            );
                        break;
                    case 9:
                        dataGridView1.DataSource = await _dataBase.GetRecords("select *from getworked_hours_by_otdel('" +
                                            treeView1.SelectedNode.Name + "','" +
                                            dateTimePicker1.Value.ToString("yyyy-MM-dd") + "','" +
                                            dateTimePicker2.Value.ToString("yyyy-MM-dd") + "')"
                                            );
                        break;
                    case 10:
                        dataGridView1.DataSource = await _dataBase.GetRecords("select *from getworked_hours_total_by_otdel('" +
                                            treeView1.SelectedNode.Name + "','" +
                                            dateTimePicker1.Value.ToString("yyyy-MM-dd") + "','" +
                                            dateTimePicker2.Value.ToString("yyyy-MM-dd") + "')"
                                            );
                        break;
                    case 11:
                        dataGridView1.DataSource = await _dataBase.GetRecords("select *from get_extrawork_by_otdel('" +
                                            treeView1.SelectedNode.Name + "','" +
                                            dateTimePicker1.Value.ToString("yyyy-MM-dd") + "','" +
                                            dateTimePicker2.Value.ToString("yyyy-MM-dd") + "')"
                                            );
                        break;
                    case 12:
                        dataGridView1.DataSource = await _dataBase.GetRecords("select *from get_extra_worked_hours_total_by_otdel('" +
                                            treeView1.SelectedNode.Name + "','" +
                                            dateTimePicker1.Value.ToString("yyyy-MM-dd") + "','" +
                                            dateTimePicker2.Value.ToString("yyyy-MM-dd") + "')"
                                            );
                        break;
                    case 13:
                        dataGridView1.DataSource = await _dataBase.GetRecords("select *from get_dayoff_worked_hours_by_otdel_total('" +
                                            treeView1.SelectedNode.Name + "','" +
                                            dateTimePicker1.Value.ToString("yyyy-MM-dd") + "','" +
                                            dateTimePicker2.Value.ToString("yyyy-MM-dd") + "')"
                                            );
                        break;
                    case 14:
                        dataGridView1.DataSource = await _dataBase.GetRecords("select *from getbeing_factory_by_otdel('" +
                                            treeView1.SelectedNode.Name + "','" +
                                            dateTimePicker1.Value.ToString("yyyy-MM-dd") + "','" +
                                            dateTimePicker2.Value.ToString("yyyy-MM-dd") + "')"
                                            );
                        break;
                    case 15:
                        dataGridView1.DataSource = await _dataBase.GetRecords("select *from getsotrudniki_vnutri_day('" +
                                            DateTime.Now.ToString("yyyy-MM-dd") + "','" +
                                            DateTime.Now.ToString("yyyy-MM-dd") + "')"
                                            );
                        break;
                    case 16:
                        dataGridView1.DataSource = await _dataBase.GetRecords("select t2.employeeid, t2.familiya, t2.ism, t2.otchestvo, t2.otdel, t2.lavozim, t1.sabab, t1.dan, t1.gacha " +
                                            "from otpusk t1 " +
                                            "inner join employee t2 " +
                                            "on t1.employeeid = t2.employeeid " +
                                            "where (t2.department  <@ '" + treeView1.SelectedNode.Name +
                                            "' and dan >= '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") +
                                            "' and dan <= '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") +
                                            "') or (t2.department  <@ '" + treeView1.SelectedNode.Name +
                                            "' and gacha >= '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") +
                                            "' and gacha <= '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "')"
                                            );
                        break;
                    case 17:
                        dataGridView1.DataSource = await _dataBase.GetRecords("select t2.employeeid, t2.familiya, t2.ism, t2.otchestvo, t2.otdel, t2.lavozim, t1.door, t1.sana, t1.temperature " +
                                            "from temperature t1 " +
                                            "inner join employee " +
                                            "t2 on t1.employeeid = t2.employeeid " +
                                            "where t2.department <@ '" + treeView1.SelectedNode.Name +
                                            "' and t1.sana::date >= '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") +
                                            "' and " + "t1.sana::date <= '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "'"
                                            );
                        break;
                    case 18:
                        dataGridView1.DataSource = await _dataBase.GetRecords("select *from get_all_violation_by_otdel('" +
                                            treeView1.SelectedNode.Name + "','" +
                                            dateTimePicker1.Value.ToString("yyyy-MM-dd") + "','" +
                                            dateTimePicker2.Value.ToString("yyyy-MM-dd") + "')"
                                            );
                        break;

                    case 19:
                        var dataTable = await _dataBase.GetRecords("select e.employeeid, e.ism, e.familiya, e.lavozim, r.kirish::date as kun, r.kirish, r.chiqish " +
                                                    "from (select employeeid, ism, familiya, lavozim from employee " +
                                                    "where department <@ '" + treeView1.SelectedNode.Name + "') e " +
                                                    "left join reports r " +
                                                    "on r.employeeid = e.employeeid " +
                                                    "and r.kirish::date >= '" + dateTimePicker1.Text + "' " +
                                                    "and r.kirish::date <= '" + dateTimePicker2.Text + "' " +
                                                    "order by e.employeeid"
                                            );

                        if (dataTable == null || dataTable.Rows.Count < 1)
                            break;

                        var grouped = dataTable.AsEnumerable()
                                            .GroupBy(group => (group.Field<int>("employeeid"), group.Field<DateTime?>("kun")))
                                            .Select(group => group.OrderBy(x => x.Field<DateTime?>("kirish")));

                        var maxColumn = grouped.Select(group => group.Count()).Max();

                        var columns = maxColumn * 2;

                        dataGridView1.ColumnCount = 5 + columns;
                        dataGridView1.RowCount = grouped.Count();

                        dataGridView1.Columns[0].HeaderText = "ID";
                        dataGridView1.Columns[1].HeaderText = Properties.Resources.GRIDVIEW_SURNAME;
                        dataGridView1.Columns[2].HeaderText = Properties.Resources.GRIDVIEW_NAME;
                        dataGridView1.Columns[3].HeaderText = Properties.Resources.GRIDVIEW_POSITION;
                        dataGridView1.Columns[4].HeaderText = Properties.Resources.GRIDVIEW_DATE;
                        for (int i = 1; i <= columns; i++)
                        {
                            dataGridView1.Columns[i + 4].HeaderText = i % 2 != 0 ? Properties.Resources.GRIDVIEW_ENTER :
                                                                                   Properties.Resources.GRIDVIEW_EXIT;
                        }

                        int rowIndex = 0;
                        foreach (var group in grouped)
                        {
                            dataGridView1[0, rowIndex].Value = group.Select(s => s.Field<int>("employeeid")).First().ToString();
                            dataGridView1[1, rowIndex].Value = group.Select(s => s.Field<string>("familiya")).First().ToString();
                            dataGridView1[2, rowIndex].Value = group.Select(s => s.Field<string>("ism")).First().ToString();
                            dataGridView1[3, rowIndex].Value = group.Select(s => s.Field<string>("lavozim")).First().ToString();
                            dataGridView1[4, rowIndex].Value = group.Select(s => s.Field<DateTime?>("kirish")).First()?.ToString("yyyy-MM-dd");

                            int columnIndex = 5;
                            foreach (var row in group)
                            {
                                dataGridView1[columnIndex, rowIndex].Value = row.Field<DateTime?>("kirish")?.ToString("HH:mm:ss");
                                columnIndex++;
                                dataGridView1[columnIndex, rowIndex].Value = row.Field<DateTime?>("chiqish")?.ToString("HH:mm:ss");
                                columnIndex++;
                            }
                            rowIndex++;
                        }


                        break;

                    case 20:
                        dataGridView1.DataSource = await _dataBase.GetRecords("select *from guest where kirish::date >= '"
                            + dateTimePicker1.Text + "' and kirish::date <= '" + dateTimePicker2.Text + "'");

                        break;
                    default: break;
                }

                if (treeView1.SelectedNode == treeView1.Nodes[0])
                    label5.Text = treeView1.SelectedNode.Text + " | ";
                else
                    label5.Text = Properties.Resources.GRIDVIEW_DEPARTMENT + ": " + treeView1.SelectedNode.Text + " | ";

                StopProgress();
                button1.Enabled = true;
                button2.Enabled = true;
                GridHeaders(comboBox1.SelectedIndex);
                RowCnt();
            }
            catch (Exception msg)
            {
                MessageBox.Show(msg.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StopProgress();
                label5.Text = "";
                button1.Enabled = true;
                button2.Enabled = true;
            }            
        }

        private void GridHeaders(int index)
        {
            switch (index)
            {
                case 0:
                    dataGridView1.Columns[0].HeaderText = "ID";
                    dataGridView1.Columns[1].HeaderText = Properties.Resources.GRIDVIEW_SURNAME;
                    dataGridView1.Columns[2].HeaderText = Properties.Resources.GRIDVIEW_NAME;
                    dataGridView1.Columns[3].HeaderText = Properties.Resources.GRIDVIEW_FAMILY_NAME;
                    dataGridView1.Columns[4].HeaderText = Properties.Resources.GRIDVIEW_DEPARTMENT;
                    dataGridView1.Columns[5].HeaderText = Properties.Resources.GRIDVIEW_POSITION;
                    dataGridView1.Columns[6].HeaderText = Properties.Resources.GRIDVIEW_DOOR;
                    dataGridView1.Columns[7].HeaderText = Properties.Resources.GRIDVIEW_ENTER;
                    dataGridView1.Columns[8].HeaderText = Properties.Resources.GRIDVIEW_EXIT;
                    break;

                case 1:
                    dataGridView1.Columns[0].HeaderText = "ID";
                    dataGridView1.Columns[1].HeaderText = Properties.Resources.GRIDVIEW_SURNAME;
                    dataGridView1.Columns[2].HeaderText = Properties.Resources.GRIDVIEW_NAME;
                    dataGridView1.Columns[3].HeaderText = Properties.Resources.GRIDVIEW_FAMILY_NAME;
                    dataGridView1.Columns[4].HeaderText = Properties.Resources.GRIDVIEW_DEPARTMENT;
                    dataGridView1.Columns[5].HeaderText = Properties.Resources.GRIDVIEW_POSITION;
                    dataGridView1.Columns[6].HeaderText = Properties.Resources.GRIDVIEW_HOLIDAY;
                    dataGridView1.Columns[7].HeaderText = Properties.Resources.GRIDVIEW_VACATION;
                    dataGridView1.Columns[8].HeaderText = Properties.Resources.GRIDVIEW_QUICK_OUTSIDE;
                    dataGridView1.Columns[9].HeaderText = Properties.Resources.GRIDVIEW_DATE;
                    dataGridView1.Columns[10].HeaderText = Properties.Resources.GRIDVIEW_GRAFIK;
                    dataGridView1.Columns[11].HeaderText = Properties.Resources.GRIDVIEW_GRAFIK_ENTER;
                    dataGridView1.Columns[12].HeaderText = Properties.Resources.GRIDVIEW_GRAFIK_EXIT;
                    dataGridView1.Columns[13].HeaderText = Properties.Resources.GRIDVIEW_ENTER;
                    dataGridView1.Columns[14].HeaderText = Properties.Resources.GRIDVIEW_LATE;
                    dataGridView1.Columns[15].HeaderText = Properties.Resources.GRIDVIEW_EXIT;
                    dataGridView1.Columns[16].HeaderText = Properties.Resources.GRIDVIEW_EARLY;
                    dataGridView1.Columns[17].HeaderText = Properties.Resources.GRIDVIEW_MISSING;
                    break;

                case 2:
                    dataGridView1.Columns[0].HeaderText = "ID";
                    dataGridView1.Columns[1].HeaderText = Properties.Resources.GRIDVIEW_SURNAME;
                    dataGridView1.Columns[2].HeaderText = Properties.Resources.GRIDVIEW_NAME;
                    dataGridView1.Columns[3].HeaderText = Properties.Resources.GRIDVIEW_FAMILY_NAME;
                    dataGridView1.Columns[4].HeaderText = Properties.Resources.GRIDVIEW_DEPARTMENT;
                    dataGridView1.Columns[5].HeaderText = Properties.Resources.GRIDVIEW_POSITION;
                    dataGridView1.Columns[6].HeaderText = Properties.Resources.GRIDVIEW_FROM;
                    dataGridView1.Columns[7].HeaderText = Properties.Resources.GRIDVIEW_TO;
                    dataGridView1.Columns[8].HeaderText = Properties.Resources.GRIDVIEW_WORKED;
                    dataGridView1.Columns[9].HeaderText = Properties.Resources.GRIDVIEW_HOURS;
                    dataGridView1.Columns[10].HeaderText = Properties.Resources.GRIDVIEW_WORKED_EXTRA;
                    dataGridView1.Columns[11].HeaderText = Properties.Resources.GRIDVIEW_LATE;
                    dataGridView1.Columns[12].HeaderText = Properties.Resources.GRIDVIEW_LATE_HOURS;
                    dataGridView1.Columns[13].HeaderText = Properties.Resources.GRIDVIEW_EARLY;
                    dataGridView1.Columns[14].HeaderText = Properties.Resources.GRIDVIEW_EARLY_HOURS;
                    dataGridView1.Columns[15].HeaderText = Properties.Resources.GRIDVIEW_MISSING;
                    break;

                case 3:
                    dataGridView1.Columns[0].HeaderText = "ID";
                    dataGridView1.Columns[1].HeaderText = Properties.Resources.GRIDVIEW_SURNAME;
                    dataGridView1.Columns[2].HeaderText = Properties.Resources.GRIDVIEW_NAME;
                    dataGridView1.Columns[3].HeaderText = Properties.Resources.GRIDVIEW_FAMILY_NAME;
                    dataGridView1.Columns[4].HeaderText = Properties.Resources.GRIDVIEW_DEPARTMENT;
                    dataGridView1.Columns[5].HeaderText = Properties.Resources.GRIDVIEW_POSITION;
                    dataGridView1.Columns[6].HeaderText = Properties.Resources.GRIDVIEW_DATE;
                    dataGridView1.Columns[7].HeaderText = Properties.Resources.GRIDVIEW_GRAFIK;
                    dataGridView1.Columns[8].HeaderText = Properties.Resources.GRIDVIEW_GRAFIK_ENTER;
                    dataGridView1.Columns[9].HeaderText = Properties.Resources.GRIDVIEW_LATE;
                    break;

                case 4:
                    dataGridView1.Columns[0].HeaderText = "ID";
                    dataGridView1.Columns[1].HeaderText = Properties.Resources.GRIDVIEW_SURNAME;
                    dataGridView1.Columns[2].HeaderText = Properties.Resources.GRIDVIEW_NAME;
                    dataGridView1.Columns[3].HeaderText = Properties.Resources.GRIDVIEW_FAMILY_NAME;
                    dataGridView1.Columns[4].HeaderText = Properties.Resources.GRIDVIEW_DEPARTMENT;
                    dataGridView1.Columns[5].HeaderText = Properties.Resources.GRIDVIEW_POSITION;
                    dataGridView1.Columns[6].HeaderText = Properties.Resources.GRIDVIEW_FROM;
                    dataGridView1.Columns[7].HeaderText = Properties.Resources.GRIDVIEW_TO;
                    dataGridView1.Columns[8].HeaderText = Properties.Resources.GRIDVIEW_LATE;
                    break;

                case 5:
                    dataGridView1.Columns[0].HeaderText = "ID";
                    dataGridView1.Columns[1].HeaderText = Properties.Resources.GRIDVIEW_SURNAME;
                    dataGridView1.Columns[2].HeaderText = Properties.Resources.GRIDVIEW_NAME;
                    dataGridView1.Columns[3].HeaderText = Properties.Resources.GRIDVIEW_FAMILY_NAME;
                    dataGridView1.Columns[4].HeaderText = Properties.Resources.GRIDVIEW_DEPARTMENT;
                    dataGridView1.Columns[5].HeaderText = Properties.Resources.GRIDVIEW_POSITION;
                    dataGridView1.Columns[6].HeaderText = Properties.Resources.GRIDVIEW_DATE;
                    dataGridView1.Columns[7].HeaderText = Properties.Resources.GRIDVIEW_GRAFIK;
                    dataGridView1.Columns[8].HeaderText = Properties.Resources.GRIDVIEW_GRAFIK_EXIT;
                    dataGridView1.Columns[9].HeaderText = Properties.Resources.GRIDVIEW_EARLY;
                    break;

                case 6:
                    dataGridView1.Columns[0].HeaderText = "ID";
                    dataGridView1.Columns[1].HeaderText = Properties.Resources.GRIDVIEW_SURNAME;
                    dataGridView1.Columns[2].HeaderText = Properties.Resources.GRIDVIEW_NAME;
                    dataGridView1.Columns[3].HeaderText = Properties.Resources.GRIDVIEW_FAMILY_NAME;
                    dataGridView1.Columns[4].HeaderText = Properties.Resources.GRIDVIEW_DEPARTMENT;
                    dataGridView1.Columns[5].HeaderText = Properties.Resources.GRIDVIEW_POSITION;
                    dataGridView1.Columns[6].HeaderText = Properties.Resources.GRIDVIEW_FROM;
                    dataGridView1.Columns[7].HeaderText = Properties.Resources.GRIDVIEW_TO;
                    dataGridView1.Columns[8].HeaderText = Properties.Resources.GRIDVIEW_EARLY;
                    break;

                case 7:
                    dataGridView1.Columns[0].HeaderText = "ID";
                    dataGridView1.Columns[1].HeaderText = Properties.Resources.GRIDVIEW_SURNAME;
                    dataGridView1.Columns[2].HeaderText = Properties.Resources.GRIDVIEW_NAME;
                    dataGridView1.Columns[3].HeaderText = Properties.Resources.GRIDVIEW_FAMILY_NAME;
                    dataGridView1.Columns[4].HeaderText = Properties.Resources.GRIDVIEW_DEPARTMENT;
                    dataGridView1.Columns[5].HeaderText = Properties.Resources.GRIDVIEW_POSITION;
                    dataGridView1.Columns[6].HeaderText = Properties.Resources.GRIDVIEW_DATE;
                    dataGridView1.Columns[7].HeaderText = Properties.Resources.GRIDVIEW_HOLIDAY;
                    dataGridView1.Columns[8].HeaderText = Properties.Resources.GRIDVIEW_VACATION;
                    break;

                case 8:
                    dataGridView1.Columns[0].HeaderText = "ID";
                    dataGridView1.Columns[1].HeaderText = Properties.Resources.GRIDVIEW_SURNAME;
                    dataGridView1.Columns[2].HeaderText = Properties.Resources.GRIDVIEW_NAME;
                    dataGridView1.Columns[3].HeaderText = Properties.Resources.GRIDVIEW_FAMILY_NAME;
                    dataGridView1.Columns[4].HeaderText = Properties.Resources.GRIDVIEW_DEPARTMENT;
                    dataGridView1.Columns[5].HeaderText = Properties.Resources.GRIDVIEW_POSITION;
                    dataGridView1.Columns[6].HeaderText = Properties.Resources.GRIDVIEW_FROM;
                    dataGridView1.Columns[7].HeaderText = Properties.Resources.GRIDVIEW_TO;
                    dataGridView1.Columns[8].HeaderText = Properties.Resources.GRIDVIEW_MISSING;
                    break;

                case 9:
                    dataGridView1.Columns[0].HeaderText = "ID";
                    dataGridView1.Columns[1].HeaderText = Properties.Resources.GRIDVIEW_SURNAME;
                    dataGridView1.Columns[2].HeaderText = Properties.Resources.GRIDVIEW_NAME;
                    dataGridView1.Columns[3].HeaderText = Properties.Resources.GRIDVIEW_FAMILY_NAME;
                    dataGridView1.Columns[4].HeaderText = Properties.Resources.GRIDVIEW_DEPARTMENT;
                    dataGridView1.Columns[5].HeaderText = Properties.Resources.GRIDVIEW_POSITION;
                    dataGridView1.Columns[6].HeaderText = Properties.Resources.GRIDVIEW_DATE;
                    dataGridView1.Columns[7].HeaderText = Properties.Resources.GRIDVIEW_HOLIDAY;
                    dataGridView1.Columns[8].HeaderText = Properties.Resources.GRIDVIEW_VACATION;
                    dataGridView1.Columns[9].HeaderText = Properties.Resources.GRIDVIEW_HOURS;
                    break;

                case 10:
                    dataGridView1.Columns[0].HeaderText = "ID";
                    dataGridView1.Columns[1].HeaderText = Properties.Resources.GRIDVIEW_SURNAME;
                    dataGridView1.Columns[2].HeaderText = Properties.Resources.GRIDVIEW_NAME;
                    dataGridView1.Columns[3].HeaderText = Properties.Resources.GRIDVIEW_FAMILY_NAME;
                    dataGridView1.Columns[4].HeaderText = Properties.Resources.GRIDVIEW_DEPARTMENT;
                    dataGridView1.Columns[5].HeaderText = Properties.Resources.GRIDVIEW_POSITION;
                    dataGridView1.Columns[6].HeaderText = Properties.Resources.GRIDVIEW_FROM;
                    dataGridView1.Columns[7].HeaderText = Properties.Resources.GRIDVIEW_TO;
                    dataGridView1.Columns[8].HeaderText = Properties.Resources.GRIDVIEW_HOURS;
                    break;                

                case 11:
                    dataGridView1.Columns[0].HeaderText = "ID";
                    dataGridView1.Columns[1].HeaderText = Properties.Resources.GRIDVIEW_SURNAME;
                    dataGridView1.Columns[2].HeaderText = Properties.Resources.GRIDVIEW_NAME;
                    dataGridView1.Columns[3].HeaderText = Properties.Resources.GRIDVIEW_FAMILY_NAME;
                    dataGridView1.Columns[4].HeaderText = Properties.Resources.GRIDVIEW_DEPARTMENT;
                    dataGridView1.Columns[5].HeaderText = Properties.Resources.GRIDVIEW_POSITION;
                    dataGridView1.Columns[6].HeaderText = Properties.Resources.GRIDVIEW_DATE;
                    dataGridView1.Columns[7].HeaderText = Properties.Resources.GRIDVIEW_HOURS;
                    break;

                case 12:
                    dataGridView1.Columns[0].HeaderText = "ID";
                    dataGridView1.Columns[1].HeaderText = Properties.Resources.GRIDVIEW_SURNAME;
                    dataGridView1.Columns[2].HeaderText = Properties.Resources.GRIDVIEW_NAME;
                    dataGridView1.Columns[3].HeaderText = Properties.Resources.GRIDVIEW_FAMILY_NAME;
                    dataGridView1.Columns[4].HeaderText = Properties.Resources.GRIDVIEW_DEPARTMENT;
                    dataGridView1.Columns[5].HeaderText = Properties.Resources.GRIDVIEW_POSITION;
                    dataGridView1.Columns[6].HeaderText = Properties.Resources.GRIDVIEW_FROM;
                    dataGridView1.Columns[7].HeaderText = Properties.Resources.GRIDVIEW_TO;
                    dataGridView1.Columns[8].HeaderText = Properties.Resources.GRIDVIEW_HOURS;
                    break;

                case 13:
                    dataGridView1.Columns[0].HeaderText = "ID";
                    dataGridView1.Columns[1].HeaderText = Properties.Resources.GRIDVIEW_SURNAME;
                    dataGridView1.Columns[2].HeaderText = Properties.Resources.GRIDVIEW_NAME;
                    dataGridView1.Columns[3].HeaderText = Properties.Resources.GRIDVIEW_FAMILY_NAME;
                    dataGridView1.Columns[4].HeaderText = Properties.Resources.GRIDVIEW_DEPARTMENT;
                    dataGridView1.Columns[5].HeaderText = Properties.Resources.GRIDVIEW_POSITION;
                    dataGridView1.Columns[6].HeaderText = Properties.Resources.GRIDVIEW_FROM;
                    dataGridView1.Columns[7].HeaderText = Properties.Resources.GRIDVIEW_TO;
                    dataGridView1.Columns[8].HeaderText = Properties.Resources.GRIDVIEW_HOURS;
                    break;

                case 14:
                    dataGridView1.Columns[0].HeaderText = "ID";
                    dataGridView1.Columns[1].HeaderText = Properties.Resources.GRIDVIEW_SURNAME;
                    dataGridView1.Columns[2].HeaderText = Properties.Resources.GRIDVIEW_NAME;
                    dataGridView1.Columns[3].HeaderText = Properties.Resources.GRIDVIEW_FAMILY_NAME;
                    dataGridView1.Columns[4].HeaderText = Properties.Resources.GRIDVIEW_DEPARTMENT;
                    dataGridView1.Columns[5].HeaderText = Properties.Resources.GRIDVIEW_POSITION;
                    dataGridView1.Columns[6].HeaderText = Properties.Resources.GRIDVIEW_DATE;
                    dataGridView1.Columns[7].HeaderText = Properties.Resources.GRIDVIEW_ENTER;
                    dataGridView1.Columns[8].HeaderText = Properties.Resources.GRIDVIEW_EXIT;
                    dataGridView1.Columns[9].HeaderText = Properties.Resources.GRIDVIEW_HOURS;
                    break;

                case 15:
                    dataGridView1.Columns[0].HeaderText = "ID";
                    dataGridView1.Columns[1].HeaderText = Properties.Resources.GRIDVIEW_SURNAME;
                    dataGridView1.Columns[2].HeaderText = Properties.Resources.GRIDVIEW_NAME;
                    dataGridView1.Columns[3].HeaderText = Properties.Resources.GRIDVIEW_FAMILY_NAME;
                    dataGridView1.Columns[4].HeaderText = Properties.Resources.GRIDVIEW_DEPARTMENT;
                    dataGridView1.Columns[5].HeaderText = Properties.Resources.GRIDVIEW_POSITION;
                    dataGridView1.Columns[6].HeaderText = Properties.Resources.GRIDVIEW_DATE;
                    break;

                case 16:
                    dataGridView1.Columns[0].HeaderText = "ID";
                    dataGridView1.Columns[1].HeaderText = Properties.Resources.GRIDVIEW_SURNAME;
                    dataGridView1.Columns[2].HeaderText = Properties.Resources.GRIDVIEW_NAME;
                    dataGridView1.Columns[3].HeaderText = Properties.Resources.GRIDVIEW_FAMILY_NAME;
                    dataGridView1.Columns[4].HeaderText = Properties.Resources.GRIDVIEW_DEPARTMENT;
                    dataGridView1.Columns[5].HeaderText = Properties.Resources.GRIDVIEW_POSITION;
                    dataGridView1.Columns[6].HeaderText = Properties.Resources.GRIDVIEW_VACATION;
                    dataGridView1.Columns[7].HeaderText = Properties.Resources.GRIDVIEW_FROM;
                    dataGridView1.Columns[8].HeaderText = Properties.Resources.GRIDVIEW_TO;
                    break;

                case 17:
                    dataGridView1.Columns[0].HeaderText = "ID";
                    dataGridView1.Columns[1].HeaderText = Properties.Resources.GRIDVIEW_SURNAME;
                    dataGridView1.Columns[2].HeaderText = Properties.Resources.GRIDVIEW_NAME;
                    dataGridView1.Columns[3].HeaderText = Properties.Resources.GRIDVIEW_FAMILY_NAME;
                    dataGridView1.Columns[4].HeaderText = Properties.Resources.GRIDVIEW_DEPARTMENT;
                    dataGridView1.Columns[5].HeaderText = Properties.Resources.GRIDVIEW_POSITION;
                    dataGridView1.Columns[6].HeaderText = Properties.Resources.GRIDVIEW_DOOR;
                    dataGridView1.Columns[7].HeaderText = Properties.Resources.GRIDVIEW_DATE;
                    dataGridView1.Columns[8].HeaderText = Properties.Resources.GRIDVIEW_TEMPERATURE;
                    break;
                case 18:
                    dataGridView1.Columns[0].HeaderText = "ID";
                    dataGridView1.Columns[1].HeaderText = Properties.Resources.GRIDVIEW_SURNAME;
                    dataGridView1.Columns[2].HeaderText = Properties.Resources.GRIDVIEW_NAME;
                    dataGridView1.Columns[3].HeaderText = Properties.Resources.GRIDVIEW_FAMILY_NAME;
                    dataGridView1.Columns[4].HeaderText = Properties.Resources.GRIDVIEW_DEPARTMENT;
                    dataGridView1.Columns[5].HeaderText = Properties.Resources.GRIDVIEW_POSITION;
                    dataGridView1.Columns[6].HeaderText = Properties.Resources.GRIDVIEW_HOLIDAY;
                    dataGridView1.Columns[7].HeaderText = Properties.Resources.GRIDVIEW_VACATION;
                    dataGridView1.Columns[8].HeaderText = Properties.Resources.GRIDVIEW_QUICK_OUTSIDE;
                    dataGridView1.Columns[9].HeaderText = Properties.Resources.GRIDVIEW_DATE;
                    dataGridView1.Columns[10].HeaderText = Properties.Resources.GRIDVIEW_GRAFIK;
                    dataGridView1.Columns[11].HeaderText = Properties.Resources.GRIDVIEW_GRAFIK_ENTER;
                    dataGridView1.Columns[12].HeaderText = Properties.Resources.GRIDVIEW_GRAFIK_EXIT;
                    dataGridView1.Columns[13].HeaderText = Properties.Resources.GRIDVIEW_ENTER;
                    dataGridView1.Columns[14].HeaderText = Properties.Resources.GRIDVIEW_LATE;
                    dataGridView1.Columns[15].HeaderText = Properties.Resources.GRIDVIEW_EXIT;
                    dataGridView1.Columns[16].HeaderText = Properties.Resources.GRIDVIEW_EARLY;
                    dataGridView1.Columns[17].HeaderText = Properties.Resources.GRIDVIEW_MISSING;
                    break;

                case 20:
                    dataGridView1.Columns[0].HeaderText = Properties.Resources.GRIDVIEW_DOOR;
                    dataGridView1.Columns[1].HeaderText = Properties.Resources.GRIDVIEW_ENTER;
                    break;

                default: break;
            }
        }

        private void RowCnt()
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    //foreach (DataGridViewRow row in dataGridView1.Rows)
                    //    row.Cells[7].Value = row.Cells[7].Value.Equals("вход") ? Properties.Resources.GRIDVIEW_ENTER : Properties.Resources.GRIDVIEW_EXIT;


                    //label5.Text += "    " + Properties.Resources.NUMBER_OF_EVENTS + ": " + dataGridView1.RowCount;                    
                    break;
                case 1:
                    int otp = 0, opzd = 0, rann = 0, ots = 0;
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (!string.IsNullOrEmpty(row.Cells[7].Value.ToString())) // otpusk
                            otp++;

                        if (!string.IsNullOrEmpty(row.Cells[14].Value.ToString()))
                            opzd++;

                        if (!string.IsNullOrEmpty(row.Cells[16].Value.ToString()))
                            rann++;

                        if (!string.IsNullOrEmpty(row.Cells[17].Value.ToString()))
                            ots++;
                    }
                    label5.Text += Properties.Resources.VACATION + ": " + otp +
                        "   |   " + Properties.Resources.LATE_COME + ": " + opzd +
                        "   |   " + Properties.Resources.EARLY_GONE + ": " + rann +
                        "   |   " + Properties.Resources.MISSING + ": " + ots;
                    break;
                case 2:
                    //otp = 0; opzd = 0; rann = 0; ots = 0;
                    //foreach (DataGridViewRow row in dataGridView1.Rows)
                    //{                  
                    //    if (row.Cells[8].Value.ToString() != "0")
                    //        opzd++;

                    //    if (row.Cells[9].Value.ToString() != "0")
                    //        rann++;

                    //    if (row.Cells[10].Value.ToString() != "0")
                    //        ots++;
                    //}
                    //label5.Text += Properties.Resources.LATE_COME + ": " + opzd +
                    //    "   |   " + Properties.Resources.EARLY_GONE + ": " + rann +
                    //    "   |   " + Properties.Resources.MISSING + ": " + ots;                    
                    break;
                case 3:
                    label5.Text += "    " + Properties.Resources.LATE_COME + ": " + dataGridView1.RowCount;

                    //label5.Text += "    " + Properties.Resources.EARLY_GONE + ": " + dataGridView1.RowCount;
                    break;
                case 4:
                    //label5.Text += "    " + Properties.Resources.LATE_COME + ": " + dataGridView1.RowCount;

                    //label5.Text += "    " + Properties.Resources.MISSING + ": " + dataGridView1.RowCount;
                    break;
                case 5:
                    label5.Text += "    " + Properties.Resources.EARLY_GONE + ": " + dataGridView1.RowCount;
                    break;
                case 6:
                    //label5.Text += "    " + Properties.Resources.EARLY_GONE + ": " + dataGridView1.RowCount;
                    break;
                case 7:
                    label5.Text += "    " + Properties.Resources.MISSING + ": " + dataGridView1.RowCount;
                    break;
                case 8:
                    //label5.Text += "    " + Properties.Resources.MISSING + ": " + dataGridView1.RowCount;
                    break;
                case 15:
                    label5.Text += "    " + Properties.Resources.EMPLOYEE_INSIDE + ": " + dataGridView1.RowCount;
                    break;
                case 16:
                    label5.Text += "    " + Properties.Resources.HOLIDAY + "&" + Properties.Resources.VACATION + ": " + dataGridView1.RowCount;
                    break;
                case 18:
                    label5.Text += "    " + Properties.Resources.VIOLATION + ": " + dataGridView1.RowCount;

                    break;
                case 19:
                    label5.Text += "    " + Properties.Resources.ENTERS + ": " + dataGridView1.RowCount;

                    break;
                case 20:
                    label5.Text += "    " + Properties.Resources.ENTERS + ": " + dataGridView1.RowCount;

                    break;
                default: label5.Text = ""; break;
            }
        }

        private void StartProgress()
        {
            timer1.Enabled = true;
            timer1.Start();
        }

        private void StopProgress()
        {
            timer1.Enabled = false;
            timer1.Stop();
        }

        private void DataGridView1_SelectionChanged(object sender, System.EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
                return; 
            
            dataGridView1.Rows[dataGridView1.CurrentRow.Index].Selected = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void PictureBox1_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count < 1)
                return;

            var info = treeView1.Nodes[0].Text + ": " + Properties.Resources.REPORT_FROM + dateTimePicker1.Text +
                        ", " + Properties.Resources.GRIDVIEW_TO + " " + dateTimePicker2.Text +
                        ", " + Properties.Resources.FILTER + " " + comboBox1.Text;

            if(comboBox1.SelectedIndex == 20)
            {
                fileWriter.Save2(dataGridView1, info, label5.Text);
                return;
            }

            fileWriter.Save(dataGridView1, treeView1, info, label5.Text);
        }

        private void PictureBox4_Click_1(object sender, EventArgs e)
        {
            new Holiday().ShowDialog();
        }

        private void PictureBox2_Click_1(object sender, EventArgs e)
        {
            new UserSettings().ShowDialog();
        }

        private void PictureBox1_MouseEnter_1(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources.excel_light;
        }

        private void PictureBox1_MouseLeave_1(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources.excel;
        }

        private void PictureBox4_MouseEnter_1(object sender, EventArgs e)
        {
            pictureBox4.Image = Properties.Resources.calendar;
        }

        private void PictureBox4_MouseLeave_1(object sender, EventArgs e)
        {
            pictureBox4.Image = Properties.Resources.calendar_light;
        }

        private void PictureBox2_MouseEnter_1(object sender, EventArgs e)
        {
            pictureBox2.Image = Properties.Resources.user_holiday;
        }

        private void PictureBox2_MouseLeave_1(object sender, EventArgs e)
        {
            pictureBox2.Image = Properties.Resources.user_holiday_light;
        }

        private void Button3_MouseEnter(object sender, EventArgs e)
        {
            button3.BackColor = System.Drawing.Color.Red;
        }

        private void Button3_MouseLeave(object sender, EventArgs e)
        {            
            button3.BackColor = System.Drawing.Color.FromArgb(68, 68, 68);
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            MaximizeCheck();
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void Panel1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                Location = new System.Drawing.Point(
                    (Location.X - lastLocation.X) + e.X, (Location.Y - lastLocation.Y) + e.Y);

                Update();
            }
        }

        private void Panel5_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void Panel5_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void Panel5_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                Location = new System.Drawing.Point(
                    (Location.X - lastLocation.X) + e.X, (Location.Y - lastLocation.Y) + e.Y);

                Update();
            }
        }

        private void LanguageEnglish_Click(object sender, EventArgs e)
        {
            ChangeLanguage("en");
        }

        private void LanguageRussian_Click(object sender, EventArgs e)
        {
            ChangeLanguage("ru");
        }

        private void ChangeLanguage(string language)
        {
            var dialogResult = MessageBox.Show(
                Properties.Resources.CONTROL_LANGUAGE_CHANGE,
                Properties.Resources.WARNING,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    _dataBase.InsertData("update language set lan = '" + language + "'");
                    System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo(language);
                    var resources = new System.Resources.ResourceManager("Reports.Form1", System.Reflection.Assembly.GetExecutingAssembly());
                    dataGridView1.DataSource = null;
                    label3.Text = resources.GetString("label3.Text");
                    label4.Text = resources.GetString("label4.Text");
                    button1.Text = resources.GetString("button1.Text");
                    button2.Text = resources.GetString("button2.Text");
                    label5.Text = "";
                    comboBox1.Items.Clear();
                    comboBox1.Items.AddRange(new object[] {
                    resources.GetString("comboBox1.Items"),
                    resources.GetString("comboBox1.Items1"),
                    resources.GetString("comboBox1.Items2"),
                    resources.GetString("comboBox1.Items3"),
                    resources.GetString("comboBox1.Items4"),
                    resources.GetString("comboBox1.Items5"),
                    resources.GetString("comboBox1.Items6"),
                    resources.GetString("comboBox1.Items7"),
                    resources.GetString("comboBox1.Items8"),
                    resources.GetString("comboBox1.Items9"),
                    resources.GetString("comboBox1.Items10"),
                    resources.GetString("comboBox1.Items11"),
                    resources.GetString("comboBox1.Items12"),
                    resources.GetString("comboBox1.Items13"),
                    resources.GetString("comboBox1.Items14"),
                    resources.GetString("comboBox1.Items15"),
                    resources.GetString("comboBox1.Items16"),
                    resources.GetString("comboBox1.Items17"),
                    resources.GetString("comboBox1.Items18"),
                    resources.GetString("comboBox1.Items19")});
                comboBox1.SelectedIndex = 0;                    
                }
                catch (Exception msg)
                {
                    MessageBox.Show(msg.ToString(), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }                
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (label5.Text.Length > 15)
                label5.Text = Properties.Resources.LOADING + " ";

            label5.Text += ".";
        }

        private void pictureBox6_MouseEnter(object sender, EventArgs e)
        {
            pictureBox6.Image = Properties.Resources.alarm_clock_white;
        }

        private void pictureBox6_MouseLeave(object sender, EventArgs e)
        {
            pictureBox6.Image = Properties.Resources.alarm_clock;
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            new BreakReport().ShowDialog();
        }
    }
}
