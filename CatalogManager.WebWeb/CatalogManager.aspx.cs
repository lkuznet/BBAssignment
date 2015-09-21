using CatalogManager.Application;
using CatalogManager.Application.Catalog;
using CatalogManager.Infractructure.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CatalogManager.WebWeb
{
    public partial class CatalogManager : System.Web.UI.Page
    {
        SqlConnection _sqlConnection;
        ICatalogApi _catalogApi;
        ILog _log;
        protected void Page_Load(object sender, EventArgs e)
        {
            _sqlConnection = new SqlConnection();
            _sqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            _log = new DefaultLogging();
            _catalogApi = new CatalogApiWithLogging(new CatalogApi(_sqlConnection), _log);

        }
         protected void btnAddCategory_Click(object sender, EventArgs e)
        {
            string name = txtNewCatName.Text.Trim();
            string parentName = txtCatParent.Text.Trim();
            int parentId = 0;
            int.TryParse(hidId.Value, out parentId);
            if (parentId > 0)
            {

                var result = _catalogApi.AddCategory(parentId, new CategoryDto { Name = name });
                if (result)
                {
                    lblMessage.Text = "Success!";
                    DisplayTree(_catalogApi);
                }
                else
                {
                    lblMessage.Text = "Item was not added. Check that name is unique among siblings.";
                }
            }
        }
        protected void btnAddProduct_Click(object sender, EventArgs e)
        {
            string name = txtNewCatName.Text.Trim();
            string parentName = txtCatParent.Text.Trim();
            int parentId = 0;
            int.TryParse(hidId.Value, out parentId);
            if (parentId > 0)
            {
                decimal price = 0;
                decimal.TryParse(txtProdPrice.Text.Trim(), out price);

                var result = _catalogApi.AddProduct(parentId, new ProductDto
                {
                    Name = txtProdName.Text.Trim(),
                    Description = txtProdDescr.Text.Trim(),
                    PriceAmount = price,
                    PriceCurrency = rbCurrency.SelectedValue
                });
                if (result)
                {
                    lblMessage.Text = "Success!";
                    DisplayTree(_catalogApi);
                }
                else
                {
                    lblMessage.Text = "Item was not added. Check that name is unique among siblings.";
                }
            }
        }
        private void DisplayTree(ICatalogApi catalogApi)
        {
            var result = catalogApi.GetCatalogTree(3);
            if (result == null)
                lblMessage.Text = "It's beeb an error.";
            lbTree.DataSource = result;
            lbTree.DataTextField = "Name";
            lbTree.DataValueField = "Id";
            lbTree.DataBind();
        }


        protected void btnDisplayTree_Click(object sender, EventArgs e)
        {
            var result = _catalogApi.GetCatalogTree(3);
            lbTree.DataSource = result;
            lbTree.DataTextField = "Name";
            lbTree.DataValueField = "Id";
            lbTree.DataBind();

        }
        protected void btnUpdateCategory_Click(object sender, EventArgs e)
        {
            string name = txtCatName.Text.Trim();
            int id = 0;
            int.TryParse(hidId.Value, out id);
            if (id > 0)
            {
                var catalogApi = new CatalogApi(_sqlConnection);
                bool result = catalogApi.UpdateCategory(id, new CategoryDto { Name = name });
                if (result)
                {
                    lblMessage.Text = "Success!";
                    DisplayTree(catalogApi);
                }
                else
                {
                    lblMessage.Text = "Item was not updated. Check that name is unique among siblings.";
                }
            }
        }
        protected void lbTree_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbTree.SelectedIndex >= 0)
            {
                int id = 0;
                int.TryParse(lbTree.SelectedValue, out id);
                if (id > 0)
                {
                    DisplayProducts(id);

                    string[] parentNames = lbTree.SelectedItem.Text.Split("-".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    string[] parentNames1 = parentNames[1].Split("(".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    string parentName = parentNames1[0];
                    txtCatParent.Text = parentName;
                    txtProdParent.Text = parentName;
                    txtCatName.Text = parentName;
                    hidId.Value = id.ToString();

                    btnDeleteProduct.Enabled = false;
                    btnUpdateProduct.Enabled = false;
                    btnUpdateCategory.Enabled = true;
                    btnAddCategory.Enabled = true; ;
                    btnDeleteCategory.Enabled = true;
                    btnAddProduct.Enabled = true;
                }
            }
        }

        private void DisplayProducts(int id)
        {
            var result = _catalogApi.GetProducts(id);
            lbChildren.DataSource = result;
            lbChildren.DataTextField = "Name";
            lbChildren.DataValueField = "Id";
            lbChildren.DataBind();
        }
        protected void lbChildren_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbChildren.SelectedIndex >= 0)
            {
                int id = 0;
                int.TryParse(lbChildren.SelectedValue, out id);
                if (id > 0)
                {
                    var catalogApi = new CatalogApi(_sqlConnection);
                    ProductDto product = catalogApi.GetProduct(id);

                    txtProdName.Text = product.Name;
                    txtProdDescr.Text = product.Description;
                    txtProdPrice.Text = product.PriceAmount.ToString();
                    rbCurrency.SelectedIndex = rbCurrency.Items.IndexOf(rbCurrency.Items.FindByText(product.PriceCurrency));
                    hidProductId.Value = product.Id.ToString();

                    btnUpdateProduct.Enabled = true;
                    btnDeleteProduct.Enabled = true;
                    btnUpdateCategory.Enabled = false;
                    btnAddCategory.Enabled = false;
                    btnDeleteCategory.Enabled = false;
                    btnAddProduct.Enabled = false;
                }
            }
        }
        protected void btnUpdateProduct_Click(object sender, EventArgs e)
        {
            int categoryId = 0;
            int.TryParse(hidId.Value, out categoryId);
           
            int productId = 0;
            int.TryParse(hidProductId.Value, out productId);
            if (productId > 0 && categoryId > 0)
            {
                decimal price = 0;
                decimal.TryParse(txtProdPrice.Text.Trim(), out price);

                var result = _catalogApi.UpdateProduct(categoryId, productId, new ProductDto
                {
                    Name = txtProdName.Text.Trim(),
                    Description = txtProdDescr.Text.Trim(),
                    PriceAmount = price,
                    PriceCurrency = rbCurrency.SelectedValue
                });
                if (result)
                {
                    lblMessage.Text = "Success!";
                    DisplayProducts(categoryId);
                }
                else
                {
                    lblMessage.Text = "Item was not updated. Check that name is unique among siblings.";
                }
            }
        }
        protected void btnDeleteCategory_Click(object sender, EventArgs e)
        {
                       int categoryId = 0;
            int.TryParse(hidId.Value, out categoryId);
            if (categoryId > 0)
            {
                var catalogApi = new CatalogApi(_sqlConnection);
               bool  result = catalogApi.RemoveCategory(categoryId);
                 if (result)
                 {
                     lblMessage.Text = "Success!";
                     DisplayTree(catalogApi);
                 }
                 else
                 {
                     lblMessage.Text = "Item was not deleted. This category branch has products.";
                 }
            }
        }

        protected void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            int productId = 0;
            int categoryId = 0;
            int.TryParse(hidId.Value, out categoryId);
            int.TryParse(hidProductId.Value, out productId);
            if (productId > 0 && categoryId > 0)
            {
               bool result = _catalogApi.RemoveProduct(categoryId, productId);
                if (result)
                {
                    lblMessage.Text = "Success!";
                    DisplayProducts(categoryId);
                }
                else
                {
                    lblMessage.Text = "Item was not deleted. This category branch has products.";
                }
            }
        }
    }
}