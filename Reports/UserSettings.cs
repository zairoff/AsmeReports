using System.Text;
using System.Windows.Forms;

namespace Reports
{
    public partial class UserSettings : BaseForms.BaseForm
    {
        public UserSettings()
        {
            InitializeComponent();            
            _dataBase = new DataBase();
            _department = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(System.Configuration.ConfigurationManager.AppSettings["department"]));
            FillTree();
        }

        private DataBase _dataBase;
        private System.Drawing.Point lastLocation;
        private bool mouseDown = false;
        private int _dataBaseLimit = 50;
        private int _databaseOffset = 0;
        private readonly string _department;

        private void FillTree()
        {
            System.Collections.Generic.List<MyTree> myTrees = _dataBase.GetTree("select ttext, mytree from department where mytree <@ '" + _department + "' order by id asc");
            for (int i = 0; i < myTrees.Count; i++)
            {
                TreeNode tnode = new TreeNode();
                tnode.Name = myTrees[i].Tname;
                tnode.Text = myTrees[i].Ttext;
                FindByText(tnode, treeView1);
            }
        }

        private void GetEmployee(string str)
        {          
            flowLayoutPanel1.Controls.Clear();
            var employees = _dataBase.GetEmployees(str);
            for (int i = employees.Count - 1; i >= 0; i--)
                flowLayoutPanel1.Controls.Add(employees[i]);
        }

        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            GetEmployee("select employeeid, familiya, ism, otchestvo, photo::bytea, otdel, lavozim " +
                        "from employee where department <@ '" + treeView1.SelectedNode.Name +
                        "' and status = true order by employeeid asc LIMIT " + _dataBaseLimit);

            _databaseOffset = 0;
        }

        private void Btn_close_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void Btn_min_Click(object sender, System.EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
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
                this.Location = new System.Drawing.Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void TextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
                return;

            var query = "select employeeid, familiya, ism, otchestvo, photo::bytea, otdel, lavozim from employee where" +
                " familiya ILIKE '" + textBox1.Text.Trim() + "%' and status = true";

            GetEmployee(query);
        }

        private void ForwardBtn_MouseEnter(object sender, System.EventArgs e)
        {
            ForwardBtn.Image = Properties.Resources.forward;
        }

        private void ForwardBtn_MouseLeave(object sender, System.EventArgs e)
        {
            ForwardBtn.Image = Properties.Resources.forward_dark;
        }

        private void ForwardBtn_Click(object sender, System.EventArgs e)
        {
            if (treeView1.Nodes.Count == 0 || treeView1.SelectedNode == null)
                return;

            _databaseOffset += _dataBaseLimit;

            GetEmployee("select employeeid, familiya, ism, otchestvo, photo::bytea, otdel, lavozim " +
                        "from employee where department <@ '" + treeView1.SelectedNode.Name +
                        "' and status = true order by employeeid asc LIMIT " + _dataBaseLimit + 
                        " OFFSET " + _databaseOffset);
        }

        private void BackBtn_MouseEnter(object sender, System.EventArgs e)
        {
            BackBtn.Image = Properties.Resources.back;
        }

        private void BackBtn_MouseLeave(object sender, System.EventArgs e)
        {
            BackBtn.Image = Properties.Resources.back_dark;
        }

        private void BackBtn_Click(object sender, System.EventArgs e)
        {
            if (_databaseOffset < _dataBaseLimit || treeView1.Nodes.Count == 0 || treeView1.SelectedNode == null)
                return;

            _databaseOffset -= _dataBaseLimit;

            GetEmployee("select employeeid, familiya, ism, otchestvo, photo::bytea, otdel, lavozim " +
                        "from employee where department <@ '" + treeView1.SelectedNode.Name +
                        "' and status = true order by employeeid asc LIMIT " + _dataBaseLimit +
                        " OFFSET " + _databaseOffset);
        }
    }
}
