using System;
using System.Drawing;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using Fiap.Check_3.Model;

namespace Fiap.Check_3.View
{
    public class LoginForm : Form
    {
        public LoginForm()
        {
            Text = "Login";
            Width = 300;
            Height = 180;
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 3,
                ColumnCount = 2,
                Padding = new Padding(20),
                AutoSize = true
            };

            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));

            var lblUsername = new Label { Text = "Usuário:", Anchor = AnchorStyles.Right, AutoSize = true };
            var txtUsername = new TextBox { Anchor = AnchorStyles.Left, Width = 120 };

            var lblPass = new Label { Text = "Senha:", Anchor = AnchorStyles.Right, AutoSize = true };
            var txtPass = new TextBox { Anchor = AnchorStyles.Left, Width = 120, UseSystemPasswordChar = true };

            var btnLogin = new Button { Text = "Entrar", Width = 90, Anchor = AnchorStyles.None };
            var btnRegister = new Button { Text = "Registrar", Width = 90, Anchor = AnchorStyles.None };

            btnLogin.Click += (s, e) =>
            {
                try
                {
                    using (var conn = DbConnection.GetConnection())
                    {
                        conn.Open(); // Importante: garante que a conexão foi aberta

                        string query = "SELECT COUNT(*) FROM APP_USERS WHERE USERNAME = :username AND PASSWORD = :password";
                        using (var cmd = new OracleCommand(query, conn))
                        {
                            cmd.Parameters.Add(new OracleParameter("username", txtUsername.Text));
                            cmd.Parameters.Add(new OracleParameter("password", txtPass.Text));

                            int count = Convert.ToInt32(cmd.ExecuteScalar());

                            if (count > 0)
                            {
                                MessageBox.Show("Login realizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                DialogResult = DialogResult.OK;
                                Close();
                            }
                            else
                            {
                                MessageBox.Show("Usuário ou senha inválidos.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao fazer login: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnRegister.Click += (s, e) =>
            {
                var regForm = new RegisterForm();
                regForm.ShowDialog();
            };

            layout.Controls.Add(lblUsername, 0, 0);
            layout.Controls.Add(txtUsername, 1, 0);
            layout.Controls.Add(lblPass, 0, 1);
            layout.Controls.Add(txtPass, 1, 1);
            layout.Controls.Add(btnLogin, 0, 2);
            layout.Controls.Add(btnRegister, 1, 2);

            Controls.Add(layout);
        }
    }
}
