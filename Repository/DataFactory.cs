using DataAccess;
using Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository
{
    /// <summary>
    /// 操作数据库工厂
    /// </summary>
    public class DataFactory
    {
        /// <summary>
        /// 当前数据库类型
        /// </summary>
        private static string DbType = Utilities.ConfigHelper.AppSettings("ComponentDbType");

        /// <summary>
        /// 获取指定的数据库连接
        /// </summary>
        /// <param name="connString"></param>
        /// <returns></returns>
        public static IDatabase Database(string connString)
        {
            return new Database(connString);
        }
        /// <summary>
        /// 获取指定的数据库连接
        /// </summary>
        /// <returns></returns>
        public static IDatabase Database()
        {
            switch (DbType)
            {
                case "SqlServer":
                    return Database("Connectionstr");
                case "MySql":
                    return Database("MySqlConnectionstr");
                case "Oracle":
                    return Database("OracleConnectionstr");
                default:
                    return null;
            }
        }

        /// <summary>
        /// 根据参数获取指定的连接字符串
        /// </summary>
        /// <returns></returns>
        public static IDatabase DatabaseByConfigName(string name)
        {
            return Database(name);
        }
    }
}
