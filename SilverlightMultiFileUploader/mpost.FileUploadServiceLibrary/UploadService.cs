using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IO;
using System.Web.Hosting;
using System.ServiceModel.Activation;
using System.Diagnostics;
using System.Configuration;

/*
 * Copyright Michiel Post
 * http://www.michielpost.nl
 * contact@michielpost.nl
 * */

namespace mpost.FileUploadServiceLibrary
{
    [AspNetCompatibilityRequirements  (RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class UploadService : IUploadService
    {
        private string _tempExtension = "_temp";

        StreamWriter _debugFileStreamWriter;
        TextWriterTraceListener _debugListener;
   

        #region IUploadService Members

        /// <summary>
        /// Cancel the upload and delete the TEMP file
        /// </summary>
        /// <param name="fileName"></param>
        public void CancelUpload(string fileName)
        {
            string uploadFolder = GetUploadFolder();
            string tempFileName = fileName + _tempExtension;

            if (File.Exists(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + tempFileName))
                File.Delete(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + tempFileName);

        }

        /// <summary>
        /// Receive a chunk of data and store it on disk
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="data"></param>
        /// <param name="dataLength"></param>
        /// <param name="parameters"></param>
        /// <param name="firstChunk"></param>
        /// <param name="lastChunk"></param>
        public void StoreFileAdvanced(string fileName, byte[] data, int dataLength, string parameters, bool firstChunk, bool lastChunk)
        {
            try
            {
                //StartDebugListener();

                string uploadFolder = GetUploadFolder();
                string tempFileName = fileName + _tempExtension;

                //Is this the first chunk of the file?
                if (firstChunk)
                {
                    Debug.WriteLine("First chunk arrived at webservice");

                    //Delete temp file
                    if (File.Exists(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + tempFileName))
                        File.Delete(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + tempFileName);

                    //Delete target file
                    if (File.Exists(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + fileName))
                        File.Delete(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + fileName);

                }


                Debug.WriteLine(string.Format("Write data to disk FOLDER: {0}", uploadFolder));


                using (FileStream fs = File.Open(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + tempFileName, FileMode.Append))
                {
                    fs.Write(data, 0, dataLength);
                    fs.Close();
                }
               

                Debug.WriteLine("Write data to disk SUCCESS");

                //Finish up if this is the last chunk of the file
                if (lastChunk)
                {
                    Debug.WriteLine("Last chunk arrived");

                    //Rename file to original file
                    File.Move(HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + tempFileName, HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + fileName);

                    //Finish stuff....
                    FinishedFileUpload(fileName, parameters);
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

        }

        /// <summary>
        /// Delete an uploaded file
        /// </summary>
        /// <param name="fileName"></param>
        protected void DeleteUploadedFile(string fileName)
        {
            string uploadFolder = GetUploadFolder();

            if (File.Exists(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + fileName))
                File.Delete(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + fileName);

        }


        /// <summary>
        /// Do your own stuff here when the file is finished uploading
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="parameters"></param>
        protected virtual void FinishedFileUpload(string fileName, string parameters)
        {
        }

        /// <summary>
        /// Get the upload folder from the Web.config
        /// You can overwrite this method and always return your custom upload folder
        /// </summary>
        /// <returns></returns>
        protected virtual string GetUploadFolder()
        {           
            string folder = ConfigurationSettings.AppSettings["UploadFolder"];
            if (string.IsNullOrEmpty(folder))
                folder = "Upload";

            return folder;
        }
        
        /// <summary>
        /// Write debug output to a textfile in debug mode
        /// </summary>
        [Conditional("DEBUG")]
        private void StartDebugListener()
        {
            try
            {
                _debugFileStreamWriter = System.IO.File.AppendText("debug.txt");
                _debugListener = new TextWriterTraceListener(_debugFileStreamWriter);
                Debug.Listeners.Add(_debugListener);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Clean up the debug listener
        /// </summary>
        [Conditional("DEBUG")]
        private void StopDebugListener()
        {
            try
            {
                Debug.Flush();
                _debugFileStreamWriter.Close();
                Debug.Listeners.Remove(_debugListener);

            }
            catch
            {
            }
        }


        #endregion
    }
}
