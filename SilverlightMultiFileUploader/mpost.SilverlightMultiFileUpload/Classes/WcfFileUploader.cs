﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ServiceModel;
using mpost.SilverlightFramework;
using System.IO;

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
            BasicHttpBinding binding = new BasicHttpBinding();
            EndpointAddress address = new EndpointAddress(new CustomUri("SilverlightUploadService.svc"));
            _client = new UploadService.UploadServiceClient(binding, address);
            //_client = new UploadService.UploadServiceClient();

            //Subscribe to events
            _client.StoreFileAdvancedCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(_client_StoreFileAdvancedCompleted);
            _client.CancelUploadCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(_client_CancelUploadCompleted);
            //_client.InnerChannel.Closed += new EventHandler(InnerChannel_Closed);
            _client.ChannelFactory.Closed += new EventHandler(ChannelFactory_Closed);
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

       
        public event EventHandler UploadFinished;


        public void StartUpload(string initParams)
        {
            _initParams = initParams;

            StartUpload();
        }

        private void StartUpload()
        {
            
            byte[] buffer = new byte[4 * 4096];
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

                //Notify progress change
                OnProgressChanged();
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

        private void OnProgressChanged()
        {
            _file.BytesUploaded = _dataSent;
        }

        private void _client_StoreFileAdvancedCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
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

      

        private void ChannelIsClosed()
        {
            if (!_file.IsDeleted)
            {
                if (UploadFinished != null)
                    UploadFinished(this, null);
            }
        }

        /// <summary>
        /// Cancel the upload and delete the file on the server
        /// </summary>
        public void CancelUpload()
        {
            _client.CancelUploadAsync(_file.FileName);
        }


    }
}