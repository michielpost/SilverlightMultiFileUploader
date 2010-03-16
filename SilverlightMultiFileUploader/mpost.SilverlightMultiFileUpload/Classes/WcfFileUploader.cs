using System;
using System.ServiceModel;
using mpost.SilverlightFramework;


/*
 * Copyright Michiel Post
 * http://www.michielpost.nl
 * contact@michielpost.nl
 * */

namespace mpost.SilverlightMultiFileUpload.Classes
{
    public class WcfFileUploader : IFileUploader
    {
        private UserFile _file;
        private long _dataLength;
        private long _dataSent;
        private UploadService.UploadServiceClient _client;
        private string _initParams;
        private bool _firstChunk = true;
        private bool _lastChunk = false;
        

        public WcfFileUploader(UserFile file)
        {
            _file = file;

            _dataLength = _file.FileStream.Length;
            _dataSent = 0;

            //Create WCF connection
            //BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            //BasicHttpBinding binding = new BasicHttpBinding();
                        
            EndpointAddress address = new EndpointAddress(new CustomUri("SilverlightUploadService.svc"));
            _client = new UploadService.UploadServiceClient("CustomBinding_IUploadService", address);

            //_client = new UploadService.UploadServiceClient(binding, address);
            
            //Enable this line for HTTPS (and disable the above 3 lines)
            //_client = new UploadService.UploadServiceClient();

            //Subscribe to events
            //_client.StoreFileAdvancedCompleted += new EventHandler<mpost.SilverlightMultiFileUpload.UploadService.StoreFileAdvancedCompletedEventArgs>(_client_StoreFileAdvancedCompleted);
            _client.StoreFileAdvancedCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(_client_StoreFileAdvancedCompleted);
            _client.CancelUploadCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(_client_CancelUploadCompleted);
            //_client.InnerChannel.Closed += new EventHandler(InnerChannel_Closed);
            _client.ChannelFactory.Closed += new EventHandler(ChannelFactory_Closed);
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
        /// Cancel the upload and delete the file on the server
        /// </summary>
        public void CancelUpload()
        {
            _client.CancelUploadAsync(_file.FileName);
        }

        public event EventHandler UploadFinished;

        #endregion

        private void StartUpload()
        {

            byte[] buffer = new byte[_file.Configuration.WcfChunkSize];
            int bytesRead = _file.FileStream.Read(buffer, 0, buffer.Length);

            //Are there any bytes left?
            if (bytesRead != 0)
            {
                _dataSent += bytesRead;

                if (_dataSent == _dataLength)
                    _lastChunk = true;

                //Upload this chunk
                _client.StoreFileAdvancedAsync(_file.FileName, buffer, bytesRead, _initParams, _firstChunk, _lastChunk);


                //Always false after the first message
                _firstChunk = false;

            }
            else
            {
                //Finished
                _file.FileStream.Dispose();
                _file.FileStream.Close();

                //Close
                _client.ChannelFactory.Close();

            }

        }

               

        private void _client_StoreFileAdvancedCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            //Notify progress change
            OnProgressChanged();

            //Check for webservice errors
            if (e.Error != null)
            {
                //Abort upload on error
                _file.State = Constants.FileStates.Error;              

            }            
            else
            {
                //Continue with uploading if the Delete Button isn't pushed
                if (!_file.IsDeleted)
                    StartUpload();
            }
        }
        

        private void OnProgressChanged()
        {
            _file.BytesUploaded = _dataSent;
        }


        private void ChannelFactory_Closed(object sender, EventArgs e)
        {
            ChannelIsClosed();
        }

        private void _client_CancelUploadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            //Close channel after cancel is complete
            _client.ChannelFactory.Close();
            
        }      

        private void ChannelIsClosed()
        {
            if (!_file.IsDeleted)
            {
                if (UploadFinished != null)
                    UploadFinished(this, null);
            }
        }

    }
}
