using System.Windows.Forms;

namespace Reports.MyControls
{
    public partial class Employee : UserControl
    {
        public Employee()
        {
            InitializeComponent();
        }
        //private string _otdel;
        //private string _doljnost;
        //private System.Drawing.Image _myimage;

        [System.ComponentModel.Category("Custom Props")]
        public string UserID { get; set; }

        [System.ComponentModel.Category("Custom Props")]
        public System.Drawing.Image MyImage
        {
            get { return pictureBox1.Image; }
            set { pictureBox1.Image = value; }
        }

        [System.ComponentModel.Category("Custom Props")]
        public string FIO
        {
            get { return label4.Text; }
            set { label4.Text = value; }
        }

        [System.ComponentModel.Category("Custom Props")]
        public string Otdel
        {
            get { return label5.Text; }
            set { label5.Text = value; }
        }

        [System.ComponentModel.Category("Custom Props")]
        public string Lavozim
        {
            get { return label6.Text; }
            set { label6.Text = value; }
        }

     

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new Otpusk(System.Convert.ToInt32(UserID)).ShowDialog();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new Uvolnitelniy(System.Convert.ToInt32(UserID)).ShowDialog();
        }
    }
}
