using System.Windows.Forms;
using Fiap.Check_3.Controller;

namespace Fiap.Check_3.View
{
    public class ProductListForm : Form
    {
        public ProductListForm()
        {
            Text = "Lista de Produtos";
            Width = 600;
            Height = 350;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            var grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Margin = new Padding(10)
            };

            grid.DataSource = ProductController.GetAllProducts();

            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };
            panel.Controls.Add(grid);

            Controls.Add(panel);
        }
    }
}
