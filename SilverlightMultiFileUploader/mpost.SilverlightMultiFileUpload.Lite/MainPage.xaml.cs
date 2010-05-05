using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using mpost.SilverlightMultiFileUpload.Core;
using System.Windows.Browser;
using System.IO;
using System.Windows.Messaging;

/*
* Copyright Michiel Post
* http://www.michielpost.nl
* contact@michielpost.nl
*
* http://www.codeplex.com/SLFileUpload/
*
* */

namespace mpost.SilverlightMultiFileUpload.Lite
{
    public partial class MainPage : UserControl
    {
        private FileCollection _files;
        private Configuration _configuration;
        private LocalMessageSender _localSender;
        
        public MainPage(IDictionary<string, string> initParams)
        {            
            InitializeComponent();

            _localSender = new LocalMessageSender("SLMFU");
            _configuration = new Configuration(initParams);

            _files = new FileCollection(_configuration.CustomParams, _configuration.MaxUploads);
            _files.TotalPercentageChanged += new EventHandler(_files_TotalPercentageChanged);

            HtmlPage.RegisterScriptableObject("Files", _files);
            HtmlPage.RegisterScriptableObject("Control", this);               

        }

        void _files_TotalPercentageChanged(object sender, EventArgs e)
        {
            _localSender.SendAsync(_files.Percentage.ToString());
        }

      

        ///////////////////////////////////////////////////////////
        //Scriptable members to control functions via javascript

        [ScriptableMember]
        public void StartUpload()
        {
            UploadFiles();
        }

        [ScriptableMember]
        public void ClearList()
        {
            ClearFilesList();
        }        

        [ScriptableMember()]
        public event EventHandler MaximumFileSizeReached;

        ///////////////////////////////////////////////////////////
     


        /// <summary>
        /// Select files button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectFilesButton_Click(object sender, RoutedEventArgs e)
        {
            SelectUserFiles();
        }

        /// <summary>
        /// Drag and drop of files is supported
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LayoutRoot_Drop(object sender, DragEventArgs e)
        {
            FileInfo[] files = (FileInfo[])e.Data.GetData(System.Windows.DataFormats.FileDrop);  
           
            foreach (FileInfo file in files)
            {
                AddFile(file);
            }
            
        }

        /// <summary>
        /// Open the select file dialog
        /// </summary>
        private void SelectUserFiles()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;

            try
            {
                //Check the file filter (filter is used to filter file extensions to select, for example only .jpg files)
                if (!string.IsNullOrEmpty(_configuration.FileFilter))
                    ofd.Filter = _configuration.FileFilter;
            }
            catch (ArgumentException ex)
            {
                //User supplied a wrong configuration file
                throw new Exception(UserMessages.ErrorFileFilterConfig, ex);
            }

            if (ofd.ShowDialog() == true)
            {
                foreach (FileInfo file in ofd.Files)
                {
                    AddFile(file);
                }
            }
        }
        
       
        /// <summary>
        /// Start uploading files
        /// </summary>
        private void UploadFiles()
        {
            if (_files.Count > 0)
            {
                //Tell the collection to start uploading
                _files.UploadFiles();
            }            
        }       
       

        /// <summary>
        /// Clear the file list
        /// </summary>
        private void ClearFilesList()
        {
            _files.Clear();
        }

        private void AddFile(FileInfo file)
        {
            string fileName = file.Name;

            //Create a new UserFile object
            UserFile userFile = new UserFile();
            userFile.FileName = file.Name;
            userFile.FileStream = file.OpenRead();
            userFile.UIDispatcher = this.Dispatcher;
            userFile.Configuration = _configuration;
            

            //Check for the file size limit (configurable)
            if (userFile.FileStream.Length <= _configuration.MaxFileSize)
            {
                //Add to the list
                _files.Add(userFile);
            }
            else
            {
                //MessageChildWindow messageWindow = new MessageChildWindow();
                //messageWindow.Message = UserMessages.MaxFileSize + (_maxFileSize / 1024).ToString() + "KB.";
                //messageWindow.Show();

                if (MaximumFileSizeReached != null)
                    MaximumFileSizeReached(this, null);

            }
        }
    }
}
