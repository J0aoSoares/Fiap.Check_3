using System;
using System.Windows.Forms;

namespace Fiap.Check_3.View
{
    public class MainForm : Form
    {
        public MainForm()
        {
            Text = "Main Menu";
            Width = 300;
            Height = 250;

            var btnAdd = new Button { Text = "Adicionar Produtos", Left = 80, Top = 30, Width = 120 };
            var btnList = new Button { Text = "Lista de Produtos", Left = 80, Top = 70, Width = 120 };
            var btnTestConn = new Button { Text = "Testar Conexão", Left = 80, Top = 110, Width = 120 };
            var btnDashboard = new Button { Text = "Dashboard", Left = 80, Top = 150, Width = 120 };

            btnAdd.Click += (s, e) => new ProductForm().Show();
            btnList.Click += (s, e) => new ProductListForm().Show();
            btnTestConn.Click += BtnTestConn_Click;
            btnDashboard.Click += (s, e) => new DashboardForm().ShowDialog();

            Controls.Add(btnAdd);
            Controls.Add(btnList);
            Controls.Add(btnTestConn);
            Controls.Add(btnDashboard);
        }

        private void BtnTestConn_Click(object sender, EventArgs e)
        {
            try
            {
                using (var conn = Model.DbConnection.GetConnection())
                {
                    MessageBox.Show("Conexão com o banco realizada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao conectar: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
