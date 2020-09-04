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
            myDatabase = new MyDatabase();
            myDatabase.getRecords("select id, bayram_nomi, dan, gacha from holidays order by id desc", dataGridView1);
            dateTimePicker1.Text = DateTime.Now.ToString("yyyy-MM-dd");
            dateTimePicker2.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

        private MyDatabase myDatabase;

        public override void Setheaders()
        {
            dataGridView1.Columns[0].HeaderText = "Index";
            dataGridView1.Columns[1].HeaderText = "Название предприятие";
            dataGridView1.Columns[2].HeaderText = "От";
            dataGridView1.Columns[3].HeaderText = "До";
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
                if (myDatabase.checkRow("select exists(select 1 from holidays where ('" +
                dateTimePicker1.Text + "' >= dan and '" + dateTimePicker1.Text + "' <= gacha) or " +
                "('" + dateTimePicker2.Text + "' >= dan and '" + dateTimePicker2.Text + "' <= gacha)" +
                " or (dan >= '" + dateTimePicker1.Text + "' and dan <= '" + dateTimePicker2.Text + "'))"))
                {
                    MessageBox.Show("Праздник существует в этом периоде времени");
                    return;
                }
                myDatabase.insertData("insert into holidays (bayram_nomi, dan, gacha) " +
                "values('" + textBox1.Text + "','" + dateTimePicker1.Text + "','" + dateTimePicker2.Text + "')");

                myDatabase.getRecords("select id, bayram_nomi, dan, gacha from holidays order by id desc", dataGridView1);
                Setheaders();
            }
            else
            {
                MessageBox.Show("Дата должна быть установлена ​​на будущее");
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count < 1)
                return;           

            if (Check())
            {
                myDatabase.insertData("delete from holidays where id = " + 
                Convert.ToInt32(dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value));
                myDatabase.getRecords("select id, bayram_nomi, dan, gacha from holidays order by id desc", dataGridView1);
                Setheaders();
            }
        }
    }
}
