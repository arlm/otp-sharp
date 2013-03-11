using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OtpSharp;
using System.Web;

namespace GoogleAuthenticatorTotpTest
{
    public partial class GoogleAuthenticatorTotpTest : Form
    {
        static readonly byte[] rfcKey = new byte[] { 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x30 };
        /// <summary>
        /// The test key provided in the RFC
        /// </summary>
        Totp totp = new Totp(rfcKey);

        public GoogleAuthenticatorTotpTest()
        {
            InitializeComponent();
            this.UpdateCode();
        }

        private void ResetTotp()
        {
            this.totp = new Totp(rfcKey, this.stepSize, totpSize: this.digits);
            var name = this.textBoxKeyLabel.Text;
            if (string.IsNullOrWhiteSpace(name))
                name = "OtpSharp@test.com";

            string url = KeyUrl.GetTotpUrl(rfcKey, name, step: this.stepSize, totpSize: this.digits);
            this.pictureBox1.ImageLocation = string.Format("http://qrcode.kaywa.com/img.php?s=4&d={0}", HttpUtility.UrlEncode(url));
        }

        private void UpdateCode()
        {
            this.labelTotp.Text = totp.ComputeTotp();
            this.labelRemaining.Text = totp.RemainingSeconds().ToString();
        }

        private void timerUpdate_Tick(object sender, EventArgs e)
        {
            this.UpdateCode();
        }

        private void buttonVerify_Click(object sender, EventArgs e)
        {
            Verify();
        }

        private void textBoxValue_TextChanged(object sender, EventArgs e)
        {
            this.tabPage2.BackColor = Color.White;
        }

        private void textBoxValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.Verify();
        }

        private void Verify()
        {
            var text = this.textBoxValue.Text;
            this.textBoxValue.Text = string.Empty;


            long windowUsed;
            bool result = totp.VerifyTotp(text, out windowUsed, new VerificationWindow(1, 0));
            if (result)
                this.tabPage2.BackColor = Color.Green;
            else
                this.tabPage2.BackColor = Color.Red;
        }

        private int stepSize
        {
            get
            {
                int val;
                if (int.TryParse(this.textBoxPeriod.Text, out val))
                {
                    if (val <= 0)
                    {
                        this.textBoxPeriod.Text = "30";
                        val = 30;
                    }
                }
                else
                {
                    this.textBoxPeriod.Text = "30";
                    val = 30;
                }

                return val;
            }
        }

        private int digits
        {
            get
            {
                if (this.radioButtonSix.Checked)
                    return 6;
                else if (this.radioButtonEight.Checked)
                    return 8;
                else
                    return 6;
            }
        }

        private void textBoxKeyLabel_TextChanged(object sender, EventArgs e)
        {
            this.ResetTotp();
        }

        private void textBoxPeriod_TextChanged(object sender, EventArgs e)
        {
            this.ResetTotp();
        }

        private void radioButtonSix_CheckedChanged(object sender, EventArgs e)
        {
            this.ResetTotp();
        }

        private void radioButtonEight_CheckedChanged(object sender, EventArgs e)
        {
            this.ResetTotp();
        }
    }
}
