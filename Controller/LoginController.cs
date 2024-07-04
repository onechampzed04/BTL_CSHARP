using BTL_2.Model;
using BTL_2.Shareds;
using BTL_2.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTL_2.Controller
{
    public class LoginController
    {
        //private DatabaseDataContext dataClassesDataContext;
        public TextBox txtUsername { get; set; }
        public TextBox txtPassword { get; set; }
        public Label Msg { get; set; }
        public Button btnLogin { get; set; }

        public LoginForm login { get; set; }

        public LoginController(TextBox txtUsername, TextBox txtPassword, Label msg , Button btnLogin, LoginForm login)
        {
            this.txtUsername = txtUsername;
            this.txtPassword = txtPassword;
            Msg = msg;
            this.btnLogin = btnLogin;
            //dataClassesDataContext = new DatabaseDataContext();
            this.login = login;

            //setEvent();  // Đăng ký sự kiện tại đây
        }

        public void CheckLogin()
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                FuncResult<List<User>> rs = FuncShares<User>.GetAllData();
                switch(rs.ErrorCode)
                {
                    case EnumErrorCode.ERROR:
                        MessageBox.Show(rs.ErrorDesc, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case EnumErrorCode.SUCCESS:
                        var tk = rs.Data
                        .Where(u => u.Username == username && u.PasswordHash == password)
                        .FirstOrDefault();
                        if (tk != null)
                        {
                            Constant.User = tk;
                            Msg.Visible = false;
                            login.Close();
                        }
                        else
                        {
                            Msg.Text = Constants.invalidAccount;
                            Msg.Visible = true;
                        }
                        break;
                    case EnumErrorCode.FAILED: 
                        break;
                }
                
            }
            else
            {
                Msg.Text = Constants.emptyAccount;
                Msg.Visible = true;
            }
        }

        public void setEvent()
        {
            btnLogin.Click += new System.EventHandler((object sender, EventArgs e) =>
            {
                CheckLogin();
            });
            //login.FormClosing += Login_FormClosing;
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            //hien thi lai MainForm
        }
    }
}
