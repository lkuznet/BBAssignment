using CatalogManager.Domain;
using CatalogManager.Domain.Aggregates;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Configuration;

namespace CatalogManager.Persistence.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        SqlConnection _sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
        public CategoryRepository()
        {
        }

        private SqlCommand CreateSqlCommand(string commandText)
        {
            var cm = new SqlCommand();
            cm.Connection = _sqlConnection;
            cm.CommandText = commandText;
            return cm;
        }
        private Category CreateCategory(SqlDataReader dr)
        {
            var category = new Category(dr["Name"].ToString());
            category.Id = int.Parse(dr["Id"].ToString());
            category.Path = dr["Path"].ToString();

            return category;
        }
        private Product CreateProduct(SqlDataReader dr)
        {
            var product = new Product(dr["Name"].ToString(), dr["Description"].ToString(),
                                    new Money(decimal.Parse(dr["Price_Amount"].ToString()),
                                    (Currency)Enum.Parse(typeof(Currency), dr["Price_Currency"].ToString())));
            product.Id = int.Parse(dr["Id"].ToString());

            return product;
        }
        private ICatalogItem CreateCatalogItem(SqlDataReader dr)
        {
            var item = new Product(dr["Name"].ToString(), string.Empty, new Money(0.00M, Currency.CAD));
            item.Id = int.Parse(dr["Id"].ToString());
            return item;
        }
        public ICatalogItem GetCatalog()
        {
            ICatalogItem catalog = null;
           
            SqlCommand cm = CreateSqlCommand(
                                    @"Select c.Id, c.Name, c.Path, c.ParentId,
                                     (Select COUNT(Id) as ProductCount 
                                            from Product p 
                                            where  p.Path  LIKE '%' + c.Name + '%' ) as ProductCount
                                 from Category c  Order By ParentId");
             _sqlConnection.Open();
            List<Category> list = new List<Category>();
            SqlDataReader dr = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

            while (dr.Read())
            {
                var category = CreateCategory(dr);
                 category.ProductCount = int.Parse(dr["ProductCount"].ToString());

                int parentId = 0;
                int.TryParse(dr["ParentId"].ToString(), out parentId);
                if (parentId == 0)
                    category.ParentId = null;
                else
                {
                    category.ParentId = parentId;
                }
                list.Add(category);
            }
            dr.Close();
            _sqlConnection.Close();

            foreach (var item in list)
            {
                if (item.Id == 1)
                {
                    catalog = item;
                    continue;
                }
                var parent = catalog.Find((int)item.ParentId);
                parent.Children.Add(item);
            }

            return catalog;
        }
        private Category GetCategoryWithChildren(string commandText)
        {
            Category category = null;
           SqlCommand cm = CreateSqlCommand(commandText
                );
            _sqlConnection.Open();
            SqlDataReader dr = cm.ExecuteReader();

            while (dr.Read())
            {
                category = CreateCategory(dr);
            }
            dr.Close();
            cm.CommandText = string.Format(@"Select c.Id, c.Name, c.ParentId, c.Path 
                                                from Category c  where c.ParentId = {0}", category.Id);
            SqlDataReader dr1 = cm.ExecuteReader();

            while (dr1.Read())
            {
                category.AddChild(CreateCategory(dr1));
            }
            dr1.Close();

            cm.CommandText = string.Format(@"Select c.Id, c.Name, c.ParentId, c.Path 
                                                from Product c  where c.ParentId = {0}", category.Id);
            SqlDataReader dr2 = cm.ExecuteReader();

            while (dr2.Read())
            {
                category.AddChild(CreateCategory(dr2));
            }
            dr2.Close();
            _sqlConnection.Close();
            return category;
        }
        public Category GetById(int id)
        {

            Category category = GetCategoryWithChildren(
                string.Format(@"Select c.Id, c.Name, c.ParentId, c.Path 
                                    from Category c  where c.Id = {0}", id)
                );
            return category;
        }
        public IEnumerable<ICatalogItem> GetProducts(int parentId)
        {
            List<Product> products = new List<Product>();
            SqlCommand cm = CreateSqlCommand(
            string.Format(@"Select p.Id, p.Name, p.Description, p.Price_Amount, p.Price_Currency 
                                    from Product p  
                                    join Category c on p.ParentId = c.Id 
                                    where p.ParentId = {0}", parentId));

            _sqlConnection.Open();
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                products.Add(CreateProduct(dr));
            }
            dr.Close();
            _sqlConnection.Close();
            return products;
        }
        public void Save(ICatalogItem catalogItem)
        {
            SqlCommand cm = null;
            if (catalogItem is Product)
            {
                var product = (Product)catalogItem;
                cm = CreateSqlCommand(string.Format(@"Insert into Product(Name, ParentId, Path, Price_Amount, Price_Currency, Description)
                        Values('{0}',{1},'{2}', {3}, '{4}', '{5}')", product.Name, product.Parent.Id, product.Path,
                            product.Price.Amount, product.Price.Currency.ToString(), product.Description));
            }
            else
            {
                cm= CreateSqlCommand( string.Format(@"Insert into Category(Name, ParentId, Path)
                        Values('{0}',{1},'{2}')", catalogItem.Name, catalogItem.Parent.Id, catalogItem.Path));

            }
            _sqlConnection.Open();

            var result = cm.ExecuteNonQuery();

            _sqlConnection.Close();
        }
        public void Update(ICatalogItem updatedItem)
        {
            SqlCommand cm = null;
            if (updatedItem is Product)
            {
                var product = (Product)updatedItem;
                cm= CreateSqlCommand(string.Format(@"Update Product Set Name='{0}', Description = '{1}', Price_Amount= {2}, Price_Currency = '{3}'
                                                where Id = {4}",
                          product.Name, product.Description, product.Price.Amount, product.Price.Currency.ToString(), product.Id));
            }
            else
            {
                cm= CreateSqlCommand(string.Format(@"Update Category set Name = '{0}'
                                                    where id = {1}", updatedItem.Name, updatedItem.Id));

            }
            _sqlConnection.Open();

            var result = cm.ExecuteNonQuery();

            _sqlConnection.Close();

        }
        public void Delete(ICatalogItem catalogItem)
        {
            SqlCommand cm = null;
            if (catalogItem is Product)
            {
                var product = (Product)catalogItem;
                 cm= CreateSqlCommand(string.Format(@"Delete from Product where Id = {0}", product.Id));
            }
            else
            {
                cm = CreateSqlCommand(string.Format(@"Delete from Category where id = {0}", catalogItem.Id));

            }
            _sqlConnection.Open();
            var result = cm.ExecuteNonQuery();
            _sqlConnection.Close();
        }
        public Category GetByName(string name)
        {
            Category category = null;
            SqlCommand cm = CreateSqlCommand(
            string.Format(@"Select c.Id, c.Name, c.ParentId, c.Path 
                                from Category c  where c.Name = '{0}'", name));

            _sqlConnection.Open();
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                category = CreateCategory(dr);
            }
            dr.Close();
            _sqlConnection.Close();
            return category;
        }
        public Category GetParent(int childId)
        {
            Category category = GetCategoryWithChildren(
            string.Format(@"Select cc.Id, cc.Name, cc.ParentId, cc.Path 
                    from Category c 
                    join Category cc on c.ParentId = cc.Id  
                    where c.Id = {0}", childId));
            return category;
        }
        public Product GetProductById(int childId)
        {
            Product product = null;
            SqlCommand cm = CreateSqlCommand(string.Format(@"Select p.Id, p.Name, p.ParentId, p.Path, 
                                            p.Description, p.Price_Amount, p.Price_Currency 
                                            from Product p  where p.Id = {0}", childId));
            _sqlConnection.Open();
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                product = CreateProduct(dr);
            }
            dr.Close();
            _sqlConnection.Close();
            return product;
        }
        public bool HasProducts(ICatalogItem catalogItem)
        {
            _sqlConnection.Open();
            SqlCommand cm = CreateSqlCommand(string.Format(@"Select (Select COUNT(Id) as ProductCount 
                                                    from Product p 
                                                    where  p.Path  LIKE '%' + c.Name +'%' ) as ProductCount
                                             from Category c 
                                             where c.Id = {0}", catalogItem.Id));

            var result = (int)cm.ExecuteScalar();
            _sqlConnection.Close();
            return result > 0;
        }
    }
}
