using System;
using System.Windows.Forms;
using Fiap.Check_3.Model;
using Oracle.ManagedDataAccess.Client;

namespace Fiap.Check_3.View
{
    public class RegisterForm : Form
    {
        public RegisterForm()
        {
            Text = "Registrar Usuário";
            Width = 350;
            Height = 250;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 5,
                ColumnCount = 2,
                Padding = new Padding(20),
                AutoSize = true
            };

            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));

            var lblRM = new Label { Text = "RM:", Anchor = AnchorStyles.Right, AutoSize = true };
            var txtRM = new TextBox { Anchor = AnchorStyles.Left, Width = 120 };

            var lblPass = new Label { Text = "Senha:", Anchor = AnchorStyles.Right, AutoSize = true };
            var txtPass = new TextBox { Anchor = AnchorStyles.Left, Width = 120, UseSystemPasswordChar = true };

            var lblNascimento = new Label { Text = "Data de Nascimento:", Anchor = AnchorStyles.Right, AutoSize = true };
            var dtNascimento = new DateTimePicker { Anchor = AnchorStyles.Left, Format = DateTimePickerFormat.Short, Width = 120, MaxDate = DateTime.Today };

            var btnRegister = new Button { Text = "Registrar", Width = 100, Anchor = AnchorStyles.None };
            btnRegister.Click += (s, e) =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(txtRM.Text) || string.IsNullOrWhiteSpace(txtPass.Text))
                    {
                        MessageBox.Show("Preencha todos os campos.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Verifica se é maior de idade
                    var hoje = DateTime.Today;
                    var idade = hoje.Year - dtNascimento.Value.Year;
                    if (dtNascimento.Value.Date > hoje.AddYears(-idade)) idade--;

                    if (idade < 18)
                    {
                        MessageBox.Show("Apenas maiores de 18 anos podem se registrar. Acesso negado.", "Acesso Negado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    using (var conn = DbConnection.GetConnection())
                    {
                        string query = "INSERT INTO users (rm, password, data_nascimento) VALUES (:rm, :password, :data_nascimento)";
                        using (var cmd = new OracleCommand(query, conn))
                        {
                            cmd.Parameters.Add(new OracleParameter("rm", txtRM.Text));
                            cmd.Parameters.Add(new OracleParameter("password", txtPass.Text));
                            cmd.Parameters.Add(new OracleParameter("data_nascimento", dtNascimento.Value));
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Usuário registrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Close();
                        }
                    }
                }
                catch (OracleException ex) when (ex.Number == 1)
                {
                    MessageBox.Show("RM já cadastrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao registrar: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            layout.Controls.Add(lblRM, 0, 0);
            layout.Controls.Add(txtRM, 1, 0);
            layout.Controls.Add(lblPass, 0, 1);
            layout.Controls.Add(txtPass, 1, 1);
            layout.Controls.Add(lblNascimento, 0, 2);
            layout.Controls.Add(dtNascimento, 1, 2);
            layout.Controls.Add(btnRegister, 0, 3);
            layout.SetColumnSpan(btnRegister, 2);

            Controls.Add(layout);
        }
    }
}