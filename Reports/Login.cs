using System;
using System.Windows.Forms;

namespace Reports
{
    public partial class Login : Form
    {
        public Login()
        {
            _dataBase = new DataBase();
            SetLanguage();
            InitializeComponent();
            textBox3.Select();            
        }

        private readonly DataBase _dataBase;

        private void SetLanguage()
        {
            try
            {
                var language = _dataBase.GetString("select lan from language limit 1");
                System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo(language);
            }
            catch (Exception msg)
            {
                MessageBox.Show(msg.ToString(), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_connect_Click(object sender, System.EventArgs e)
        {
            ConnectDB();
        }

        private void ConnectDB()
        {
            try
            {
                if (string.IsNullOrEmpty(textBox2.Text) && string.IsNullOrEmpty(textBox3.Text))
                {
                    MessageBox.Show(Properties.Resources.FILL_IN_FIELDS, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (_dataBase.checkRow("select 1 from login where username = '" + textBox2.Text + "' and pass = '" +
                    textBox3.Text + "'"))
                    {
                        Hide();
                        new Form1().Show();
                    }
                    else
                    {
                        textBox3.ForeColor = System.Drawing.Color.Red;
                        MessageBox.Show(Properties.Resources.USERNAME_OR_PASSWORD_WRONG, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception msg)
            {
                MessageBox.Show(msg.ToString(), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                ConnectDB();
            }
        }
    }
}
