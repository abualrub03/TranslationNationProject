using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RacoonProvider
{
    public class TN_DB_University : Core.Disposable
    {
        public List<Entities.University> UniversityBasedOnCountryId(int id)
        {
           
            using var DAL2 = new DataAccess.DataAccessLayer();
            DAL2.Parameters = new List<SqlParameter> {
                new SqlParameter{ ParameterName = "@CountryId", Value =  id }
            };
            return DAL2.ExecuteReader<Entities.University>("sp_TN_DB_UniversityBasedOnCountryId");
        }

        public List<ViewModel.SupervisorViewModel> GetUniversityNameAndNumberOfStudents(int id)
        {

            using var DAL2 = new DataAccess.DataAccessLayer();
            DAL2.Parameters = new List<SqlParameter> {
                new SqlParameter{ ParameterName = "@UniversityId", Value =  id }
            };
            return DAL2.ExecuteReader<ViewModel.SupervisorViewModel>("spGetUniversityNameAndNumberOfStudents");
        }



        public List<Entities.Accounts> GetVerificationRequests(int id)
        {

            using var DAL2 = new DataAccess.DataAccessLayer();
            DAL2.Parameters = new List<SqlParameter> {
                new SqlParameter{ ParameterName = "@UniversityId", Value =  id }
            };
            return DAL2.ExecuteReader<Entities.Accounts>("spGetVerificationRequests");
        }
    }
}
