using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Web;

namespace PublicLibrary
{
  public  class DataConvert
    {
        private static string rowSplit = "@@@@";     //行分割符
        private static string cellSplit = "***";   //单元格分割符
        private static string replaceRowSplit = "@@@";
        private static string replaceCellSplit = "**";   //单元格分割符 

        private static string nullTag = "%%%";     //Null标识
        private static string specialStr = "^";    //特殊字符

        private static string splitKeyValue = ":";
        private static string addchar = "^";
        private static string tagFieldName = "modifiedtag";

        private static Dictionary<string, string> replaceStr = new Dictionary<string, string>();       //需要替换的字符串

        public static string TagFieldName
        {
            get { return tagFieldName; }
        }
        public static string RowSplit
        {
            get { return rowSplit; }
        }

        public static string CellSplit
        {
            get { return cellSplit; }
        }

        public static string SplitKeyValue
        {
            get { return splitKeyValue; }
        }
        public static string AddChar
        {
            get { return addchar; }
        }


        //增加特殊字符
        private static void AddPaticular()
        {
        }
        //获取第一行参数部分
        public static string getconfirgue(string context)
        {
            string[] strs = context.Split(new String[] { rowSplit }, StringSplitOptions.None);
            if (strs.Length > 0)
            {
                return strs[0];
            }
            else
            {
                return "";
            }
        }

        //根据context分析出表，如果有多行时，取其第一行.因为第一行表示数据，第二行表示条件
        public static DataTable FormatDataByKeyValue(string context)
        {
            string keyvalue = "";
            int startindex = 0;
            Dictionary<string, string> keys = new Dictionary<string, string>();
            var rows = context.Split(new string[] { rowSplit }, StringSplitOptions.None);
            if (rows.Length > 1)
                context = rows[0];
            string[] cells = context.Split(new String[] { CellSplit }, StringSplitOptions.None);
            for (int index = 0; index < cells.Length; index++)
            {
                keyvalue = cells[index];
                keyvalue = Out_SplitStr(keyvalue);
                startindex = keyvalue.IndexOf(":");
                if (startindex > 0)
                {
                    if (startindex == keyvalue.Length - 1)
                    {
                        keys[keyvalue.Substring(0, startindex)] = keyvalue.Substring(startindex + 1, keyvalue.Length - startindex - 1);
                    }
                    else
                    {
                        keys[keyvalue.Substring(0, startindex)] = keyvalue.Substring(startindex + 1, keyvalue.Length - startindex - 1);
                    }
                }
            }
            DataTable dt = new DataTable();
            DataColumn col = null;
            foreach (string key in keys.Keys)
            {
                col = new DataColumn(key);
                dt.Columns.Add(col);
            }
            DataRow dr = dt.NewRow();
            foreach (string key in keys.Keys)
            {
                dr[key] = keys[key];
            }
            dt.Rows.Add(dr);
            return dt;
        }

        //增加特殊字符
        //public static ClientDataSouce FormatClinetContext(string context)
        //{
        //    string firstrow = "";
        //    string secondrow = "";
        //    string cellvalue = "";
        //    string key = "";
        //    int postion = 0;
        //    ClientDataSouce returnData = new ClientDataSouce();
        //    List<string> primaryKeys = new List<string>();
        //    Dictionary<string, string> keys = new Dictionary<string, string>();
        //    string[] rows = context.Split(new String[] { RowSplit }, StringSplitOptions.None);
        //    if (rows.Length < 3)
        //    {
        //        return null;
        //    }
        //    firstrow = rows[0];
        //    string[] cells = firstrow.Split(new String[] { CellSplit }, StringSplitOptions.None);
        //    for (int i = 0; i < cells.Length; i++)
        //    {
        //        cellvalue = Out_SplitStr(cells[i]);
        //        postion = cellvalue.IndexOf(splitKeyValue);
        //        key = cellvalue.Substring(0, postion).ToLower();
        //        if (postion < cellvalue.Length - 1)
        //        {
        //            cellvalue = cellvalue.Substring(postion + 1, cellvalue.Length - (postion + 1));
        //        }
        //        else
        //        {
        //            cellvalue = "";
        //        }
        //        if (key == "primarykeys")
        //        {
        //            string[] keystrs = cellvalue.Split(new char[] { ',' });
        //            for (int j = 0; j < keystrs.Length; j++)
        //            {
        //                if (keystrs[j] != "")
        //                {
        //                    primaryKeys.Add(keystrs[j].ToLower());
        //                }
        //            }
        //        }
        //        else
        //        {
        //            keys.Add(key, cellvalue);
        //        }
        //    }
        //    returnData.Keys = keys;
        //    returnData.PrimaryKeys = primaryKeys;
        //    secondrow = rows[1];

