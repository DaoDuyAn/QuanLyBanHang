using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using SV20T1020293.DomainModels;

namespace SV20T1020293.DataLayers.SQLServer
{
    public class CustomerDAL : _BaseDAL, ICommonDAL<Customer>
    {
        public CustomerDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Customer data)
        {
            throw new NotImplementedException();
        }

        public int Count(string searchValue = "")
        {
            int count = 0;

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = "%" + searchValue + "%";
            }

            using (var connection = OpenConnection())
            {
                var sql = @"select count(*) from Customers 
                            where (@searchValue = N'') or (CustomerName like @searchValue)";

                var parameters = new
                {
                    searchValue = searchValue ?? ""
                };

                count = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);

                connection.Close();
            }

            return count;
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Customer? Get(int id)
        {
            throw new NotImplementedException();
        }

        public bool InUsed(int id)
        {
            throw new NotImplementedException();
        }

        public IList<Customer> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<Customer> list = new List<Customer>();

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = "%" + searchValue + "%";
            }

            using (var connection = OpenConnection())
            {
                var sql = @"with cte as
                            (
	                            select	*, row_number() over (order by CustomerName) as RowNumber
	                            from	Customers 
	                            where	(@searchValue = N'') or (CustomerName like @searchValue)
                            )

                            select * from cte
                            where  (@pageSize = 0) 
                                or (RowNumber between (@page - 1) * @pageSize + 1 and @page * @pageSize)
                            order by RowNumber";

                var parameters = new
                {
                    page = page,
                    pageSize = pageSize,
                    searchValue = searchValue ?? ""
                };

                list = connection.Query<Customer>(sql: sql, param: parameters, commandType: CommandType.Text).ToList();

                connection.Close();
            }

            return list;
        }

        public bool Update(Customer data)
        {
            throw new NotImplementedException();
        }
    }
}
