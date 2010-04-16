using System;
using System.Net;
using System.IO;
using mpost.SilverlightFramework;

/*
 * Copyright Michiel Post
 * http://www.michielpost.nl
 * contact@michielpost.nl
 * */

namespace mpost.SilverlightMultiFileUpload.Core
{
    public class HttpFileUploader : IFileUploader
    {
        private UserFile _file;
        private long _dataLength;
        private long _dataSent;
        private string _initParams;
       
        private string UploadUrl; 

        public HttpFileUploader(UserFile file, string httpHandlerName)
        {
            _file = file;

            _dataLength = _file.FileStream.Length;
            _dataSent = 0;

            if(string.IsNullOrEmpty(httpHandlerName))
                httpHandlerName = "HttpUploadHandler.ashx";

            UploadUrl = new CustomUri(httpHandlerName).ToString();
        }
               
        #region IFileUploader Members


        /// <summary>
        /// Start the file upload
        /// </summary>
        /// <param name="initParams"></param>
        public void StartUpload(string initParams)
        {
            _initParams = initParams;

            StartUpload();
        }        

        /// <summary>
        /// Cancel the file upload
        /// </summary>
        public void CancelUpload()
        {
            //Not implemented yet...
        }

        public event EventHandler UploadFinished;

        #endregion

        private void StartUpload()
        {
            long dataToSend = _dataLength - _dataSent;
            bool isLastChunk = dataToSend <= _file.Configuration.ChunkSize;
            bool isFirstChunk = _dataSent == 0;

            UriBuilder httpHandlerUrlBuilder = new UriBuilder(UploadUrl);
            httpHandlerUrlBuilder.Query = string.Format("{5}file={0}&offset={1}&last={2}&first={3}&param={4}", _file.FileName, _dataSent, isLastChunk, isFirstChunk, _initParams, string.IsNullOrEmpty(httpHandlerUrlBuilder.Query) ? "" : httpHandlerUrlBuilder.Query.Remove(0, 1) + "&");

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(httpHandlerUrlBuilder.Uri);
            webRequest.Method = "POST";
            webRequest.BeginGetRequestStream(new AsyncCallback(WriteToStreamCallback), webRequest);


        }

        private void WriteToStreamCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest webRequest = (HttpWebRequest)asynchronousResult.AsyncState;
            Stream requestStream = webRequest.EndGetRequestStream(asynchronousResult);

            byte[] buffer = new Byte[4096];
            int bytesRead = 0;
            int tempTotal = 0;

            //Set the start position
            _file.FileStream.Position = _dataSent;

            long chunkSize = _file.Configuration.ChunkSize;

            //Read the next chunk
            while ((bytesRead = _file.FileStream.Read(buffer, 0, buffer.Length)) != 0 
                && tempTotal + bytesRead <= chunkSize 
                && !_file.IsDeleted 
                && _file.State != Constants.FileStates.Error)
            {
                requestStream.Write(buffer, 0, bytesRead);
                requestStream.Flush();

                _dataSent += bytesRead;
                tempTotal += bytesRead;

                //Notify progress change of data sent
                _file.UIDispatcher.BeginInvoke(delegate()
                {
                    OnUploadProgressChanged();
                });
            }

            //Leave the fileStream OPEN
            //fileStream.Close();

            requestStream.Close();

            //Get the response from the HttpHandler
            webRequest.BeginGetResponse(new AsyncCallback(ReadHttpResponseCallback), webRequest);

        }

        private void ReadHttpResponseCallback(IAsyncResult asynchronousResult)
        {    
            //Check if the response is OK
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)asynchronousResult.AsyncState;
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.EndGetResponse(asynchronousResult);
                StreamReader reader = new StreamReader(webResponse.GetResponseStream());

                string responsestring = reader.ReadToEnd();
                reader.Close();

                //Notify progress change of data successfully processed
                _file.UIDispatcher.BeginInvoke(delegate()
                {
                    OnUploadFinishedProgressChanged();
                });


                if (_dataSent < _dataLength)
                {
                    //Not finished yet, continue uploading
                    if (_file.State != Constants.FileStates.Error)
                        StartUpload();
                }
                else
                {
                    _file.FileStream.Close();
                    _file.FileStream.Dispose();

                    //Finished event
                    _file.UIDispatcher.BeginInvoke(delegate()
                    {
                        if (UploadFinished != null)
                            UploadFinished(this, null);
                    });
                }

            }
            catch
            {
                _file.FileStream.Close();
                _file.FileStream.Dispose();

                _file.UIDispatcher.BeginInvoke(delegate()
               {
                   _file.State = Constants.FileStates.Error;
               });
            }          

        }

        private void OnUploadProgressChanged()
        {
            _file.BytesUploaded = _dataSent;
        }

        private void OnUploadFinishedProgressChanged()
        {
            _file.BytesUploadedFinished = _dataSent;
        }
    }
}
