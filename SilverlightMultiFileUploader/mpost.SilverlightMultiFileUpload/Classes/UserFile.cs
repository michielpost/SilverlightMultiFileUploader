using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Threading;
using System.Windows.Browser;

/*
 * Copyright Michiel Post
 * http://www.michielpost.nl
 * contact@michielpost.nl
 * */

namespace mpost.SilverlightMultiFileUpload.Classes
{
    public class UserFile : INotifyPropertyChanged
    {
        private string _fileName;
        private bool _isDeleted = false;       
        private Stream _fileStream;
        private Constants.FileStates _state = Constants.FileStates.Pending;
        private double _bytesUploaded = 0;
        private double _bytesUploadedFinished = 0;
        private double _fileSize = 0;
        private int _percentage = 0;
        private int _percentageFinished = 0;
        private IFileUploader _fileUploader;
      

        public Dispatcher UIDispatcher { get; set; }
        public Configuration Configuration { get; set; }
       

        [ScriptableMember()]
        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                NotifyPropertyChanged("FileName");
            }
        }

        public Constants.FileStates State
        {
            get { return _state; }
            set
            {
                _state = value;


                NotifyPropertyChanged("State");
            }
        }

        [ScriptableMember()]
        public string StateString
        {
            get { return _state.ToString(); }
           
        }


        public bool IsDeleted
        {
            get { return _isDeleted; }
            set
            {
                _isDeleted = value;

                if (_isDeleted)
                    CancelUpload();

                NotifyPropertyChanged("IsDeleted");
            }
        }

        public Stream FileStream
        {
            get { return _fileStream; }
            set
            {
                _fileStream = value;

                if (_fileStream != null)
                    _fileSize = _fileStream.Length;
                
                
            }
        }

        public double FileSize
        {
            get {
                return _fileSize;               
            }
        }

        public double BytesUploaded
        {
            get { return _bytesUploaded; }
            set
            {
                _bytesUploaded = value;

                NotifyPropertyChanged("BytesUploaded");

                Percentage = (int)((value * 100) / FileSize);

            }
        }

        public double BytesUploadedFinished
        {
            get { return _bytesUploadedFinished; }
            set
            {
                _bytesUploadedFinished = value;

                NotifyPropertyChanged("BytesUploadedFinished");

                PercentageFinished = (int)((value * 100) / FileSize);

            }
        }

        [ScriptableMember()]
        public int Percentage
        {
            get { return _percentage; }
            set
            {
                _percentage = value;
                NotifyPropertyChanged("Percentage");


            }
        }

        [ScriptableMember()]
        public int PercentageFinished
        {
            get { return _percentageFinished; }
            set
            {
                _percentageFinished = value;
                NotifyPropertyChanged("PercentageFinished");


            }
        }


        public string ErrorMessage { get; set; }
      

        public void Upload(string initParams)
        {
            this.State = Constants.FileStates.Uploading;

            if (Configuration.UseHttpUploader)
                _fileUploader = new HttpFileUploader(this, Configuration.UploadHandlerName);
            else
                _fileUploader = new WcfFileUploader(this);
                
               
            
            _fileUploader.StartUpload(initParams);
            _fileUploader.UploadFinished += new EventHandler(fileUploader_UploadFinished);
            
            
        }

        public void CancelUpload()
        {
            if (_fileUploader != null && this.State == Constants.FileStates.Uploading)
            {
                _fileUploader.CancelUpload();

                //
                //_fileUploader = null;
            }

        }

        private void fileUploader_UploadFinished(object sender, EventArgs e)
        {
            _fileUploader = null;

            this.State = Constants.FileStates.Finished;
        }


        #region INotifyPropertyChanged Members

        private void NotifyPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
