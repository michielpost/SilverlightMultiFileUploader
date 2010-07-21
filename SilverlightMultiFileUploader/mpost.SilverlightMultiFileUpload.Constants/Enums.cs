﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace mpost.SilverlightMultiFileUpload.Constants
{
    public static class Enums
    {
        /// <summary>
        /// Possible States
        /// </summary>
        public enum FileStates
        {
            Pending = 0,
            Uploading = 1,
            Finished = 2,
            Deleted = 3,
            Error = 4
        }
    }
}
