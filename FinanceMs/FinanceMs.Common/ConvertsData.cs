using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using Genersoft.MDM.Pub.Common;
using System.ComponentModel;

namespace FinanceMs.Common
{
    public class ConvertsData
    {
        /// <summary>
        /// DataTable转换成集合
        /// </summary>
        /// <typeparam name="T">转换类型</typeparam>
        /// <param name="dataTable">数据源</param>
        /// <returns></returns>
        public static IList<T> DataTableToList<T>(DataTable dataTable)
        {
            // 确认参数有效
            if (dataTable == null || dataTable.Rows.Count == 0 || dataTable.Columns.Count == 0)
            {
                return null;
            }
            DataTable dt = dataTable;

            IList<T> list = new List<T>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                // 创建泛型对象
                T _t = Activator.CreateInstance<T>();
                // 获取对象所有属性
                PropertyInfo[] propertyInfo = _t.GetType().GetProperties();
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    foreach (PropertyInfo info in propertyInfo)
                    {
                        // 属性名称和列名相同时赋值
                        if (dt.Columns[j].ColumnName.ToUpper().Equals(info.Name.ToUpper()))
                        {
                            if (dt.Rows[i][j] != DBNull.Value)
                            {
                                if (info.PropertyType == typeof(int) && info.PropertyType != dt.Rows[i][j].GetType())
                                {
                                    int value = 0;
                                    int.TryParse(dt.Rows[i][j].ToString(), out value);
                                    info.SetValue(_t, value, null);
                                }
                                else
                                {
                                    info.SetValue(_t, dt.Rows[i][j], null);
                                }
                            }
                            else
                            {
                                info.SetValue(_t, null, null);
                            }
                            break;
                        }
                    }
                }
                list.Add(_t);
            }
            return list;
        }

        /// <summary>
        /// 根据实体类字段的Description属性将datatable转为List
        /// </summary>
        /// <typeparam name="T">转换类型</typeparam>
        /// <param name="dataTable">数据源</param>
        /// <returns></returns>
        public static IList<T> DataTableToListByProperties<T>(DataTable dataTable)
        {
            // 确认参数有效
            if (dataTable == null || dataTable.Rows.Count == 0 || dataTable.Columns.Count == 0)
            {
                return null;
            }
            DataTable dt = dataTable;

            IList<T> list = new List<T>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                // 创建泛型对象
                T _t = Activator.CreateInstance<T>();
                // 获取对象所有属性
                PropertyInfo[] propertyInfo = _t.GetType().GetProperties();
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    foreach (PropertyInfo info in propertyInfo)
                    {
                        DescriptionAttribute descAttr = Attribute.GetCustomAttribute(info, typeof(DescriptionAttribute)) as DescriptionAttribute;
                        if (descAttr == null)
                        {
                            continue;
                        }
                        // 属性的Description属性和列名相同时赋值
                        if (dt.Columns[j].ColumnName.Trim().Equals(descAttr.Description))
                        {
                            if (dt.Rows[i][j] != DBNull.Value)
                            {
                                if (info.PropertyType == typeof(int))
                                {
                                    int value = 0;
                                    int.TryParse(dt.Rows[i][j].ToString(), out value);
                                    info.SetValue(_t, value, null);
                                }
                                else
                                {
                                    info.SetValue(_t, dt.Rows[i][j], null);
                                }
                            }
                            else
                            {
                                info.SetValue(_t, null, null);
                            }
                            break;
                        }
                    }
                }
                list.Add(_t);
            }
            return list;
        }

        #region 使用NPOI处理excel数据
        /// <summary>
        /// 将Excel数据读取到DataSet
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public DataSet ExcelToDataSet(string filePath)
        {
            string msg = "";
            return ExcelHelper.ExcelToDataSet(filePath, ref msg);
        }

        /// <summary>
        /// 导出Excel，执行函数
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="filePath"></param>
        public void DataSetToExcel(DataSet ds, string filePath)
        {
            string msg = "";
            ExcelHelper.DataSetToExcel(ds, filePath, ref msg);
        }
        #endregion




        /// <summary>
        /// 判断字符串是否为空，若为空则返回result
        /// </summary>
        /// <param name="value">判断的字符串</param>
        /// <param name="result">返回值</param>
        /// <returns></returns>
        public static string ValidNullString(string value, string result)
        {
            return string.IsNullOrWhiteSpace(value) ? result : value;
        }

        /// <summary>
        /// 添加时，根据代码项的Name得code
        /// </summary>
        /// <param name="codeTableNM">码表内码</param>
        /// <param name="itemName">代码项Name</param>
        /// <returns></returns>
        public static string GetCodeByName(string codeTableNM, string itemName)
        {
            string str = "";
            if (!string.IsNullOrWhiteSpace(itemName))
            {
                str = " (SELECT Code FROM  GSCodeItems WHERE CodeSetNM='" + codeTableNM + "' AND Name='" + itemName.Trim() + "') ";
            }
            else
            {
                str = " '' ";
            }
            return str;
        }

    }
}
