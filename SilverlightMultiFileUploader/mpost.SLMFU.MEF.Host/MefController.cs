using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel.Composition;
using mpost.SilverlightMultiFileUpload.Contracts;
using mpost.Silverlight.MEF.Interfaces;
using mpost.SilverlightMultiFileUpload;

namespace mpost.SLMFU.MEF.Host
{
    public class MefController : IPartImportsSatisfiedNotification
    {
        [Import(AllowRecomposition = true, AllowDefault=true)]
        public Lazy<IVisualizeFileRow> VisualizeFileRowControl { get; set; }

        [Import]
        public IDeploymentCatalogService CatalogService { get; set; }

        public UIElement RootVisual { get; set; }

        private Page _mainPage;

        public MefController()
        {
            CompositionInitializer.SatisfyImports(this);

            _mainPage = new Page();
            RootVisual = _mainPage;

            CatalogService.AddXap("mpost.SLMFU.MEF.Plugins.Thumbnails.xap");
        }


        public void OnImportsSatisfied()
        {
            //Create the template to show individual file rows
            if (VisualizeFileRowControl != null)
            {
                _mainPage.SetRowTemplate(VisualizeFileRowControl.Value.GetType());
            }

        }
    }
}
