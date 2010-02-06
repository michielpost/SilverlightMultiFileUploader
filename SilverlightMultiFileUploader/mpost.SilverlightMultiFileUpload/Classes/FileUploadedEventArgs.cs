using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Browser;

namespace mpost.SilverlightMultiFileUpload.Classes
{
    [ScriptableType]
    public class FileUploadedEventArgs : EventArgs
    {
        [ScriptableMember()]
        public string FileName { get; set; }
        public double FileSize { get; set; }

        public FileUploadedEventArgs(string fileName, double fileSize)
        {
            FileName = fileName;
            FileSize = fileSize;
        }
    }
}
