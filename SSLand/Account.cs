using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SSLand
{
    public partial class Account : Form
    {
        public Account()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            var api = new SecondStreetAPI();
            bool loginres = api.trySecondStreetLogin(emailTextBox.Text.Trim(), passwordTextBox.Text.Trim());
            if (loginres) {
                var settingsDBHelper = new SettingsDBHelper();
                settingsDBHelper.updateSettings("email", this.emailTextBox.Text.Trim());
                settingsDBHelper.updateSettings("password", this.passwordTextBox.Text.Trim());
                MessageBox.Show("ログインに成功しました", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else {
                MessageBox.Show("ログインに失敗しました", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Account_Load(object sender, EventArgs e) {
            var settingsDBHelper = new SettingsDBHelper();
            this.emailTextBox.Text = settingsDBHelper.getSettingValue("email");
            this.passwordTextBox.Text = settingsDBHelper.getSettingValue("password");
        }
    }
}
