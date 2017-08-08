
using DataAccess.Attributes;
using DataAccess.DataAccess.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
//using XNEW.DataAccess.Attributes;
//using XNEW.DataAccess.DataAccess.Attributes;

namespace DataAccess
{
    public class DatabaseCommon
    {
        #region 对象参数转换DbParameter
        /// <summary>
        /// 对象参数转换DbParameter
        /// </summary>
        /// <returns></returns>
        public static DbParameter[] GetParameter<T>(T entity)
        {
            IList<DbParameter> parameter = new List<DbParameter>();
            DbType dbtype = new DbType();
            Type type = entity.GetType();
            PropertyInfo[] props = type.GetProperties();
            foreach (PropertyInfo pi in props)
            {
                if (pi.GetValue(entity, null) != null)
                {
                    switch (pi.PropertyType.ToString())
                    {
                        case "System.Nullable`1[System.Int32]":
                            dbtype = DbType.Int32;
                            break;
                        case "System.Nullable`1[System.Decimal]":
                            dbtype = DbType.Decimal;
                            break;
                        case "System.Nullable`1[System.DateTime]":
                            dbtype = DbType.DateTime;
                            break;
                        default:
                            dbtype = DbType.String;
                            break;
                    }
                    var strIsNull = pi.GetValue(entity, null);
                    if (strIsNull.ToString() == "&nbsp;")
                    {
                        strIsNull = DBNull.Value;
                    }
                    parameter.Add(DbFactory.CreateDbParameter(DbHelper.DbParmChar + pi.Name, strIsNull, dbtype));
                }
            }
            return parameter.ToArray();
        }
        /// <summary>
        /// 对象参数转换DbParameter
        /// </summary>
        /// <returns></returns>
        public static DbParameter[] GetParameter(Hashtable ht)
        {
            IList<DbParameter> parameter = new List<DbParameter>();
            DbType dbtype = new DbType();
            foreach (string key in ht.Keys)
            {
                if (ht[key] is DateTime)
                    dbtype = DbType.DateTime;
                else
                    dbtype = DbType.String;
                parameter.Add(DbFactory.CreateDbParameter(DbHelper.DbParmChar + key, ht[key], dbtype));
            }
            return parameter.ToArray();
        }
        #endregion

        #region 获取实体类自定义信息
        /// <summary>
        /// 获取实体类主键字段
        /// </summary>
        /// <returns></returns>
        public static object GetKeyField<T>()
        {
            Type objTye = typeof(T);
            string _KeyField = "";
            PrimaryKeyAttribute KeyField;
            var name = objTye.Name;
            foreach (Attribute attr in objTye.GetCustomAttributes(true))
            {
                KeyField = attr as PrimaryKeyAttribute;
                if (KeyField != null)
                    _KeyField = KeyField.Name;
            }
            return _KeyField;
        }
        #region 获取实体类自定义信息
        /// <summary>
        /// 获取实体类主键字段
        /// </summary>
        /// <returns></returns>
        public static string GetKeyField(string className)
        {
            Assembly asmb = Assembly.LoadFrom(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "XNEW.Entity.dll"));
            Type objTye = asmb.GetType(className);
            string _KeyField = "";
            PrimaryKeyAttribute KeyField;
            var name = objTye.Name;
            foreach (Attribute attr in objTye.GetCustomAttributes(true))
            {
                KeyField = attr as PrimaryKeyAttribute;
                if (KeyField != null)
                    _KeyField = KeyField.Name;
            }
            return _KeyField;
        }
        #endregion
        /// <summary>
        /// 获取实体类主键字段
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public static object GetKeyFieldValue<T>(T entity)
        {
            Type objTye = typeof(T);
            string _KeyField = "";
            PrimaryKeyAttribute KeyField;
            var name = objTye.Name;
            foreach (Attribute attr in objTye.GetCustomAttributes(true))
            {
                KeyField = attr as PrimaryKeyAttribute;
                if (KeyField != null)
                    _KeyField = KeyField.Name;
            }
            PropertyInfo property = objTye.GetProperty(_KeyField);
            return property.GetValue(entity, null).ToString();
        }
        /// <summary>
        /// 获取实体类 字段中文名称
        /// </summary>
        /// <param name="pi">字段属性信息</param>
        /// <returns></returns>
        public static string GetFieldText(PropertyInfo pi)
        {
            DisplayNameAttribute descAttr;
            string txt = "";
            var descAttrs = pi.GetCustomAttributes(typeof(DisplayNameAttribute), true);
            if (descAttrs.Any())
            {
                descAttr = descAttrs[0] as DisplayNameAttribute;
                txt = descAttr.DisplayName;
            }
            else
            {
                txt = pi.Name;
            }
            return txt;
        }

