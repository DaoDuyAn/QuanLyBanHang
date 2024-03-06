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
    public class EmployeeDAL : _BaseDAL, ICommonDAL<Employee>
    {
        public EmployeeDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Employee data)
        {
            int id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"if exists(select * from Employees where Email = @Email)
                                select -1
                            else
                                begin
                                    insert into Employees(FullName,BirthDate,Address,Phone,Email,Photo,IsWorking)
                                    values(@FullName,@BirthDate,@Address,@Phone,@Email,@Photo,@IsWorking);
                                    select @@identity;
                                end";

                var parameters = new
                {
                    FullName = data.FullName ?? "",
                    BirthDate = data.BirthDate,
                    Address = data.Address ?? "",
                    Phone = data.Phone ?? "",
                    Email = data.Email ?? "",
                    Photo = data.Photo ?? "",
                    IsWorking = data.IsWorking
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
                searchValue = "%" + searchValue + "%";

            using (SqlConnection connection = OpenConnection())
            {
                var sql = @"
                    select count(*) from Employees
                    where (@searchValue = N'') or (FullName like @searchValue)";

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
                var sql = "delete from Employees where EmployeeID = @employeeId";
                var parameters = new { employeeId = id };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public Employee? Get(int id)
        {
            Employee? data = null;
            using (var connection = OpenConnection())
            {
                var sql = "select * from Employees where EmployeeID = @employeeId";
                var parameters = new { employeeId = id };
                data = connection.QueryFirstOrDefault<Employee>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public bool InUsed(int id)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"if exists(select * from Orders where EmployeeID = @employeeId)
                                select 1
                            else 
                                select 0";

                var parameters = new { employeeId = id };
                result = connection.ExecuteScalar<bool>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return result;
        }

        public IList<Employee> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<Employee> listEmployees = new List<Employee>();

            if (!searchValue.IsNullOrEmpty())
                searchValue = "%" + searchValue + "%";

            using (var connection = OpenConnection())
            {
                var sql = @"
                    with cte as
                    (
	                    select *, ROW_NUMBER() over (order by FullName) as RowNumber
	                    from Employees
	                    where (@searchvalue = N'') or (FullName like @searchvalue)
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

                listEmployees = connection.Query<Employee>(sql: sql, param: parameters, commandType: CommandType.Text).ToList();

                connection.Close();
            }

            return listEmployees;
        }

        public bool Update(Employee data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"if not exists(select * from Employees where EmployeeID <> @EmployeeID and Email = @Email)
                                begin
                                    update Employees 
                                    set FullName = @FullName,
                                        Photo = @Photo,
                                        BirthDate = @BirthDate,
                                        Address = @Address,
                                        Phone = @Phone,
                                        Email = @Email,
                                        IsWorking = @IsWorking
                                    where EmployeeID = @EmployeeID
                                end";

                var parameters = new
                {
                    EmployeeID = data.EmployeeID,
                    FullName = data.FullName ?? "",
                    BirthDate = data.BirthDate,
                    Address = data.Address ?? "",
                    Phone = data.Phone ?? "",
                    Email = data.Email ?? "",
                    Photo = data.Photo ?? "",
                    IsWorking = data.IsWorking
                };

                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }
    }
}
