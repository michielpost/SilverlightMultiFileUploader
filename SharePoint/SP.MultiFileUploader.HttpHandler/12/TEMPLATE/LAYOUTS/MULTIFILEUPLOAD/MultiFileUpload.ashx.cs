using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Web.Hosting;
using System.Diagnostics;
using System.IO;

using Microsoft.SharePoint;

namespace MultiFileUpload
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    ///// </summary>
    //[WebService(Namespace = "http://tempuri.org/")]
    //[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class MultiFileUpload : IHttpHandler
    {

        private HttpContext _httpContext;
        private string _tempExtension = "_temp";
        private string _fileName;
        private string _parameters;
        private bool _lastChunk;
        private bool _firstChunk;
        private long _startByte;

        private string uploadFolder = string.Empty;
        private string spSite = string.Empty;
        private string docLib = string.Empty;

        public void ProcessRequest(HttpContext context)
        {
            _httpContext = context;


            if (context.Request.InputStream.Length == 0)
                throw new ArgumentException("No file input");

            try
            {
                //StartDebugListener();

                GetQueryStringParameters();

                Dictionary<string, string> cParams = CreateCustomParametersInDictionary(_parameters);

                spSite = cParams["site"];
                docLib = cParams["docLib"];

                uploadFolder = GetUploadFolder();
                string tempFileName = _fileName + _tempExtension;

                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    if (!Directory.Exists(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder))
                        Directory.CreateDirectory(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder);
                }
                );

                //Is it the first chunk? Prepare by deleting any existing files with the same name
                if (_firstChunk)
                {
                    Debug.WriteLine("First chunk arrived at webservice");

                    SPSecurity.RunWithElevatedPrivileges(delegate()
                    {
                        //Delete temp file
                        if (File.Exists(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + tempFileName))
                            File.Delete(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + tempFileName);

                        //Delete target file
                        if (File.Exists(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + _fileName))
                            File.Delete(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + _fileName);
                    }
                    );
                }

                using (FileStream fs = File.Open(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + tempFileName, FileMode.Append))
                {
                    SaveFile(context.Request.InputStream, fs);
                    fs.Close();
                }

                Debug.WriteLine("Write data to disk SUCCESS");

                //Is it the last chunk? Then finish up...
                if (_lastChunk)
                {
                    Debug.WriteLine("Last chunk arrived");

                    //Rename file to original file
                    SPSecurity.RunWithElevatedPrivileges(delegate()
                    {
                        File.Move(@HostingEnvironment.ApplicationPhysicalPath + @"\" + uploadFolder + @"\" + tempFileName, @HostingEnvironment.ApplicationPhysicalPath + @"\" + uploadFolder + @"\" + _fileName);
                    }
                    );
                    //Finish stuff....
                    FinishedFileUpload(_fileName, _parameters);

                    //delete file in upload folder
                    SPSecurity.RunWithElevatedPrivileges(delegate()
                    {
                        File.Delete(@HostingEnvironment.ApplicationPhysicalPath + @"\" + uploadFolder + @"\" + _fileName);
                    }
                    );
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());

                throw;
            }
            finally
            {
                //StopDebugListener();
            }

            context.Response.ContentType = "text/plain";
            context.Response.Write("Upload complete");
        }

        private Dictionary<string, string> CreateCustomParametersInDictionary(string _parameters)
        {
            string[] customParams = _parameters.Replace("[", "").Replace("]", "").Split(new char[] { ';' });

            Dictionary<string, string> dictionaryCustomParams = new Dictionary<string, string>(customParams.Length);

            foreach (var item in customParams)
            {
                string[] singleParams = item.Split(new char[] { '=' });

                if (!dictionaryCustomParams.ContainsKey(singleParams[0]))
                    dictionaryCustomParams.Add(singleParams[0], singleParams[1]);
            }

            return dictionaryCustomParams;
        }

        /// <summary>
        /// Get the querystring parameters
        /// </summary>
        private void GetQueryStringParameters()
        {
            _fileName = _httpContext.Request.QueryString["file"];
            _parameters = _httpContext.Request.QueryString["param"];
            _lastChunk = string.IsNullOrEmpty(_httpContext.Request.QueryString["last"]) ? true : bool.Parse(_httpContext.Request.QueryString["last"]);
            _firstChunk = string.IsNullOrEmpty(_httpContext.Request.QueryString["first"]) ? true : bool.Parse(_httpContext.Request.QueryString["first"]);
            _startByte = string.IsNullOrEmpty(_httpContext.Request.QueryString["offset"]) ? 0 : long.Parse(_httpContext.Request.QueryString["offset"]); ;
        }

        protected virtual void FinishedFileUpload(string fileName, string parameters)
        {
            WriteFileToSPDocLib writeFileToSharePoint = new WriteFileToSPDocLib { SPDocLib = docLib, SPSite = spSite};
            writeFileToSharePoint.WriteFileToDocLib(HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + _fileName);
        }


        protected virtual string GetUploadFolder()
        {
            string folder = System.Configuration.ConfigurationSettings.AppSettings["UploadFolder"];
            if (string.IsNullOrEmpty(folder))
                folder = "MultiFileUploadTemp";

            return folder;
        }

        private void SaveFile(Stream stream, FileStream fs)
        {
            byte[] buffer = new byte[4096];
            int bytesRead;
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                fs.Write(buffer, 0, bytesRead);
            }
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
