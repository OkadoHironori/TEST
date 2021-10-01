using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace CT30K
{
	public partial class frmInputBox : Form
	{
		public frmInputBox()
		{
			InitializeComponent();
		}

		public static string InputBox(string prompt, string title = null)
		{
			frmInputBox dlg = new frmInputBox();

			dlg.lblPrompt.Text = prompt;

			dlg.Text = title != null? title : Process.GetCurrentProcess().MainWindowTitle;

			DialogResult result = dlg.ShowDialog();

			return result == DialogResult.OK? dlg.txtValue.Text : string.Empty;
		}
	}
}
