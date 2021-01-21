﻿using OnlineStore.Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace OnlineStore.Core
{
    public class ConfigurationHelper : IConfigurationHelper
    {
        public int DefaultProductListCount { get { return Convert.ToInt32(ConfigurationManager.AppSettings["DefaultProductListCount"]); } }
    }
}
