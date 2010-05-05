using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using Microsoft.SharePoint;

namespace MultiFileUpload
{
    public class WriteFileToSPDocLib
    {
        public string SPSite { get; set; }
        public string SPDocLib { get; set; }
        
        public WriteFileToSPDocLib() { }

        public bool WriteFileToDocLib(string fileToUpload)
        {
            using (SPWeb oWeb = SPContext.Current.Site.OpenWeb())
            {
                oWeb.AllowUnsafeUpdates = true;
                if (System.IO.File.Exists(fileToUpload))
                {
                    SPFolder myLibrary = oWeb.Folders[SPDocLib];

                    bool replaceExistingFiles = true;
                    string fileName = System.IO.Path.GetFileName(fileToUpload);

                    var fileStream = File.OpenRead(fileToUpload); 
                    byte[] content = new byte[fileStream.Length];
                    fileStream.Read(content, 0, (int)fileStream.Length);
                    fileStream.Close();
                    fileStream.Dispose();

                    SPFile spfile = myLibrary.Files.Add(fileName, content, replaceExistingFiles);
                    myLibrary.Update();
                }
                else
                {
                    throw new FileNotFoundException("File not found.", fileToUpload);
                }
            }
            return true;
        }
    }
}
