using System;
using System.Windows.Forms;

namespace Reports
{
    public class Holiday : BaseForms.HolidayBase
    {
        public Holiday()
        {
            InitializeComponent();
            MaximizedBounds = Screen.FromHandle(Handle).WorkingArea;
            WindowState = FormWindowState.Maximized;
            _dataBase = new DataBase();
            _dataBase.getRecords("select id, bayram_nomi, dan, gacha from holidays order by id desc", dataGridView1);
            dateTimePicker1.Text = DateTime.Now.ToString("yyyy-MM-dd");
            dateTimePicker2.Text = DateTime.Now.ToString("yyyy-MM-dd");
            Setheaders();
        }

        private DataBase _dataBase;

        public override void Setheaders()
        {
            dataGridView1.Columns[0].HeaderText = "Index";
            dataGridView1.Columns[1].HeaderText = Properties.Resources.HOLIDAY;
            dataGridView1.Columns[2].HeaderText = Properties.Resources.GRIDVIEW_FROM;
            dataGridView1.Columns[3].HeaderText = Properties.Resources.GRIDVIEW_TO;
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
            // Holiday
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(844, 528);
            this.Name = "Holiday";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void btn_do_Click(object sender, EventArgs e)
        {
            if (checkDate())
            {
                if (_dataBase.checkRow("select exists(select 1 from holidays where ('" +
                dateTimePicker1.Text + "' >= dan and '" + dateTimePicker1.Text + "' <= gacha) or " +
                "('" + dateTimePicker2.Text + "' >= dan and '" + dateTimePicker2.Text + "' <= gacha)" +
                " or (dan >= '" + dateTimePicker1.Text + "' and dan <= '" + dateTimePicker2.Text + "'))"))
                {
                    MessageBox.Show(Properties.Resources.HOLIDAY_INFO, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                _dataBase.insertData("insert into holidays (bayram_nomi, dan, gacha) " +
                "values('" + textBox1.Text + "','" + dateTimePicker1.Text + "','" + dateTimePicker2.Text + "')");

                _dataBase.getRecords("select id, bayram_nomi, dan, gacha from holidays order by id desc", dataGridView1);
                Setheaders();
            }
            else
            {
                MessageBox.Show(Properties.Resources.DATE_FUTURE, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count < 1)
                return;           

            if (Check())
            {
                _dataBase.insertData("delete from holidays where id = " + 
                Convert.ToInt32(dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value));
                _dataBase.getRecords("select id, bayram_nomi, dan, gacha from holidays order by id desc", dataGridView1);
                Setheaders();
            }
        }
    }
}
