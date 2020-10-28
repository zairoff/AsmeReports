using System;
using System.Windows.Forms;

namespace Reports
{
    class Uvolnitelniy : BaseForms.HolidayBase
    {
        public Uvolnitelniy(int userID)
        {
            InitializeComponent();
            MaximizedBounds = Screen.FromHandle(Handle).WorkingArea;
            WindowState = FormWindowState.Maximized;
            this.userID = userID;
            myDatabase = new DataBase();
            myDatabase.getRecords("select t1.id, t2.familiya, t2.ism, t2.otdel, t2.lavozim, t1.sabab," +
            "t1.dan, t1.gacha from uvolnitelnie t1 inner join employee t2 on t1.employeeid = t2.employeeid " +
            "where t1.employeeid = " + userID + " order by t1.id desc", dataGridView1);
            dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            dateTimePicker2.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            dateTimePicker1.Text = DateTime.Now.ToString("yyyy-MM-dd 00:01:00");
            dateTimePicker2.Text = DateTime.Now.ToString("yyyy-MM-dd 23:59:00");
            comboBox1.Items.Add(Properties.Resources.QUICK_OUTSIDE_UNPAID);
            comboBox1.Items.Add(Properties.Resources.QUICK_OUTSIDE_PAID);
            comboBox1.SelectedIndex = 0;
            Setheaders();
        }

        private DataBase myDatabase;
        private readonly int userID;

        public override void Setheaders()
        {
            dataGridView1.Columns[0].HeaderText = "Index";
            dataGridView1.Columns[1].HeaderText = Properties.Resources.GRIDVIEW_SURNAME;
            dataGridView1.Columns[2].HeaderText = Properties.Resources.GRIDVIEW_NAME;
            dataGridView1.Columns[3].HeaderText = Properties.Resources.GRIDVIEW_DEPARTMENT;
            dataGridView1.Columns[4].HeaderText = Properties.Resources.GRIDVIEW_POSITION;
            dataGridView1.Columns[5].HeaderText = Properties.Resources.GRIDVIEW_REASON;
            dataGridView1.Columns[6].HeaderText = Properties.Resources.GRIDVIEW_FROM;
            dataGridView1.Columns[7].HeaderText = Properties.Resources.GRIDVIEW_TO;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // btn_delete
            // 
            this.btn_delete.Click += new System.EventHandler(this.btn_delete_Click);
            // 
            // btn_do
            // 
            this.btn_do.Click += new System.EventHandler(this.btn_do_Click);
            // 
            // Uvolnitelniy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(844, 528);
            this.Name = "Uvolnitelniy";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void btn_do_Click(object sender, EventArgs e)
        {
            if (checkDate())
            {
                MessageBox.Show(Properties.Resources.DATE_FUTURE, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if ((dateTimePicker2.Value - dateTimePicker1.Value).Days != 0)
            {
                MessageBox.Show(Properties.Resources.QUICK_OUTSIDE_INFO, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (myDatabase.checkRow("select exists(select 1 from uvolnitelnie where (employeeid = " +
            userID + " and '" + dateTimePicker1.Text + "' >= dan and '" + dateTimePicker1.Text + "' <= gacha) or " +
            "(employeeid = " + userID + " and '" + dateTimePicker2.Text + "' >= dan and '" +
            dateTimePicker2.Text + "' <= gacha) or (employeeid = " + userID + " and dan >= '" +
            dateTimePicker1.Text + "' and dan <= '" + dateTimePicker2.Text + "'))"))
            {
                MessageBox.Show(Properties.Resources.EMPLOYEE_IN_PERIOD_TIME, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            myDatabase.insertData("insert into uvolnitelnie (employeeid, status, sabab, dan, gacha) " +
            "values(" + userID + "," + comboBox1.SelectedIndex + ",'" + textBox1.Text + "','" +
            dateTimePicker1.Text + "','" + dateTimePicker2.Text + "')");

            myDatabase.getRecords("select t1.id, t2.familiya, t2.ism, t2.otdel, t2.lavozim, t1.sabab," +
            "t1.dan, t1.gacha from uvolnitelnie t1 inner join employee t2 on t1.employeeid = t2.employeeid " +
            "where t1.employeeid = " + userID + " order by t1.id desc", dataGridView1);
            Setheaders();                                                             
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count < 1)
                return;

            if (Check())
            {
                myDatabase.insertData("delete from uvolnitelnie where id = " +
                Convert.ToInt32(dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value));
                myDatabase.getRecords("select t1.id, t2.familiya, t2.ism, t2.otdel, t2.lavozim, t1.sabab," +
                "t1.dan, t1.gacha from uvolnitelnie t1 inner join employee t2 on t1.employeeid = t2.employeeid " +
                "where t1.employeeid = " + userID + " order by t1.id desc", dataGridView1);
                Setheaders();
            }
        }
    }
}
