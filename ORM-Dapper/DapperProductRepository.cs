using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM_Dapper
{
    public class DapperProductRepository : IProductRepository
    {
        private readonly IDbConnection _connection;
        //Constructor
        public DapperProductRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public void CreateProduct(string name, double price, int categoryID, int stock)
        {
            _connection.Execute("INSERT INTO PRODUCTS (Name, Price, CategoryID, StockLevel) VALUES (@productName, @productPrice, @productCategoryID, @stock);",
             new { productName = name, productPrice = price, productCategoryID = categoryID, stock = stock });

        }

        public void DeleteProduct(int id)
        {
            _connection.Execute("DELETE FROM SALES WHERE ProductID = @id;", new { id = id});
            _connection.Execute("DELETE FROM REVIEWS WHERE ProductID = @id;", new { id = id });
            _connection.Execute("DELETE FROM PRODUCTS WHERE ProductID = @id;", new { id = id });

        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _connection.Query<Product>("SELECT * FROM Products;").ToList();
        }

        public Product GetProductById(int id)
        {
            return _connection.QuerySingle<Product>("SELECT * FROM Products WHERE ProductID = @id;" , new { id = id });
        }

        public void UpdateProduct(Product product)
        {
            _connection.Execute("UPDATE products SET Name = @name, Price = @price, CategoryID = @catID, OnSale = @onSale, StockLevel = @stock WHERE ProductID = @id;",
            new { 
                name = product.Name, 
                price = product.Price, 
                catID = product.CategoryID, 
                onSale = product.OnSale, 
                stock = product.StockLevel,
                id = product.ProductID
            });
        }
    }
}
