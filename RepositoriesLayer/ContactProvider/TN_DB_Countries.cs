using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RacoonProvider
{
    public class TN_DB_Countries : Core.Disposable
    {
        public List<Entities.Country> GetAllCountries()
        {

            using var DAL2 = new DataAccess.DataAccessLayer();
           
            return DAL2.ExecuteReader<Entities.Country>("sp_TN_DB_Countries");
        }
    }
}
