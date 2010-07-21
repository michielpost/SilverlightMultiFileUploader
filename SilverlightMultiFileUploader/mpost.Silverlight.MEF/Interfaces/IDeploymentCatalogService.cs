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
using System.ComponentModel;

namespace mpost.Silverlight.MEF.Interfaces
{
    //Thanks to: http://codebetter.com/blogs/glenn.block/archive/2010/03/07/building-hello-mef-part-iv-deploymentcatalog.aspx
    
    public interface IDeploymentCatalogService
    {
        void AddXap(string uri, Action<AsyncCompletedEventArgs> completedAction = null);
        void RemoveXap(string uri);
    }
}
