using CatalogManager.Domain;
using CatalogManager.Domain.Aggregates;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace CatalogManager.Persistence.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private SqlConnection _sqlConnection;
        public CategoryRepository()
        {
        }

        public CategoryRepository(SqlConnection sqlConnection)
        {
            this._sqlConnection = sqlConnection;
        }
        public ICatalogItem GetCatalog()
        {
            ICatalogItem catalog = GetById(1);
            _sqlConnection.Open();
            SqlCommand cm = new SqlCommand();
            cm.Connection = _sqlConnection;

            cm.CommandText = @"Select c.Id, c.Name, c.Path, c.ParentId,
                                (Select COUNT(Id) as ProductCount from Product p where  p.Path  LIKE '%' + c.Name + '%' ) as ProductCount
                                 from Category c where c.Id > 1 Order By ParentId";
            SqlDataReader dr = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            List<Category> result = new List<Category>();

            while (dr.Read())
            {
                int parentId = 0;
                int.TryParse(dr["ParentId"].ToString(), out parentId);
                var category = new Category(dr["Name"].ToString(), int.Parse(dr["Id"].ToString()), dr["Path"].ToString());
                category.ProductCount = int.Parse(dr["ProductCount"].ToString());
                var parent = catalog.Find(parentId);
                if (parent != null)
                {
                    parent.AddChild(category);
                }
            }
            dr.Close();
            _sqlConnection.Close();
            return catalog;
        }
        public Category GetById(int id)
        {
            Category category = null;
            _sqlConnection.Open();
            SqlCommand cm = new SqlCommand();
            cm.Connection = _sqlConnection;
            cm.CommandText = string.Format(@"Select c.Id, c.Name, c.ParentId, c.Path from Category c  where c.Id = {0}", id);
            SqlDataReader dr = cm.ExecuteReader();

            while (dr.Read())
            {
                category = new Category(dr["Name"].ToString(), int.Parse(dr["Id"].ToString()), dr["Path"].ToString());
            }
            dr.Close();
            cm.CommandText = string.Format(@"Select c.Id, c.Name, c.ParentId, c.Path from Category c  where c.ParentId = {0}", id);
            SqlDataReader dr1 = cm.ExecuteReader();

            while (dr1.Read())
            {
                category.AddChild(new Category(dr1["Name"].ToString(), int.Parse(dr1["Id"].ToString()), dr1["Path"].ToString()));
            }
            dr1.Close();
            cm.CommandText = string.Format(@"Select c.Id, c.Name, c.ParentId, c.Path from Product c  where c.ParentId = {0}", id);
            SqlDataReader dr2 = cm.ExecuteReader();

            while (dr2.Read())
            {
                var product = new Product(dr2["Name"].ToString(), int.Parse(dr2["Id"].ToString()));
                category.AddChild(product);
            }
            dr2.Close();
            _sqlConnection.Close();
            return category;
        }
        //public IEnumerable<ICatalogItem> GetChildren(int parentId)
        //{
        //    List<ICatalogItem> result = new List<ICatalogItem>();
        //    _sqlConnection.Open();
        //    SqlCommand cm = new SqlCommand();
        //    cm.Connection = _sqlConnection;
        //    cm.CommandText = string.Format(@"Select c.Id, c.Name, c.ParentId, c.Path from Category c  where c.ParentId = {0}", parentId);
        //    SqlDataReader dr = cm.ExecuteReader();

        //    while (dr.Read())
        //    {
        //        result.Add(new Category(dr["Name"].ToString(), int.Parse(dr["Id"].ToString()), dr["Path"].ToString()));
        //    }
        //    dr.Close();
        //    cm.CommandText = string.Format(@"Select p.Id, p.Name, p.ParentId, p.Path, p.Description, p.Price_Amount, p.Price_Currency from Product p  where p.ParentId = {0}", parentId);
        //    SqlDataReader dr2 = cm.ExecuteReader();
        //    while (dr2.Read())
        //    {
        //        result.Add(new Product(dr2["Name"].ToString(), dr2["Description"].ToString(),
        //             new Money(decimal.Parse(dr2["Price_Amount"].ToString()), (Currency)Enum.Parse(typeof(Currency), dr2["Price_Currency"].ToString())), int.Parse(dr2["Id"].ToString())));
        //    }
        //    dr2.Close();
        //    _sqlConnection.Close();
        //    return result;
        //}
        public IEnumerable<ICatalogItem> GetProducts(int parentId)
        {
            List<Product> products = new List<Product>();
            _sqlConnection.Open();
            SqlCommand cm = new SqlCommand();
            cm.Connection = _sqlConnection;
            cm.CommandText = string.Format(@"Select p.Id, p.Name, p.Description, p.Price_Amount, p.Price_Currency from Product p  join Category c on p.ParentId = c.Id where p.ParentId = {0}", parentId);
            SqlDataReader dr = cm.ExecuteReader();

            while (dr.Read())
            {
                products.Add(new Product(dr["Name"].ToString(), dr["Description"].ToString(),
                    new Money(decimal.Parse(dr["Price_Amount"].ToString()), (Currency)Enum.Parse(typeof(Currency), dr["Price_Currency"].ToString())), int.Parse(dr["Id"].ToString())));
            }
            dr.Close();
            _sqlConnection.Close();
            return products;
        }
        public void Save(ICatalogItem catalogItem)
        {
            _sqlConnection.Open();
            SqlCommand cm = new SqlCommand();
            cm.Connection = _sqlConnection;
            if (catalogItem is Product)
            {
                var product = (Product)catalogItem;
                cm.CommandText = string.Format(@"Insert into Product(Name, ParentId, Path, Price_Amount, Price_Currency, Description)
                        Values('{0}',{1},'{2}', {3}, '{4}', '{5}')", product.Name, product.Parent.Id, product.Path,
                            product.Price.Amount, product.Price.Currency.ToString(), product.Description);
            }
            else
            {
                cm.CommandText = string.Format(@"Insert into Category(Name, ParentId, Path)
                        Values('{0}',{1},'{2}')", catalogItem.Name, catalogItem.Parent.Id, catalogItem.Path);

            }
            var result = cm.ExecuteNonQuery();

            _sqlConnection.Close();
        }
        public void Update(ICatalogItem updatedItem)
        {
            _sqlConnection.Open();
            SqlCommand cm = new SqlCommand();
            cm.Connection = _sqlConnection;
            if (updatedItem is Product)
            {
                var product = (Product)updatedItem;
                cm.CommandText = string.Format(@"Update Product Set Name='{0}', Description = '{1}', Price_Amount= {2}, Price_Currency = '{3}'
                                                where Id = {4}",
                          product.Name, product.Description, product.Price.Amount, product.Price.Currency.ToString(), product.Id);
            }
            else
            {
                cm.CommandText = string.Format(@"Update Category set Name = '{0}' where id = {1}", updatedItem.Name, updatedItem.Id);

            }
            var result = cm.ExecuteNonQuery();

            _sqlConnection.Close();

        }
        public void Delete(ICatalogItem catalogItem)
        {
            _sqlConnection.Open();
            SqlCommand cm = new SqlCommand();
            cm.Connection = _sqlConnection;
            if (catalogItem is Product)
            {
                var product = (Product)catalogItem;
                cm.CommandText = string.Format(@"Delete from Product where Id = {0}", product.Id);
            }
            else
            {
                cm.CommandText = string.Format(@"Delete from Category where id = {0}", catalogItem.Id);

            }
            var result = cm.ExecuteNonQuery();

            _sqlConnection.Close();
        }
        public Category GetByName(string name)
        {
            Category category = null;
            _sqlConnection.Open();
            SqlCommand cm = new SqlCommand();
            cm.Connection = _sqlConnection;
            cm.CommandText = string.Format(@"Select c.Id, c.Name, c.ParentId, c.Path from Category c  where c.Name = '{0}'", name);
            SqlDataReader dr = cm.ExecuteReader();

            while (dr.Read())
            {
                category = new Category(dr["Name"].ToString(), int.Parse(dr["Id"].ToString()), dr["Path"].ToString());
                category.Id = int.Parse(dr["Id"].ToString());

            }
            dr.Close();
            _sqlConnection.Close();
            return category;
        }
        public Category GetParent(int childId)
        {
            Category category = null;
            _sqlConnection.Open();
            SqlCommand cm = new SqlCommand();
            cm.Connection = _sqlConnection;
            cm.CommandText = string.Format(@"Select cc.Id, cc.Name, cc.ParentId, cc.Path from Category c join Category cc on c.ParentId = cc.Id  where c.Id = {0}", childId);
            SqlDataReader dr = cm.ExecuteReader();

            while (dr.Read())
            {
                category = new Category(dr["Name"].ToString(), int.Parse(dr["Id"].ToString()), dr["Path"].ToString());

            }
            dr.Close();

            cm.CommandText = string.Format(@"Select c.Id, c.Name, c.ParentId, c.Path from Category c  where c.ParentId = {0}", category.Id);
            SqlDataReader dr1 = cm.ExecuteReader();

            while (dr1.Read())
            {
                category.AddChild(new Category(dr1["Name"].ToString(), int.Parse(dr1["Id"].ToString()), dr1["Path"].ToString()));
            }
            dr1.Close();

            cm.CommandText = string.Format(@"Select c.Id, c.Name, c.ParentId, c.Path from Product c  where c.ParentId = {0}", category.Id);
            SqlDataReader dr2 = cm.ExecuteReader();

            while (dr2.Read())
            {
                var product = new Product(dr2["Name"].ToString(), int.Parse(dr2["Id"].ToString()));
                category.AddChild(product);
            }
            dr2.Close();
            _sqlConnection.Close();
            return category;
        }
        public Product GetProductById(int childId)
        {
            Product product = null;
            _sqlConnection.Open();
            SqlCommand cm = new SqlCommand();
            cm.Connection = _sqlConnection;
            cm.CommandText = string.Format(@"Select p.Id, p.Name, p.ParentId, p.Path, p.Description, p.Price_Amount, p.Price_Currency from Product p  where p.Id = {0}", childId);
            SqlDataReader dr = cm.ExecuteReader();

            while (dr.Read())
            {
                product = new Product(dr["Name"].ToString(), dr["Description"].ToString(),
                        new Money(decimal.Parse(dr["Price_Amount"].ToString()), (Currency)Enum.Parse(typeof(Currency), dr["Price_Currency"].ToString())));
                product.Id = int.Parse(dr["Id"].ToString());

            }
            dr.Close();
            _sqlConnection.Close();
            return product;
        }
        public bool HasProducts(ICatalogItem catalogItem)
        {
            _sqlConnection.Open();
            SqlCommand cm = new SqlCommand();
            cm.Connection = _sqlConnection;

            cm.CommandText = string.Format(@"Select (Select COUNT(Id) as ProductCount from Product p where  p.Path  LIKE '%' + c.Name +'%' ) as ProductCount
                                 from Category c where c.Id = {0}", catalogItem.Id);

            var result = (int)cm.ExecuteScalar();
            _sqlConnection.Close();
            return result > 0;
        }
    }
}
