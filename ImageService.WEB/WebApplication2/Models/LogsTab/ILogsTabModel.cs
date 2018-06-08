﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.WEB.Models
{
    /// <summary>
    /// An interface for logtab model objects.
    /// Specifies that the object should store a list of logs and be able to refresh it.
    /// </summary>
    interface ILogsTabModel
    {
        List<LogData> LogList { get; set; }
        void RefreshButtonPress();
    }
}
