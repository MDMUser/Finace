using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FinanceMs.Common
{
    /// <summary>
    /// 数据验证类
    /// </summary>
    public class Verification
    {
        /// <summary>
        /// 判断导入表中是否有必填属性
        /// </summary>
        /// <param name="dataColumns">导入表的表头集合</param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static string ImportColumns(DataColumnCollection dataColumns, string[] columns)
        {
            string msg = "导入必须包含：" + string.Join("、 ", columns) + "各列。";
            if (dataColumns.Count <= 0)
            {
                return msg;
            }
            for (int i = 0; i < columns.Count(); i++)
            {
                if (!dataColumns.Contains(columns[i].Trim()))
                {
                    return msg;
                }
            }
            return null;
        }

        /// <summary>
        /// 判断字符型参数是否超出范围
        /// 该参数是属于某个枚举中
        /// </summary>
        /// <param name="paraValue">参数值</param>
        /// <param name="paraName">参数名</param>
        /// <param name="enumType">枚举类型</param>
        public static bool CharRangeOut(string paraValue,  Type enumType)
        {
            int value = -1;
            int.TryParse(paraValue, out value);
            return Enum.IsDefined(enumType, value);
        }

    }
}
