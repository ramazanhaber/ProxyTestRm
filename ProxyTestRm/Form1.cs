using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.Pdf.Native.BouncyCastle.Utilities.Net;

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

        private void btnToptanTest_Click(object sender, EventArgs e)
        {

            TestProxy(txtIp.Text, Convert.ToInt32(txtPort.Text));
        }
         void TestProxy(string ipAddress, int port)
        {
            try
            {
                string proxyAddress = "http://" + ipAddress + ":" + port;

                WebProxy proxy = new WebProxy(proxyAddress);


                // WebRequest oluşturun ve proxy'yi ayarlayın
                WebRequest request = WebRequest.Create(txtLink.Text);
                request.Proxy = proxy;

                // WebResponse alın
                WebResponse response = request.GetResponse();

                // Geri dönen veriyi işleyin (örneğin, ekrana yazdırabilirsiniz)
                using (var stream = response.GetResponseStream())
                using (var reader = new System.IO.StreamReader(stream))
                {
                    richEditControl1.HtmlText = reader.ReadToEnd();
                }

                // İşlem tamamlandıktan sonra response'ı kapatın
                response.Close();
            }
            catch (Exception ex)
            {
                richEditControl1.HtmlText = ex.Message+ "";

            }


            //string proxyAddress = "http://"+ ipAddress + ":"+ port;

            //HttpClientHandler handler = new HttpClientHandler
            //{
            //    Proxy = new WebProxy(proxyAddress),
            //    UseProxy = true
            //};

            //using (HttpClient client = new HttpClient(handler))
            //{
            //    try
            //    {
            //        HttpResponseMessage response = client.GetAsync(txtLink.Text).Result;
            //        string responseBody = response.Content.ReadAsStringAsync().Result;
            //        richEditControl1.HtmlText = responseBody;
            //    }
            //    catch (Exception ex)
            //    {
            //        richEditControl1.HtmlText = "ÇALIŞMADI!<br><br>" + ex.ToString();

            //    }

            //}
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;// güvenlikli demek
            ServicePointManager.Expect100Continue = true;

        }
    }
}
