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

namespace mpost.SilverlightMultiFileUpload.Classes
{
    public interface IFileUploader
    {
        void StartUpload(string initParams);        
        void CancelUpload();

        event EventHandler UploadFinished;
    }
}
