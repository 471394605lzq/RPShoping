using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicLibrary
{
   public class ClientDataSouce
    {
        private DataTable dt = null;                                 //记录集
        private List<string> primaryKeys = new List<string>();        //字段
        private Dictionary<string, string> keys = new Dictionary<string, string>();  //其他信息
        public DataTable DT
        {
            set { dt = value; }
            get { return dt; }
        }
        public List<string> PrimaryKeys
        {
            set { primaryKeys = value; }
            get { return primaryKeys; }
        }
        public Dictionary<string, string> Keys
        {
            set { keys = value; }
            get { return keys; }

        }
    }
}
