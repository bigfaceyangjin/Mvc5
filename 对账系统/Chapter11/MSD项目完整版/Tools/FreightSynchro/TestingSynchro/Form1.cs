using FreightSynchro.Entitys;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestingSynchro
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtUrl.Text) || string.IsNullOrEmpty(this.txtExpressNo.Text))
                    return;
                this.richTextBox1.Text = WayBillInCome.GetFeeTesting(this.txtUrl.Text, this.txtExpressNo.Text, this.txtExpress.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
