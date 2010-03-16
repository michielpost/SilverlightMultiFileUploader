using System;
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
