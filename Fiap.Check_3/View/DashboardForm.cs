using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using Fiap.Check_3.Model;

namespace Fiap.Check_3.View
{
    public class DashboardForm : Form
    {
        private Label lblTotalUsers;
        private Label lblLastUser;
        private Label lblAvgAge;

        public DashboardForm()
        {
            Text = "Dashboard";
            Width = 400;
            Height = 250;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 4,
                ColumnCount = 1,
                Padding = new Padding(20),
                AutoSize = true
            };

            lblTotalUsers = new Label { Text = "Total de usuários: ...", AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 12) };
            lblLastUser = new Label { Text = "Usuário mais recente: ...", AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 12) };
            lblAvgAge = new Label { Text = "Média de idade: ...", AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 12) };

            layout.Controls.Add(lblTotalUsers, 0, 0);
            layout.Controls.Add(lblLastUser, 0, 1);
            layout.Controls.Add(lblAvgAge, 0, 2);

            Controls.Add(layout);

            Load += DashboardForm_Load;
        }

        private void DashboardForm_Load(object sender, EventArgs e)
        {
            try
            {
                using (var conn = DbConnection.GetConnection())
                {
                    // Total de usuários
                    using (var cmd = new OracleCommand("SELECT COUNT(*) FROM users", conn))
                    {
                        var total = Convert.ToInt32(cmd.ExecuteScalar());
                        lblTotalUsers.Text = $"Total de usuários: {total}";
                    }

                    // Usuário mais recente
                    using (var cmd = new OracleCommand("SELECT rm, data_nascimento FROM users ORDER BY rowid DESC FETCH FIRST 1 ROWS ONLY", conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var rm = reader.GetString(0);
                            var nascimento = reader.GetDateTime(1).ToShortDateString();
                            lblLastUser.Text = $"Usuário mais recente: RM {rm} (Nascimento: {nascimento})";
                        }
                        else
                        {
                            lblLastUser.Text = "Usuário mais recente: Nenhum usuário cadastrado";
                        }
                    }

                    // Média de idade
                    using (var cmd = new OracleCommand("SELECT data_nascimento FROM users", conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        var hoje = DateTime.Today;
                        int totalIdade = 0, count = 0;
                        while (reader.Read())
                        {
                            var nascimento = reader.GetDateTime(0);
                            int idade = hoje.Year - nascimento.Year;
                            if (nascimento.Date > hoje.AddYears(-idade)) idade--;
                            totalIdade += idade;
                            count++;
                        }
                        lblAvgAge.Text = count > 0
                            ? $"Média de idade: {totalIdade / count} anos"
                            : "Média de idade: N/A";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dashboard: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}