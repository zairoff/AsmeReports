using System;
using System.Windows.Forms;
using Npgsql;

namespace Reports
{
    public partial class BreakForm : Form
    {
        public BreakForm()
        {
            InitializeComponent();
            _dataBase = new DataBase();
            _dataBase.InsertData("create table if not exists breaks(" +
                                        "name text," +
                                        "start_time time not null," +
                                        "end_time time not null);");

            GetRecords();
        }

        private readonly DataBase _dataBase;
        public event EventHandler Notify;

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
                return;

            try
            {
                var isExist = _dataBase.CheckRow("select exists(select 1 from breaks where name='" + textBox1.Text + "')");

                if (isExist)
                {
                    MessageBox.Show($"{textBox1.Text} существует");
                    return;
                }

                _dataBase.InsertData("insert into breaks(name, start_time, end_time) values('" + textBox1.Text +
                                        "','" + dateTimePicker1.Text +
                                        "','" + dateTimePicker2.Text + "')");

                GetRecords();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Notify?.Invoke(this, EventArgs.Empty);
            Close();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count < 1)
                return;

            if (dataGridView1.CurrentCell.ColumnIndex.Equals(3) && e.RowIndex != -1)
            {
                try
                {
                    _dataBase.InsertData("delete from breaks where name ='" +
                        dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString() + "'");

                    GetRecords();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void GetRecords()
        {
            dataGridView1.Rows.Clear();
            using (var conn = new NpgsqlConnection(Helper.CnnVal("DBConnection")))
            {
                using (var cmd = new NpgsqlCommand("select *from breaks", conn))
                {
                    conn.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        dataGridView1.Rows.Insert(0,
                                        reader[0].ToString(),
                                        reader[1].ToString(),
                                        reader[2].ToString());
                    }
                }
            }
        }
    }
}