        //    DataTable dt = new DataTable();
        //    DataColumn col = null;
        //    cells = secondrow.Split(new String[] { CellSplit }, StringSplitOptions.None);
        //    for (int i = 0; i < cells.Length; i++)
        //    {
        //        col = new DataColumn();
        //        col.ColumnName = cells[i];
        //        dt.Columns.Add(col);
        //    }
        //    //正式增加数据
        //    DataRow dr = null;
        //    for (int i = 2; i < rows.Length; i++)
        //    {
        //        if (i == (rows.Length - 1))
        //        {
        //            if (rows[i].Split(new string[] { CellSplit }, StringSplitOptions.None).Length != cells.Length)
        //                break;
        //        }
        //        cells = rows[i].Split(new String[] { CellSplit }, StringSplitOptions.None);
        //        dr = dt.NewRow();
        //        for (int j = 0; j < cells.Length; j++)
        //        {
        //            cellvalue = Out_SplitStr(cells[j], true);
        //            dr[j] = cellvalue;
        //        }
        //        dt.Rows.Add(dr);
        //    }
        //    returnData.DT = dt;
        //    return returnData;
        //}

        /// <summary>
        /// 单条数据格式化
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string FormatClinetContextForData(string context)
        {
            string firstrow = "";
            string secondrow = "";
            string cellvalue = "";
            string key = "";
            string keys = "";
            int postion = 0;
            ClientDataSouce returnData = new ClientDataSouce();
            List<string> primaryKeys = new List<string>();
            //Dictionary<string, string> keys = new Dictionary<string, string>();
            string[] rows = context.Split(new String[] { RowSplit }, StringSplitOptions.None);

            firstrow = rows[0];
            string[] cells = firstrow.Split(new String[] { CellSplit }, StringSplitOptions.None);
            for (int i = 0; i < cells.Length; i++)
            {
                cellvalue = Out_SplitStr(cells[i]);
                postion = cellvalue.IndexOf(splitKeyValue);
                key = cellvalue.Substring(0, postion).ToLower();
                if (postion < cellvalue.Length - 1)
                {
                    cellvalue = cellvalue.Substring(postion + 1, cellvalue.Length - (postion + 1));
                }
                else
                {
                    cellvalue = "";
                }
                if (key == "primarykeys")
                {
                    string[] keystrs = cellvalue.Split(new char[] { ',' });
                    for (int j = 0; j < keystrs.Length; j++)
                    {
                        if (keystrs[j] != "")
                        {
                            primaryKeys.Add(keystrs[j].ToLower());
                        }
                    }
                }
                else
                {
                    //keys.Add(key, cellvalue);
                    keys += "\"" + key + "\":\"" + cellvalue + "\",";
                }
            }
            keys = "{" + (keys.Substring(0, keys.Length - 1)) + "}";
            return keys;
        }

