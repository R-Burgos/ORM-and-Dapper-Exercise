using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM_Dapper
{
    public class DapperCategoriesRepository
    {
        private readonly IDbConnection _connection;
        //Constructor
        public DapperCategoriesRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public IEnumerable<Categories> GetAllCategories()
        {
            return _connection.Query<Categories>("SELECT * FROM Categories;");
        }


    }
}
