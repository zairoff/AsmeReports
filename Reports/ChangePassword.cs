using System.Windows.Forms;

namespace Reports
{
    public partial class ChangePassword : Form
    {
        public ChangePassword()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            Hide();
            new Login().Show();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            MyDatabase myDatabase = new MyDatabase();
            if(string.IsNullOrEmpty(textBox1.Text) && string.IsNullOrEmpty(textBox2.Text) &&
                string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("заполните все обязательные поля");
                return;
            }
            if (!myDatabase.checkRow("select 1 from login where pass = '" + textBox1.Text + "'"))
            {
                textBox1.ForeColor = System.Drawing.Color.Red;
                MessageBox.Show("неверный пароль");                
                return;
            }
            if (!string.Equals(textBox2.Text, textBox3.Text))
            {
                textBox2.ForeColor = System.Drawing.Color.Red;
                textBox3.ForeColor = System.Drawing.Color.Red;
                MessageBox.Show("пароли не совпадают");                
                return;
            }
            myDatabase.insertData("update login set pass = '" + textBox3.Text + "' where pass = '" +
                textBox1.Text + "'");
            if(myDatabase.checkRow("select 1 from login where pass = '" + textBox3.Text + "'"))
            {
                MessageBox.Show("Успешно обновлено");
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

        private void label1_Click(object sender, System.EventArgs e)
        {

        }

        private void label2_Click(object sender, System.EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, System.EventArgs e)
        {

        }

        private void label3_Click(object sender, System.EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, System.EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, System.EventArgs e)
        {

        }
    }
}