using System.Collections.Generic;
using System.Data.SqlClient;

namespace Jobs
{
    internal class SqlLiteProvider : SQLProvider
    {
        public override void LoadCategories(List<LoadCategories.Category> categories)
        {
            using(var connection = new SqlConnection(""))
            {
                connection.Open();
            }
        }
    }
}