using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using SV20T1020293.DomainModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020293.DataLayers.SQLServer
{
    public class SupplierDAL : _BaseDAL, ICommonDAL<Supplier>
    {
        public SupplierDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Supplier data)
        {
            int id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"insert into Suppliers(SupplierName, ContactName, Provice, Address, Phone, Email, BirthDate)
		                    values (@SupplierName, @ContactName, @Provice, @Address, @Phone, @Email, @BirthDate)
		                    select @@IDENTITY";

                var parameters = new
                {
                    SupplierName = data.SupplierName ?? "",
                    ContactName = data.ContactName ?? "",
                    Provice = data.Provice ?? "",
                    Address = data.Address ?? "",
                    Phone = data.Phone ?? "",
                    Email = data.Email ?? "",
                    BirthDate = data.BirthDate,

                };

                id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return id;
        }

        public int Count(string searchValue = "")
        {
            int count = 0;

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = "%" + searchValue + "%";
            }

            using (SqlConnection connection = OpenConnection())
            {
                var sql = @"
                    select count(*) from Suppliers
                    where (@searchValue = N'') or (SupplierName like @searchValue)";

                var para = new
                {
                    searchValue = searchValue ?? "",
                };

                count = connection.ExecuteScalar<int>(sql: sql, param: para, commandType: CommandType.Text);

                connection.Close();
            }

            return count;
        }

        public bool Delete(int id)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = "delete from Suppliers where SupplierID = @SupplierID";
                var parameters = new { SupplierID = id };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public Supplier? Get(int id)
        {
            Supplier? data = null;
            using (var connection = OpenConnection())
            {
                var sql = "select * from Suppliers where SupplierId = @SupplierID";
                var parameters = new { SupplierID = id };
                data = connection.QueryFirstOrDefault<Supplier>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public bool InUsed(int id)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"if exists(select * from Products where SupplierID = @SupplierID)
                                select 1
                            else 
                                select 0";
                var parameters = new { SupplierID = id };
                result = connection.ExecuteScalar<bool>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }

            return result;
        }

        public IList<Supplier> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<Supplier> listSuppliers = new List<Supplier>(); ;

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = "%" + searchValue + "%";
            }

            using (var connection = OpenConnection())
            {
                var sql = @"
                    with cte as
                    (
	                    select *, ROW_NUMBER() over (order by SupplierName) as RowNumber
	                    from Suppliers
	                    where (@searchValue = N'') or (SupplierName like @searchValue)
                    )

                    select * from cte
                    where (@pageSize= 0)
	                    or (RowNumber between (@page - 1) * @pageSize + 1 and @page * @pageSize)
                    order by RowNumber;";

                var parameters = new
                {
                    page = page,
                    pageSize = pageSize,
                    searchValue = searchValue ?? ""
                };

                listSuppliers = connection.Query<Supplier>(sql: sql, param: parameters, commandType: CommandType.Text).ToList();

                connection.Close();
            }

            return listSuppliers;
        }

        public bool Update(Supplier data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"update Suppliers 
                            set SupplierName = @supplierName,
                                ContactName = @contactName,
                                Provice = @provice,
                                Address = @address,
                                Phone = @phone,
                                Email = @email,
                                BirthDate = @BirthDate
                            where SupplierID = @supplierId";

                var parameters = new
                {
                    supplierId = data.SupplierID,
                    supplierName = data.SupplierName ?? "",
                    contactName = data.ContactName ?? "",
                    Provice = data.Provice ?? "",
                    Address = data.Address ?? "",
                    Phone = data.Phone ?? "",
                    Email = data.Email ?? "",
                    BirthDate = data.BirthDate,
                };

                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }
    }
}
