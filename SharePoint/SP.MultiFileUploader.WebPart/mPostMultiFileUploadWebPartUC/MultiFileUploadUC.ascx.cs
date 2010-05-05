using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mPost.MultiFileUploadWebPart
{
    public partial class MultiFileUploadUC : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// The adress of the site
        /// </summary>
        public string Site { get; set; }

        /// <summary>
        /// The adress to the http-handler service
        /// </summary>
        public string HttpService { get; set; }

        /// <summary>
        /// The name of the document library
        /// </summary>
        public string DocumentLibrary { get; set; }
    }
}