        public static string FormatDataTable(DataTable dt, string addResult)
        {
            int index = 0;
            string cellValue = "";
            string rowValue = "";
            int rowIndex = 0;
            int cols = 0;
            string dataTableStr = "";
            DataRow dr = null;

            //addResult = addResult;// addResult.Replace(replaceCellSplit, replaceCellSplit + specialStr);
            dataTableStr = addResult;
            if (dt == null) { return dataTableStr; }
            cols = dt.Columns.Count;
            //增加列名
            for (index = 0; index < dt.Columns.Count; index++)
            {
                cellValue = dt.Columns[index].ColumnName;
                if (rowValue == "")
                {
                    rowValue = cellValue;
                }
                else
                {
                    rowValue = rowValue + cellSplit + cellValue;
                }
            }
            //增加一标志 记录值是否改变
            rowValue = rowValue + cellSplit + tagFieldName;
            dataTableStr = dataTableStr + rowSplit + rowValue;
            for (rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
            {
                rowValue = "";
                dr = dt.Rows[rowIndex];
                for (index = 0; index < cols; index++)
                {
                    cellValue = dr[index].ToString();
                    if (cellValue.Length > 2)
                    {
                        cellValue = cellValue.Replace(replaceCellSplit, replaceCellSplit + specialStr);     //去掉特殊字符
                    }
                    if (index == 0)
                    {
                        rowValue = cellValue;
                    }
                    else
                    {
                        rowValue = rowValue + cellSplit + cellValue;
                    }
                }
                rowValue = rowValue + cellSplit + "0";
                rowValue = rowValue.Replace(replaceRowSplit, replaceRowSplit + specialStr);
                if (dataTableStr == "")
                {
                    dataTableStr = rowValue;
                }
                else
                {
                    dataTableStr = dataTableStr + rowSplit + rowValue;
                }
            }
            return dataTableStr;
        }
        public static T UnformatToObject<T>(string str) where T : class
        {
            try
            {

                JsonQueryStringConverter json = new JsonQueryStringConverter();
                object jsonobj = json.ConvertStringToValue(str, typeof(T));
                T retobj = jsonobj as T;
                return retobj;
                //System.ServiceModel.Dispatcher
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private static bool CompareTableCol(DataTable dt1, DataTable dt2)
        {
            if (dt1.Columns.Count != dt2.Columns.Count) return false;

            foreach (DataColumn col in dt1.Columns)
            {
                if (!dt2.Columns.Contains(col.ColumnName))
                    return false;
            }
            return true;
        }
        public static DataSet CleanupDataset(DataSet ds)
        {
            var targetDs = new DataSet();
            DataTable data = null;
            DataTable colcfg = null;
            if (ds.Tables.Count == 1)//不存在列配置
            {
                data = ds.Tables[0];
            }
            else if (ds.Tables.Count == 2)
            {
                if (CompareTableCol(ds.Tables[0], ds.Tables[1]))//不存在列配置,采用系统游标分页
                {
                    data = ds.Tables[1];
                }
                else//存在列配置，采用主键分页
                {
                    data = ds.Tables[0];
                    colcfg = ds.Tables[1];
                }
            }
            else if (ds.Tables.Count == 3)//存在列配置，采用系统游标分页
            {
                data = ds.Tables[1];
                colcfg = ds.Tables[2];
            }
            if (data != null)
                targetDs.Tables.Add(data.Copy());
            if (colcfg != null)
                targetDs.Tables.Add(colcfg.Copy());

            return targetDs;
        }
        //public static string FomatDataset(DataSet ds, string addResult)
        //{
        //    ReturnValue rv = new ReturnValue();
        //    rv.RetResult = addResult;

        //    string itemValue = "";
        //    CommonReturnData item = null;
        //    List<CommonReturnData> datas = new List<CommonReturnData>();
        //    DataTable data = ds.Tables[0];
        //    DataTable colcfg = ds.Tables.Count > 1 ? ds.Tables[1] : null;


        //    //增加数据
        //    foreach (DataColumn col in data.Columns)
        //    {
        //        item = new CommonReturnData();
        //        item.Field = col.ColumnName.ToLower();
        //        item.Value = new List<string>();
        //        item.Visible = 1;//重要
        //        foreach (DataRow row in data.Rows)
        //        {
        //            itemValue = row[col] == DBNull.Value ? string.Empty : row[col].ToString();
        //            item.Value.Add(itemValue);
        //        }
        //        datas.Add(item);
        //    }


        //    //修改列配置
        //    if (colcfg != null)
        //    {
        //        foreach (DataRow row in colcfg.Rows)
        //        {
        //            if (ds.Tables[0].Rows.Count > 0)
        //            {
        //                CommonReturnData crd = datas.Find(p => p.Field.ToLower() == row["Field"].ToString().ToLower());
        //                if (crd != null)
        //                {
        //                    crd.ColOrder = row["ColOrder"] == DBNull.Value ? 10000 : Convert.ToInt32(row["ColOrder"]);
        //                    crd.RelationColumn = row["Relation_Column"] == DBNull.Value ? 0 : Convert.ToInt32(row["Relation_Column"]);
        //                    crd.IsMainCol = Convert.ToBoolean(row["IsMainCol"]);
        //                    crd.Label = row["DisplayName"] == DBNull.Value ? string.Empty : row["DisplayName"].ToString();
        //                    crd.ConditionPlus = row["ConditionPlus"] == DBNull.Value ? string.Empty : row["ConditionPlus"].ToString();
        //                    crd.TabID = row["Target_Tab_Rec_No"] == DBNull.Value ? 0 : Convert.ToInt32(row["Target_Tab_Rec_No"]);
        //                    crd.EditURL = row["EditURL"] == DBNull.Value ? string.Empty : row["EditURL"].ToString();
        //                    crd.Visible = row["Visible"] == DBNull.Value ? 1 : Convert.ToInt32(row["Visible"]);
        //                    crd.Required = row["Required"] == DBNull.Value ? false : Convert.ToBoolean(row["Required"]);
        //                    crd.ColType = row["ColType"] == DBNull.Value ? string.Empty : Convert.ToString(row["ColType"]);
        //                    crd.IsCustomizedField = row["isCustomizeField"] == DBNull.Value ? false : Convert.ToBoolean(row["isCustomizeField"]);
        //                }
        //            }
        //            else
        //            {
        //                datas.Add(new CommonReturnData
        //                {
        //                    IsMainCol = Convert.ToBoolean(row["IsMainCol"]),
        //                    Field = row["Field"] == DBNull.Value ? string.Empty : row["Field"].ToString().ToLower(),
        //                    Label = row["DisplayName"] == DBNull.Value ? string.Empty : row["DisplayName"].ToString(),
        //                    ConditionPlus = row["ConditionPlus"] == DBNull.Value ? string.Empty : row["ConditionPlus"].ToString(),
        //                    TabID = row["Target_Tab_Rec_No"] == DBNull.Value ? 0 : Convert.ToInt32(row["Target_Tab_Rec_No"]),
        //                    Value = new List<string>(),
        //                    EditURL = string.Empty,
        //                    Visible = row["Visible"] == DBNull.Value ? 1 : Convert.ToInt32(row["Visible"]),
        //                    Required = row["Required"] == DBNull.Value ? false : Convert.ToBoolean(row["Required"]),
        //                    ColType = row["ColType"] == DBNull.Value ? string.Empty : Convert.ToString(row["ColType"]),
        //                    IsCustomizedField = row["isCustomizeField"] == DBNull.Value ? false : Convert.ToBoolean(row["isCustomizeField"])
        //                });
        //            }
        //        }
        //    }
        //    rv.CommRetData = datas;
        //    return ValueToString(rv, rv.GetType());
        //}
        public static string ConvertDsToJson(DataTable dt)
        {
            return ConvertDsToJson(dt, false);
        }
        public static string ConvertDsToJson(DataTable dt, bool isLowerField)
        {
            if (dt == null) { return "[]"; }

            var colValue = "";
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("[");
            foreach (DataRow row in dt.Rows)
            {
                jsonBuilder.Append("{");
                foreach (DataColumn col in dt.Columns)
                {
                    if (isLowerField)
                    {
                        col.ColumnName = col.ColumnName.ToLower();
                    }

                    if (col.DataType == typeof(DateTime))
                    {
                        colValue = ReplaceSpecStr(row[col] != DBNull.Value ? Convert.ToDateTime(row[col]).ToString("yyyy-MM-dd HH:mm:ss") : string.Empty);
                    }
                    else
                    {
                        colValue = ReplaceSpecStr(Convert.ToString(row[col]));
                    }

                    jsonBuilder.Append("\"" + ReplaceSpecStr(col.ColumnName) + "\"" + ":\"" + colValue + "\"");
                    jsonBuilder.Append(",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("}");
                jsonBuilder.Append(",");
            }
            if (jsonBuilder.Length > 1)
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("]");
            return jsonBuilder.ToString();
        }
        public static string ReplaceSpecStr(string str)
        {
            var newStr = ReplaceUnicodeToString(str);
            newStr = newStr.Trim()
                .Replace("\t", "")
                .Replace("\b", "")
                .Replace("\f", "")
                .Replace("\n", "")
                .Replace("\r", "")
                .Replace("\\", "\\\\")
                .Replace("\"", "\\\"")
                ;
            return newStr;
        }
        /// <summary>  
        /// 替换特珠的unicode字符  
        /// </summary>  
        /// <param name="srcText"></param>  
        /// <returns></returns>  
        public static string ReplaceUnicodeToString(string srcText)
        {
            StringBuilder sb = new StringBuilder();
            var oneByte = new byte[2];
            var bytes = Encoding.Unicode.GetBytes(srcText);
            for (var i = 0; i < bytes.Length; i += 2)
            {
                oneByte[0] = bytes[i];
                oneByte[1] = bytes[i + 1];

                if (oneByte[0] == 0x29 && oneByte[1] == 0x20)
                {
                    continue;
                }

                sb.Append(Encoding.Unicode.GetString(oneByte));
            }
            return sb.ToString();
        }
        public static T ConvertToObjectFromJson<T>(string str) where T : class
        {
            JsonQueryStringConverter converter = new JsonQueryStringConverter();
            if (converter.CanConvert(typeof(T)))
            {
                try
                {
                    return (T)converter.ConvertStringToValue(str, typeof(T));
                }
                catch
                {
                    return default(T);
                }
            }
            return default(T);
        }

        //将对象转换成json字符串
        public static string ValueToString(object val, Type type)
        {
            JsonQueryStringConverter json = new JsonQueryStringConverter();
            string jsonstr = json.ConvertValueToString(val, type);
            return jsonstr;
        }

        //设置字符串中某一项的值
        public static string delItemByName(string name, string souce)
        {
            int start_pos = -1;
            int find_pos = -1;
            string patten = name + splitKeyValue;
            if (name.Trim().Length == 0 || souce.Trim().Length == 0)
            {
                throw new ArgumentNullException();
            }

            if (souce.Substring(0, patten.Length) == patten)
            {
                start_pos = 0;
            }
            else
            {
                patten = cellSplit + name + splitKeyValue;
                find_pos = souce.IndexOf(patten, start_pos + 1);
                if (find_pos >= 0)
                {
                    start_pos = find_pos;
                }
            }

            if (start_pos < 0)
            {
                return souce;
            }
            //  start_pos=
            find_pos = souce.IndexOf(cellSplit, start_pos + cellSplit.Length);
            if (find_pos < 0)
            {
                find_pos = souce.Length;
            }

            patten = "";
            if (start_pos > 0)
            {
                //  patten = souce.Substring(0, start_pos - cellSplit.Length);
                patten = souce.Substring(0, start_pos);
            }


            if (find_pos < souce.Length)
            {
                if (start_pos == 0)
                {
                    patten = patten + souce.Substring(find_pos + cellSplit.Length, souce.Length - (find_pos + cellSplit.Length));
                }
                else
                {
                    patten = patten + souce.Substring(find_pos, souce.Length - find_pos);
                }
            }
            return patten;
        }

        //设置字符串中某一项的值
        public static string setvaluebyname(string name, string value, string souce)
        {
            int start_pos = -1;
            int find_pos = -1;

            string patten = name + splitKeyValue;

            if (name.Trim().Length == 0 || souce.Trim().Length == 0)
            {
                throw new ArgumentNullException();
            }

            if (souce.Substring(0, patten.Length) == patten)
            {
                start_pos = 0;
            }
            else
            {
                patten = cellSplit + name + splitKeyValue;
                find_pos = souce.IndexOf(patten, start_pos + 1);
                if (find_pos >= 0)
                {
                    start_pos = find_pos;
                }
            }

            if (start_pos < 0)
            {
                return null;
            }
            start_pos = start_pos + patten.Length;
            find_pos = souce.IndexOf(cellSplit, start_pos);
            if (find_pos < 0)
            {
                find_pos = souce.Length;
            }
            patten = "";
            if (start_pos >= 0)
            {
                patten = souce.Substring(0, start_pos);
            }
            patten = patten + Add_SplitStr(value);

            if (find_pos < souce.Length - 1)
            {
                patten = patten + souce.Substring(find_pos, souce.Length - find_pos);

            }
            return patten;

        }

        //根据名称查找值
        public static string getvaluebyname(string name, string souce)
        {
            int start_pos = -1;
            int find_pos = -1;
            string socue_tag = souce.ToLower();
            name = name.ToLower();
            string patten = name + splitKeyValue;
            if (name.Trim().Length == 0 || souce.Trim().Length == 0)
            {
                return null;
            }

            if (socue_tag.Length < patten.Length)
            {
                return null;
            }

            if (socue_tag.Substring(0, patten.Length) == patten)
            {
                start_pos = 0;
            }
            else
            {
                patten = cellSplit + name + splitKeyValue;
                find_pos = socue_tag.IndexOf(patten, start_pos + 1);
                if (find_pos >= 0)
                {
                    start_pos = find_pos;
                }
            }

            if (start_pos < 0)
            {
                return null;
            }

            find_pos = socue_tag.IndexOf(cellSplit, start_pos + cellSplit.Length);
            if (find_pos < 0)
            {
                find_pos = souce.Length;
            }
            try
            {
                patten = souce.Substring(start_pos + patten.Length, find_pos - start_pos - patten.Length);
            }
            catch
            {
                patten = "";
            }
            return Out_SplitStr(patten);
        }


        //根据名称查找值
        public static bool isExistsname(string name, string souce)
        {
            int start_pos = -1;
            int find_pos = -1;
            bool isExists = true;
            string socue_tag = souce.ToLower();
            name = name.ToLower();
            string patten = name + splitKeyValue;

            if (name.Trim().Length == 0 || souce.Trim().Length == 0)
            {
                isExists = false;
                return isExists;
            }

            if (socue_tag.Length < patten.Length)
            {
                return false;
            }

            if (socue_tag.Substring(0, patten.Length) == patten)
            {
                start_pos = 0;
            }
            else
            {
                patten = cellSplit + name + splitKeyValue;
                find_pos = socue_tag.IndexOf(patten, start_pos + 1);
                if (find_pos >= 0)
                {
                    start_pos = find_pos;
                }
            }

            if (start_pos < 0)
            {
                return false;
            }

            return isExists;
        }


        //将单引号转为两个单引号
        public static string Add_RefString(string instr)
        {
            if (!string.IsNullOrWhiteSpace(instr))
                instr = instr.Replace("'", "''");
            else
                instr = string.Empty;
            return instr;
        }

        /// <summary>
        ///<para>加分隔符</para>
        ///<para>规则如下：</para>
        ///<para>如果值中间包含CellSplit,会在CellSplit的(CellSplit-1)位置插入AddChar</para>
        ///<para>CellSplit为相等的多个字符组成时,如果source以CellSplit的单个字符开始，将在source前加上一个AddChar;如果以CellSplit的单个字符结尾，将在source的结尾加上一个AddChar.</para>
        /// </summary>
        /// <param name="souce"></param>
        /// <returns></returns>
        public static string Add_SplitStr(string souce)
        {
            souce = souce.Replace(addchar, addchar + addchar);//编码转义字符

            souce = souce.Replace(cellSplit.Substring(0, cellSplit.Length - 1), cellSplit.Substring(0, cellSplit.Length - 1) + addchar);
            souce = souce.Replace(rowSplit.Substring(0, rowSplit.Length - 1), rowSplit.Substring(0, rowSplit.Length - 1) + addchar);
            if (IsOverChar(cellSplit))
            {
                if (souce.EndsWith(cellSplit.First().ToString()))
                {
                    souce += addchar;
                }
                if (souce.StartsWith(cellSplit.First().ToString()))
                {
                    souce = addchar + souce;
                }
            }
            return souce;
        }

        //去分隔符
        public static string Out_SplitStr(string souce)
        {
            return Out_SplitStr(souce, false);
        }
        public static string Out_SplitStr(string souce, bool urlDecode)
        {
            souce = souce.Replace(addchar + addchar, addchar);//解码转义字符

            souce = souce.Replace(cellSplit.Substring(0, cellSplit.Length - 1) + addchar, cellSplit.Substring(0, cellSplit.Length - 1));
            souce = souce.Replace(rowSplit.Substring(0, rowSplit.Length - 1) + addchar, rowSplit.Substring(0, rowSplit.Length - 1));
            if (IsOverChar(cellSplit))
            {
                if (souce.EndsWith(cellSplit.First().ToString() + addchar))
                {
                    souce = souce.Substring(0, souce.Length - 1);
                }
                if (souce.StartsWith(addchar + cellSplit.First().ToString()))
                {
                    souce = souce.Substring(1, souce.Length - 1);
                }
            }

            //是否对值进行解码
            if (urlDecode)
            {
                souce = HttpUtility.UrlDecode(souce);
            }

            return souce;
        }
        public static string Out_SpliRowtStr(string souce)
        {
            souce = souce.Replace(rowSplit.Substring(0, rowSplit.Length - 1) + addchar, rowSplit.Substring(0, rowSplit.Length - 1));
            if (IsOverChar(rowSplit))
            {
                if (souce.EndsWith(rowSplit.First().ToString() + addchar))
                {
                    souce = souce.Substring(0, souce.Length - 1);
                }
                if (souce.StartsWith(addchar + rowSplit.First().ToString()))
                {
                    souce = souce.Substring(1, souce.Length - 1);
                }
            }
            souce = HttpUtility.UrlDecode(souce);
            return souce;
        }
        /// <summary>
        /// str中的每个字符是否相等
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>结果</returns>
        private static bool IsOverChar(string str)
        {
            char firstChar = str.First();
            return firstChar.ToString().PadLeft(str.Length, firstChar) == str;
        }

        public static string AppendKeyValue(string souce, string key, string value)
        {
            if (souce.Length == 0 || getvaluebyname(key, souce) == null)
            {
                value = Add_SplitStr(value);
                if (souce.Length == 0)
                {
                    souce = key + splitKeyValue + value;
                }
                else
                {
                    souce = souce + cellSplit + key + splitKeyValue + value;
                }
            }
            else
            {
                souce = setvaluebyname(key, value, souce);
            }
            return souce;

        }

        public static void NewAddLine(ref string source)
        {
            source += rowSplit;
        }
        public static void UpdateFieldValue(ref string source, string field, string value)
        {
            string content = field + splitKeyValue + value;
            var srcValue = getvaluebyname(field, source);
            var srcContent = field + splitKeyValue + srcValue;
            source = source.Replace(srcContent, content);
        }
    }
}
