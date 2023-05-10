using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProxyTestRm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        public void loadingAc()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
        }

        public void loadingKapat()
        {
            SplashScreenManager.CloseForm(false);
        }


        private void btnCalistir_Click(object sender, EventArgs e)
        {
            try
            {
                loadingAc();
                Api2 api2 = new Api2(txtIp.Text, txtPort.Text, txtUsername.Text, txtSifre.Text);
                richEditControl1.HtmlText = api2.requestGet(txtLink.Text);
            }
            catch (Exception ex)
            {
                richEditControl1.HtmlText = "ÇALIŞMADI!<br><br>" + ex.ToString();
            }

            loadingKapat();
        }

        private void btnTemizle_Click(object sender, EventArgs e)
        {
            txtIp.Text = "";
            txtPort.Text = "";
            txtUsername.Text = "";
            txtSifre.Text = "";
            richEditControl1.HtmlText = "";
            txtIp.Focus();
        }
    }
}