        /// <summary>
        /// 获取实体类 字段中文名称 lwf
        /// </summary>
        /// <param name="pi">字段属性信息</param>
        /// <returns></returns>
        public static string GetFieldDesc(PropertyInfo pi)
        {
            DescriptionAttribute descAttr;
            string txt = "";
            var descAttrs = pi.GetCustomAttributes(typeof(DescriptionAttribute), true);
            if (descAttrs.Any())
            {
                descAttr = descAttrs[0] as DescriptionAttribute;
                txt = descAttr.Description;
            }
            else
            {
                txt = pi.Name;
            }
            return txt;
        }


              /// <summary>
        /// 获取忽略项 lwf
        /// </summary>
        /// <param name="pi">字段属性信息</param>
        /// <returns></returns>
        public static bool GetFieldSelect(PropertyInfo pi)
        {
            IsSelectAttribute descAttr;
            bool IsSelect = false;
            var descAttrs = pi.GetCustomAttributes(typeof(IsSelectAttribute), true);
            if (descAttrs.Any())
            {
                descAttr = descAttrs[0] as IsSelectAttribute;
                IsSelect = descAttr.Value;
            }
            else
            {
                IsSelect = false;

            }
            return IsSelect;
        }
        /// <summary>
        /// 获取实体类中文名称
        /// </summary>
        /// <returns></returns>
        public static string GetClassName<T>()
        {
            Type objTye = typeof(T);
            string entityName = "";
            var busingessNames = objTye.GetCustomAttributes(true).OfType<DisplayNameAttribute>();
            var descriptionAttributes = busingessNames as DisplayNameAttribute[] ?? busingessNames.ToArray();
            if (descriptionAttributes.Any())
                entityName = descriptionAttributes.ToList()[0].DisplayName;
            else
            {
                entityName = objTye.Name;
            }
            return entityName;
        }
        #endregion

        #region 格式化SQL语句
        /// <summary>
        /// 格式化SQL语句
        /// </summary>
        /// <param name="strsql">SQL语句</param>
        /// <returns></returns>
        public static string FormatSQL(string strsql)
        {
            strsql = strsql.ToLower();
            switch (DbHelper.DbType)
            {
                case DatabaseType.SqlServer:
                    return strsql;
                case DatabaseType.Oracle:
                    return strsql;
                case DatabaseType.MySql:
                    strsql = strsql.Replace(",condition,", ",`condition`,");
                    strsql = strsql.Replace("getdate()", "now()");
                    strsql = strsql.Replace("isnull(", "ifnull(");
                    return strsql;
                default:
                    throw new Exception("数据库类型目前不支持！");
            }
        }
        #endregion

