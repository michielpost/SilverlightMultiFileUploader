using System;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Serialization;

using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.WebPartPages;

using System.ComponentModel;


namespace mPost.MultiFileUploadWebPart
{
    [Guid("5f8ff2b6-e45a-49d8-9762-fcbbea6a79ed")]
    public class MultiFileUpload : System.Web.UI.WebControls.WebParts.WebPart
    {
        public MultiFileUpload()
        {
            
        }

        protected override void OnLoad(EventArgs e)
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(Page);
            if (scriptManager == null)
            {
                scriptManager = new ScriptManager();
                Controls.Add(scriptManager);
            }

        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);


            ClientScriptManager cs = Page.ClientScript;// Include the required javascript file.

            if (!cs.IsClientScriptIncludeRegistered("sl_javascript"))
                cs.RegisterClientScriptInclude(this.GetType(), "sl_javascript", "~/_controltemplates/mPost.MultiFileUpload/Silverlight.js");

            if (!cs.IsClientScriptIncludeRegistered("spsl_javascript"))
                cs.RegisterClientScriptInclude(this.GetType(), "spsl_javascript", "~/_controltemplates/mPost.MultiFileUpload/SpSilverlight.js");
        }



        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            try
            {
                if (string.IsNullOrEmpty(DOCLIB))
                {
                    Controls.Add(new Label() { Text = "Please set the document library settings" });
                    return;
                }

                mPost.MultiFileUploadWebPart.MultiFileUploadUC uc = (mPost.MultiFileUploadWebPart.MultiFileUploadUC)
                    Page.Page.LoadControl("~/_controlTemplates/mPost.MultiFileUpload/MultiFileUploadUC.ascx");
                
                uc.HttpService = HTTPHANDLER;
                uc.DocumentLibrary = DOCLIB;
                uc.Site = SPContext.Current.Web.Url;

                Controls.Add(uc);
            }
            catch (Exception ex)
            {
                Controls.Add(new Label() { Text = ex.Message });
            }
        }

        private string httpHandler = string.Empty;
        [Category("MultiFileUpload"),
        Personalizable(PersonalizationScope.User),
        WebBrowsable(true), WebDisplayName("The adress of http handler, default: http://localhost/_layouts/MULTIFILEUPLOAD/MultiFileUpload.ashx"),
        WebDescription("Http-handler url")]
        public string HTTPHANDLER
        {
            get { return string.IsNullOrEmpty(httpHandler) ? @"http://localhost/_layouts/MULTIFILEUPLOAD/MultiFileUpload.ashx" : httpHandler; }
            set { httpHandler = value; }
        }

        [Category("MultiFileUpload"),
        Personalizable(PersonalizationScope.User),
        WebBrowsable(true), WebDisplayName("The name of the document library"),
        WebDescription("The name of the document library")]
        public string DOCLIB { get; set; }

    }
}
