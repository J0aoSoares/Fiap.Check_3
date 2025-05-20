using System;
using System.Windows.Forms;
using Fiap.Check_3.Model;
using Fiap.Check_3.Controller;

namespace Fiap.Check_3.View
{
    public class ProductForm : Form
    {
        public ProductForm()
        {
            Text = "Adicionar Produto";
            Width = 350;
            Height = 250;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 4,
                ColumnCount = 2,
                Padding = new Padding(20),
                AutoSize = true
            };

            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));

            var lblName = new Label { Text = "Nome:", Anchor = AnchorStyles.Right, AutoSize = true };
            var txtName = new TextBox { Anchor = AnchorStyles.Left, Width = 150 };

            var lblPrice = new Label { Text = "Preço:", Anchor = AnchorStyles.Right, AutoSize = true };
            var txtPrice = new TextBox { Anchor = AnchorStyles.Left, Width = 150 };

            var lblQuantity = new Label { Text = "Quantidade:", Anchor = AnchorStyles.Right, AutoSize = true };
            var txtQuantity = new TextBox { Anchor = AnchorStyles.Left, Width = 150 };

            var btnSave = new Button { Text = "Salvar", Width = 100, Anchor = AnchorStyles.None };
            btnSave.Click += (s, e) =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(txtName.Text) ||
                        string.IsNullOrWhiteSpace(txtPrice.Text) ||
                        string.IsNullOrWhiteSpace(txtQuantity.Text))
                    {
                        MessageBox.Show("Preencha todos os campos.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var product = new Product
                    {
                        Name = txtName.Text,
                        Price = decimal.Parse(txtPrice.Text),
                        Quantity = int.Parse(txtQuantity.Text)
                    };
                    ProductController.AddProduct(product);
                    MessageBox.Show("Produto adicionado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao adicionar produto: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            layout.Controls.Add(lblName, 0, 0);
            layout.Controls.Add(txtName, 1, 0);
            layout.Controls.Add(lblPrice, 0, 1);
            layout.Controls.Add(txtPrice, 1, 1);
            layout.Controls.Add(lblQuantity, 0, 2);
            layout.Controls.Add(txtQuantity, 1, 2);
            layout.Controls.Add(btnSave, 0, 3);
            layout.SetColumnSpan(btnSave, 2);
            layout.SetCellPosition(btnSave, new TableLayoutPanelCellPosition(0, 3));

            Controls.Add(layout);
        }
    }
}
