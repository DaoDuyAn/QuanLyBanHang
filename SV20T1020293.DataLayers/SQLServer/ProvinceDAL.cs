﻿using SV20T1020293.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020293.DataLayers.SQLServer
{
    public class ProvinceDAL : _BaseDAL, ICommonDAL<Province>
    {
        public ProvinceDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Province data)
        {
            throw new NotImplementedException();
        }

        public int Count(string searchValue = "")
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Province? Get(int id)
        {
            throw new NotImplementedException();
        }

        public bool InUsed(int id)
        {
            throw new NotImplementedException();
        }

        public IList<Province> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            throw new NotImplementedException();
        }

        public bool Update(Province data)
        {
            throw new NotImplementedException();
        }
    }
}
