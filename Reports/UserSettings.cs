using System.Windows.Forms;

namespace Reports
{
    public partial class UserSettings : BaseForms.BaseForm
    {
        public UserSettings()
        {
            InitializeComponent();            
            myDatabase = new MyDatabase();
            fillTree();
        }

        private MyDatabase myDatabase;
        private System.Drawing.Point lastLocation;
        private bool mouseDown = false;

        private void fillTree()
        {
            System.Collections.Generic.List<MyTree> myTrees = myDatabase.getTree("select ttext, mytree from department order by id asc");
            for (int i = 0; i < myTrees.Count; i++)
            {
                TreeNode tnode = new TreeNode();
                tnode.Name = myTrees[i].Tname;
                tnode.Text = myTrees[i].Ttext;
                FindByText(tnode, treeView1);
            }
        }

        private void getEmpl(string str)
        {
            flowLayoutPanel1.Controls.Clear();
            System.Collections.Generic.List<MyControls.Employee> employees = myDatabase.GetEmployees(str);
            foreach (MyControls.Employee employee in employees)
            {
                flowLayoutPanel1.Controls.Add(employee);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            getEmpl("select employeeid, familiya, ism, otchestvo, photo::bytea, otdel, lavozim from employee where" +
                " department <@ '" + treeView1.SelectedNode.Name + "'");
        }

        private void btn_close_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void btn_min_Click(object sender, System.EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
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
                this.Location = new System.Drawing.Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }
    }
}
