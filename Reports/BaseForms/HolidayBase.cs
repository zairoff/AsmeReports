using System;
using System.Windows.Forms;

namespace Reports.BaseForms
{
    public abstract partial class HolidayBase : Form
    {
        public HolidayBase()
        {
            InitializeComponent();           
        }

       
        public abstract void Setheaders();

        protected bool checkDate()
        {
            return dateTimePicker1.Value >= dateTimePicker2.Value;
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            Close();
        }        

        protected bool Check()
        {
            DialogResult dialogResult = MessageBox.Show("Index:" + dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value +
                Properties.Resources.DELETE,
                Properties.Resources.WARNING, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                return true;
            }
            else if (dialogResult == DialogResult.No)
            {
                return false;
            }
            return false;
        }        
    }
}
