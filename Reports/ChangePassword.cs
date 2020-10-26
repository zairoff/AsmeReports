using System.Windows.Forms;

namespace Reports
{
    public partial class ChangePassword : Form
    {
        public ChangePassword()
        {
            InitializeComponent();
            _dataBase = new DataBase();
        }

        private readonly DataBase _dataBase;

        private void button2_Click(object sender, System.EventArgs e)
        {
            Hide();
            new Login().Show();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            if(string.IsNullOrEmpty(textBox1.Text) && string.IsNullOrEmpty(textBox2.Text) &&
                string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show(Properties.Resources.FILL_IN_FIELDS, "info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!_dataBase.checkRow("select 1 from login where pass = '" + textBox1.Text.Trim() + "'"))
            {
                textBox1.ForeColor = System.Drawing.Color.Red;
                MessageBox.Show(Properties.Resources.PASSWORD_WRONG, "info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!string.Equals(textBox2.Text.Trim(), textBox3.Text.Trim()))
            {
                textBox2.ForeColor = System.Drawing.Color.Red;
                textBox3.ForeColor = System.Drawing.Color.Red;
                MessageBox.Show(Properties.Resources.PASSWORD_NOT_MATCH, "info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _dataBase.insertData("update login set pass = '" + textBox3.Text + "' where pass = '" + textBox1.Text + "'");

            if(_dataBase.checkRow("select 1 from login where pass = '" + textBox3.Text.Trim() + "'"))
            {
                MessageBox.Show(Properties.Resources.PASSWORD_SUCCESS, "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            textBox1.ForeColor = System.Drawing.Color.Black;
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            textBox2.ForeColor = System.Drawing.Color.Black;
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            textBox3.ForeColor = System.Drawing.Color.Black;
        }
    }
}