        #region 拼接 Insert SQL语句
        /// <summary>
        /// 哈希表生成Insert语句
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="ht">Hashtable</param>
        /// <returns>int</returns>
        public static StringBuilder InsertSql(string tableName, Hashtable ht)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" Insert Into ");
            sb.Append(tableName);
            sb.Append("(");
            StringBuilder sp = new StringBuilder();
            StringBuilder sb_prame = new StringBuilder();
            foreach (string key in ht.Keys)
            {
                if (ht[key] != null)
                {
                    sb_prame.Append("," + key);
                    sp.Append("," + DbHelper.DbParmChar + "" + key);
                }
            }
            sb.Append(sb_prame.ToString().Substring(1, sb_prame.ToString().Length - 1) + ") Values (");
            sb.Append(sp.ToString().Substring(1, sp.ToString().Length - 1) + ")");
            return sb;
        }

        /// <summary>
        /// 泛型方法，反射生成InsertSql语句
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns>int</returns>
        public static StringBuilder InsertSql<T>(T entity)
        {
            Type type = entity.GetType();
            StringBuilder sb = new StringBuilder();
            sb.Append(" Insert Into ");
            sb.Append(type.Name);
            sb.Append("(");
            StringBuilder sp = new StringBuilder();
            StringBuilder sb_prame = new StringBuilder();
            PropertyInfo[] props = type.GetProperties();
            foreach (PropertyInfo prop in props)
            {
                IsSelectAttribute IsSelect;
                bool _IsSelect = true;
                foreach (Attribute attr in prop.GetCustomAttributes(true))
                {
                    IsSelect = attr as IsSelectAttribute;
                    if (IsSelect != null)
                        _IsSelect = IsSelect.Value;
                }
                if (_IsSelect)
                {
                    if (prop.GetValue(entity, null) != null)
                    {
                        sb_prame.Append("," + (prop.Name));
                        sp.Append("," + DbHelper.DbParmChar + "" + (prop.Name));
                    }
                }
            }
            sb.Append(sb_prame.ToString().Substring(1, sb_prame.ToString().Length - 1) + ") Values (");
            sb.Append(sp.ToString().Substring(1, sp.ToString().Length - 1) + ")");
            return sb;
        }
        #endregion

        #region 拼接 Update SQL语句
        /// <summary>
        /// 哈希表生成UpdateSql语句
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="ht">Hashtable</param>
        /// <param name="pkName">主键</param>
        /// <returns></returns>
        public static StringBuilder UpdateSql(string tableName, Hashtable ht, string pkName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" Update ");
            sb.Append(tableName);
            sb.Append(" Set ");
            bool isFirstValue = true;
            foreach (string key in ht.Keys)
            {
                if (ht[key] != null && pkName != key)
                {
                    if (isFirstValue)
                    {
                        isFirstValue = false;
                        sb.Append(key);
                        sb.Append("=");
                        sb.Append(DbHelper.DbParmChar + key);
                    }
                    else
                    {
                        sb.Append("," + key);
                        sb.Append("=");
                        sb.Append(DbHelper.DbParmChar + key);
                    }
                }
            }
            sb.Append(" Where ").Append(pkName).Append("=").Append(DbHelper.DbParmChar + pkName);
            return sb;
        }
        /// <summary>
        /// 泛型方法，反射生成UpdateSql语句
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="pkName">主键</param>
        /// <returns>int</returns>
        public static StringBuilder UpdateSql<T>(T entity, string pkName)
        {
            Type type = entity.GetType();
            PropertyInfo[] props = type.GetProperties();
            StringBuilder sb = new StringBuilder();
            sb.Append(" Update ");
            sb.Append(type.Name);
            sb.Append(" Set ");
            bool isFirstValue = true;
            foreach (PropertyInfo prop in props)
            {
                if (prop.GetValue(entity, null) != null && GetKeyField<T>().ToString() != prop.Name)
                {
                    IsSelectAttribute IsSelect;
                    bool _IsSelect = true;
                    foreach (Attribute attr in prop.GetCustomAttributes(true))
                    {
                        IsSelect = attr as IsSelectAttribute;
                        if (IsSelect != null)
                            _IsSelect = IsSelect.Value;
                    }
                    if (_IsSelect)
                    {
                        if (isFirstValue)
                        {
                            isFirstValue = false;
                            sb.Append(prop.Name);
                            sb.Append("=");
                            sb.Append(DbHelper.DbParmChar + prop.Name);
                        }
                        else
                        {
                            sb.Append("," + prop.Name);
                            sb.Append("=");
                            sb.Append(DbHelper.DbParmChar + prop.Name);
                        }
                    }
                }
            }
            sb.Append(" Where ").Append(pkName).Append("=").Append(DbHelper.DbParmChar + pkName);
            return sb;
        }
        /// <summary>
        /// 泛型方法，反射生成UpdateSql语句
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns>int</returns>
        public static StringBuilder UpdateSql<T>(T entity)
        {
            string pkName = GetKeyField<T>().ToString();
            Type type = entity.GetType();
            PropertyInfo[] props = type.GetProperties();
            StringBuilder sb = new StringBuilder();
            sb.Append("Update ");
            sb.Append(type.Name);
            sb.Append(" Set ");
            bool isFirstValue = true;
            foreach (PropertyInfo prop in props)
            {
                if (prop.GetValue(entity, null) != null && pkName != prop.Name)
                {
                    IsSelectAttribute IsSelect;
                    bool _IsSelect = true;
                    foreach (Attribute attr in prop.GetCustomAttributes(true))
                    {
                        IsSelect = attr as IsSelectAttribute;
                        if (IsSelect != null)
                            _IsSelect = IsSelect.Value;
                    }
                    if (_IsSelect)
                    {
                        if (isFirstValue)
                        {
                            isFirstValue = false;
                            sb.Append(prop.Name);
                            sb.Append("=");
                            sb.Append(DbHelper.DbParmChar + prop.Name);
                        }
                        else
                        {
                            sb.Append("," + prop.Name);
                            sb.Append("=");
                            sb.Append(DbHelper.DbParmChar + prop.Name);
                        }
                    }
                }
            }
            sb.Append(" Where ").Append(pkName).Append("=").Append(DbHelper.DbParmChar + pkName);
            return sb;
        }
        #endregion

        #region 拼接 Delete SQL语句
        /// <summary>
        /// 拼接删除SQL语句
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="pkName">字段主键</param>
        /// <returns></returns>
        public static StringBuilder DeleteSql(string tableName, string pkName)
        {
            return new StringBuilder("Delete From " + tableName + " Where " + pkName + " = " + DbHelper.DbParmChar + pkName + "");
        }
        /// <summary>
        /// 拼接删除SQL语句
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="ht">多参数</param>
        /// <returns></returns>
        public static StringBuilder DeleteSql(string tableName, Hashtable ht)
        {
            StringBuilder sb = new StringBuilder("Delete From " + tableName + " Where 1=1");
            foreach (string key in ht.Keys)
            {
                sb.Append(" AND " + key + " = " + DbHelper.DbParmChar + "" + key + "");
            }
            return sb;
        }
        /// <summary>
        /// 拼接删除SQL语句
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public static StringBuilder DeleteSql<T>(T entity)
        {
            Type type = entity.GetType();
            PropertyInfo[] props = type.GetProperties();
            StringBuilder sb = new StringBuilder("Delete From " + type.Name + " Where 1=1");
            foreach (PropertyInfo prop in props)
            {
                if (prop.GetValue(entity, null) != null)
                {
                    sb.Append(" AND " + prop.Name + " = " + DbHelper.DbParmChar + "" + prop.Name + "");
                }
            }
            return sb;
        }
        #endregion

        #region 拼接 Select SQL语句
        /// <summary>
        /// 拼接 查询 SQL语句
        /// </summary>
        /// <returns></returns>
        public static StringBuilder SelectSql<T>() where T : new()
        {

            string tableName = typeof(T).Name;
            PropertyInfo[] props = GetProperties(new T().GetType());
            StringBuilder sbColumns = new StringBuilder();

            foreach (PropertyInfo prop in props)
            {
                IsSelectAttribute IsSelect;
                bool _IsSelect = true;
                foreach (Attribute attr in prop.GetCustomAttributes(true))
                {
                    IsSelect = attr as IsSelectAttribute;
                    if (IsSelect != null)
                        _IsSelect = IsSelect.Value;
                }
                if (_IsSelect)
                {
                    string propertytype = prop.PropertyType.ToString();
                    sbColumns.Append(prop.Name + ",");
                }
            }
            if (sbColumns.Length > 0) sbColumns.Remove(sbColumns.ToString().Length - 1, 1);
            string strSql = "SELECT {0} FROM {1} WHERE 1=1 ";
            strSql = string.Format(strSql, sbColumns.ToString(), tableName + " ");
            return new StringBuilder(strSql);
        }
        /// <summary>
        /// 拼接 查询 SQL语句
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public static StringBuilder SelectSql(string tableName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM " + tableName + " WHERE 1=1 ");
            return strSql;
        }
        /// <summary>
        /// 拼接 查询条数 SQL语句
        /// </summary>
        /// <returns></returns>
        public static StringBuilder SelectCountSql<T>() where T : new()
        {
            string tableName = typeof(T).Name;//获取表名
            return new StringBuilder("SELECT Count(1) FROM " + tableName + " WHERE 1=1 ");
        }
        /// <summary>
        /// 拼接 查询最大数 SQL语句
        /// </summary>
        /// <param name="propertyName">属性字段</param>
        /// <returns></returns>
        public static StringBuilder SelectMaxSql<T>(string propertyName) where T : new()
        {
            string tableName = typeof(T).Name;//获取表名
            return new StringBuilder("SELECT MAX(" + propertyName + ") FROM " + tableName + "  WHERE 1=1 ");
        }
        #endregion

        #region 扩展
        /// <summary>
        /// 获取访问元素
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static PropertyInfo[] GetProperties(Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }
        #endregion
    }
}
