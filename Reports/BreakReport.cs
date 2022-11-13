using Npgsql;
using Reports.File;
using Reports.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Reports
{
    public partial class BreakReport : BaseForms.BaseForm
    {
        public BreakReport()
        {
            InitializeComponent();
            _dataBase = new DataBase();
            FillTree();
            fileWriter = new Excel();
            FillBreaks();
            dateTimePicker1.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

        private DataBase _dataBase;
        private bool max_check;
        private Point lastLocation;
        private bool mouseDown = false;
        private readonly IFileWriter fileWriter;
        private List<BreakModel> _breaks;

        public void NotifiactionHandler(object sender, EventArgs e)
        {
            FillBreaks();
        }

        private List<BreakModel> GetBreaks()
        {
            var breaks = new List<BreakModel>();
            using (var conn = new NpgsqlConnection(Helper.CnnVal("DBConnection")))
            {
                using (var cmd = new NpgsqlCommand("select *from breaks", conn))
                {
                    conn.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var br = new BreakModel
                        {
                            Name = reader[0].ToString(),
                            From = reader[1].ToString(),
                            To = reader[2].ToString()
                        };
                        breaks.Add(br);
                    }
                }
            }
            return breaks;
        }

        private void FillBreaks()
        {
            _breaks = GetBreaks();

            if (_breaks.Count < 1)
                return;

            comboBox1.Items.Clear();
            for(int i = 0; i < _breaks.Count; i++)
            {
                comboBox1.Items.Add(_breaks[i].Name);
            }

            comboBox1.SelectedIndex = 0;
        }

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
            List<MyTree> myTrees = _dataBase.GetTree("select ttext, mytree from department order by id asc");
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

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (treeView1 == null || treeView1.SelectedNode == null || comboBox1.Items.Count < 1)
                return;

            label5.Text = Properties.Resources.LOADING + " ";
            StartProgress();
            button1.Enabled = false;

            try
            {
                dataGridView1.DataSource = null;
                dataGridView1.Columns.Clear();
                dataGridView1.Rows.Clear();

                var timeFrom = TimeSpan.Parse(_breaks[comboBox1.SelectedIndex].From);
                var timeTo = TimeSpan.Parse(_breaks[comboBox1.SelectedIndex].To);

                DateTime dateFrom = dateTimePicker1.Value;
                DateTime dateTo = dateTimePicker1.Value;

                if(timeFrom > timeTo)
                {
                    dateTo = dateTo.AddDays(1);
                }

                var from = $"{dateFrom.ToString("yyyy-MM-dd")} {_breaks[comboBox1.SelectedIndex].From}";
                var to = $"{dateTo.ToString("yyyy-MM-dd")} {_breaks[comboBox1.SelectedIndex].To}";

                //MessageBox.Show($"from: {from}, to:{to}");

                dataGridView1.DataSource = await _dataBase.GetRecords("select distinct e.employeeid, e.familiya, e.ism, e.otdel, e.lavozim " +
                                                    "from employee e " +
                                                    "inner join reports r " +
                                                    "on e.employeeid = r.employeeid " +
                                                    "where e.department <@ '" + treeView1.SelectedNode.Name + "' " +
                                                    "and r.kirish >= '" + from + "' " +
                                                    "and r.kirish <= '" + to + "' " +
                                                    "order by e.otdel"
                                            );

                if (treeView1.SelectedNode == treeView1.Nodes[0])
                    label5.Text = treeView1.SelectedNode.Text + " | ";
                else
                    label5.Text = Properties.Resources.GRIDVIEW_DEPARTMENT + ": " + treeView1.SelectedNode.Text + " | ";

                StopProgress();
                button1.Enabled = true;
                GridHeaders();
                label5.Text += "    " + Properties.Resources.ENTERS + ": " + dataGridView1.RowCount;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GridHeaders()
        {
            dataGridView1.Columns[0].HeaderText = "ID";
            dataGridView1.Columns[1].HeaderText = Properties.Resources.GRIDVIEW_SURNAME;
            dataGridView1.Columns[2].HeaderText = Properties.Resources.GRIDVIEW_NAME;
            dataGridView1.Columns[3].HeaderText = Properties.Resources.GRIDVIEW_DEPARTMENT;
            dataGridView1.Columns[4].HeaderText = Properties.Resources.GRIDVIEW_POSITION;
        }

        private void GuestGridHeaders()
        {
            dataGridView1.Columns[0].HeaderText = Properties.Resources.GRIDVIEW_DOOR;
            dataGridView1.Columns[1].HeaderText = Properties.Resources.GRIDVIEW_ENTER;
        }

        private void StopProgress()
        {
            timer1.Enabled = false;
            timer1.Stop();
        }

        private void StartProgress()
        {
            timer1.Enabled = true;
            timer1.Start();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                Location = new Point(
                    (Location.X - lastLocation.X) + e.X, (Location.Y - lastLocation.Y) + e.Y);

                Update();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (label5.Text.Length > 15)
                label5.Text = Properties.Resources.LOADING + " ";

            label5.Text += ".";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MaximizeCheck();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            var breakForm = new BreakForm();
            breakForm.Notify += NotifiactionHandler;
            breakForm.ShowDialog();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count < 1)
                return;

            var from = $"{dateTimePicker1.Text} {_breaks[comboBox1.SelectedIndex].From}";
            var to = $"{dateTimePicker1.Text} {_breaks[comboBox1.SelectedIndex].To}";

            var info = treeView1.Nodes[0].Text + ": " + Properties.Resources.REPORT_FROM + " " + from +
                        ", " + Properties.Resources.GRIDVIEW_TO + " " + to +
                        ", " + Properties.Resources.FILTER + " " + comboBox1.Text;

            fileWriter.Save2(dataGridView1, info, label5.Text);
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.Items.Count < 1)
                return;

            label5.Text = Properties.Resources.LOADING + " ";
            StartProgress();
            button1.Enabled = false;

            try
            {
                dataGridView1.DataSource = null;
                dataGridView1.Columns.Clear();
                dataGridView1.Rows.Clear();

                var from = $"{dateTimePicker1.Text} {_breaks[comboBox1.SelectedIndex].From}";
                var to = $"{dateTimePicker1.Text} {_breaks[comboBox1.SelectedIndex].To}";

                dataGridView1.DataSource = await _dataBase.GetRecords("select *from guest where kirish >= '"
                            + from + "' and kirish <= '" + to + "'");

                if (treeView1.SelectedNode == treeView1.Nodes[0])
                    label5.Text = treeView1.SelectedNode.Text + " | ";
                else
                    label5.Text += "    " + Properties.Resources.ENTERS + ": " + dataGridView1.RowCount;

                StopProgress();
                button1.Enabled = true;
                GuestGridHeaders();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
