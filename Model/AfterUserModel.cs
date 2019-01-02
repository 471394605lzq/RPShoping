using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class AfterUserModel
    {
        public AfterUserModel() { }
        /// <summary>
        /// 后台用户id
        /// </summary>
        private int aU_ID;
        public int AU_ID
        {
            get
            {
                return aU_ID;
            }

            set
            {
                aU_ID = value;
            }
        }

        /// <summary>
        /// 角色id
        /// </summary>
        private int r_ID;
        public int R_ID
        {
            get
            {
                return r_ID;
            }

            set
            {
                r_ID = value;
            }
        }
        /// <summary>
        /// 用户账号
        /// </summary>
        private string aU_UserAccount;
        public string AU_UserAccount
        {
            get
            {
                return aU_UserAccount;
            }

            set
            {
                aU_UserAccount = value;
            }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        private string aU_Name;
        public string AU_Name
        {
            get
            {
                return aU_Name;
            }

            set
            {
                aU_Name = value;
            }
        }

        /// <summary>
        /// 备注
        /// </summary>
        private string aU_Remark;
        public string AU_Remark
        {
            get
            {
                return aU_Remark;
            }

            set
            {
                aU_Remark = value;
            }
        }
    }
}
