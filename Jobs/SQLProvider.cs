using System;
using System.Collections.Generic;
using static Jobs.LoadCategories;

namespace Jobs
{
    public abstract class SQLProvider
    {
        public abstract void LoadCategories(List<Category> categories);
        
        public static SQLProvider Get(DbTypes dbType)
        {
            if (dbType == DbTypes.SqlLite)
                return new SqlLiteProvider();
            if (dbType == DbTypes.SqlServer)
                return new SqlServerProvider();

            throw new ArgumentOutOfRangeException();
        }
    }

    public enum DbTypes { SqlServer, SqlLite };
}
