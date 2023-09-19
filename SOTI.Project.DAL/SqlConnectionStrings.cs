using System.Configuration;

namespace SOTI.Project.DAL
{
    public class SqlConnectionStrings
    {
        public static string GetConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["NorthwindCon"].ConnectionString;
            }
        }
    }
}
