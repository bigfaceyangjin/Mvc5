using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Eonup.RemoteService
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void btn_open_Click(object sender, EventArgs e)
		{
			try
			{
				this.btn_close.Enabled = true;
				this.btn_open.Enabled = false;
				WcfInit.OpenService();
			}
			catch (Exception ex)
			{
				this.btn_close.Enabled = true;
				this.btn_open.Enabled = false;
				Console.WriteLine($"{DateTime.Now}");
			}
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			this.btn_close.Enabled = false;
			this.btn_open.Enabled = true;
		}

		private void btn_close_Click(object sender, EventArgs e)
		{
			this.btn_close.Enabled = false;
			this.btn_open.Enabled = true;
		}
	}
}
