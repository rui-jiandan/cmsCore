﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinCms.Data.Options
{
    public class SiteOption
    {
        public string Domain { get; set; }
        public string VVLogDomain { get; set; }
        public string CMSDomain { get; set; }
        public string IdentityServer4Domain { get; set; }
        public string Email { get; set; }
        public string BlogUrl { get; set; }
        public string DocUrl { get; set; }

    }
}
