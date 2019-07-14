using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business.Web.Models
{
   public class NhapSLModel
    {
        public int cspId { get; set; }
        public string ProName { get; set; }
        public string SizeName { get; set; }
        public string ColorName { get; set; }
        public int TC { get; set; }
        public int LK_TC { get; set; }
        public double DinhMuc { get; set; }
        public int KCS { get; set; }
        public int LK_KCS { get; set; }
        public int ERR { get; set; }
    }
}
