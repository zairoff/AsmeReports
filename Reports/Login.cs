using System.Windows.Forms;

namespace Reports
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            textBox3.Select();
        }

        private void btn_connect_Click(object sender, System.EventArgs e)
        {
            MyDatabase myDatabase = new MyDatabase();
            if(string.IsNullOrEmpty(textBox2.Text) && string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("заполните все обязательные поля");
            }
            else
            {
                if (myDatabase.checkRow("select 1 from login where username = '" + textBox2.Text + "' and pass = '" +
                textBox3.Text + "'"))
                {
                    Hide();
                    new Form1().Show();                    
                }
                else
                {
                    textBox3.ForeColor = System.Drawing.Color.Red;
                    MessageBox.Show("неверный логин или пароль");                    
                }
            }            
        }

        private void connectDB()
        {
            MyDatabase myDatabase = new MyDatabase();
            if (string.IsNullOrEmpty(textBox2.Text) && string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("заполните все обязательные поля");
            }
            else
            {
                if (myDatabase.checkRow("select 1 from login where username = '" + textBox2.Text + "' and pass = '" +
                textBox3.Text + "'"))
                {
                    Hide();
                    new Form1().Show();
                }
                else
                {
                    textBox3.ForeColor = System.Drawing.Color.Red;
                    MessageBox.Show("неверный логин или пароль");
                }
            }
        }

        private void btn_close_Click(object sender, System.EventArgs e)
        {
            Application.Exit();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Hide();
            new ChangePassword().Show();            
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            textBox3.ForeColor = System.Drawing.Color.Black;
            if (e.KeyValue == (char)Keys.Enter)
            {
                connectDB();
            }
        }
    }
}
