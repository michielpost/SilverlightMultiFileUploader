using System.Collections.Generic;
using mpost.SilverlightFramework;

namespace mpost.SilverlightMultiFileUpload.Classes
{
    public class Configuration
    {

        public string CustomParams { get; set; }
        public string FileFilter { get; set; }
        public string UploadHandlerName { get; set; }

        public int MaxUploads { get; set; }
        public int MaxFileSize { get; set; }

        public long ChunkSize { get; set; }
        public long WcfChunkSize { get; set; }

        public bool UseHttpUploader { get; set; }

        private int _testInt;
        private bool _testBool;
        private long _testLong;
       

        /// <summary>
        /// Load configuration first from initParams, then from .Config file
        /// </summary>
        /// <param name="initParams"></param>
        public Configuration(IDictionary<string, string> initParams)
        {
            //Defaults:
            MaxUploads = 2;
            UseHttpUploader = true;
            ChunkSize = 1024 * 4096;
            WcfChunkSize = 16 * 1024;

            //Load settings from Init Params (if available)
            LoadFromInitParams(initParams);


            //Overwrite initParams using settings from .config file
            LoadFromConfigFile();

        }

        /// <summary>
        ///  Load settings from Init Params (if available)
        /// </summary>
        /// <param name="initParams"></param>
        private void LoadFromInitParams(IDictionary<string, string> initParams)
        {
            //Load Custom Config String
            if (initParams.ContainsKey("CustomParam") && !string.IsNullOrEmpty(initParams["CustomParam"]))
                CustomParams = initParams["CustomParam"];

            if (initParams.ContainsKey("MaxUploads") && !string.IsNullOrEmpty(initParams["MaxUploads"]))
            {
                if (int.TryParse(initParams["MaxUploads"], out _testInt))
                    MaxUploads = int.Parse(initParams["MaxUploads"]);
            }

            if (initParams.ContainsKey("MaxFileSizeKB") && !string.IsNullOrEmpty(initParams["MaxFileSizeKB"]))
            {
                if (int.TryParse(initParams["MaxFileSizeKB"], out _testInt))
                    MaxFileSize = int.Parse(initParams["MaxFileSizeKB"]) * 1024;
            }

            if (initParams.ContainsKey("ChunkSize") && !string.IsNullOrEmpty(initParams["ChunkSize"]))
            {
                if (long.TryParse(initParams["ChunkSize"], out _testLong))
                {
                    //Minimum Chunksize is 4096 bytes
                    if(_testLong > 4096)
                        ChunkSize = int.Parse(initParams["ChunkSize"]);
                }
            }

            if (initParams.ContainsKey("FileFilter") && !string.IsNullOrEmpty(initParams["FileFilter"]))
                FileFilter = initParams["FileFilter"];

            if (initParams.ContainsKey("HttpUploader") && !string.IsNullOrEmpty(initParams["HttpUploader"]))
                if (bool.TryParse(initParams["HttpUploader"], out _testBool))
                    UseHttpUploader = bool.Parse(initParams["HttpUploader"]);

            if (initParams.ContainsKey("UploadHandlerName") && !string.IsNullOrEmpty(initParams["UploadHandlerName"]))
                UploadHandlerName = initParams["UploadHandlerName"];
        }

        /// <summary>
        /// Load settings from .config file
        /// </summary>
        private void LoadFromConfigFile()
        {
            
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MaxFileSizeKB"]))
            {
                if (int.TryParse(ConfigurationManager.AppSettings["MaxFileSizeKB"], out _testInt))
                {
                    MaxFileSize = int.Parse(ConfigurationManager.AppSettings["MaxFileSizeKB"]) * 1024;
                }
            }


            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MaxUploads"]))
            {
                if (int.TryParse(ConfigurationManager.AppSettings["MaxUploads"], out _testInt))
                {
                    MaxUploads = int.Parse(ConfigurationManager.AppSettings["MaxUploads"]);
                }
            }

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["FileFilter"]))
                FileFilter = ConfigurationManager.AppSettings["FileFilter"];
        }

    }
}
