using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
   public class QiniuToken
    {
        private string _uptoken;

        public string uptoken
        {
            get
            {
                return _uptoken;
            }

            set
            {
                _uptoken = value;
            }
        }
    }
}
