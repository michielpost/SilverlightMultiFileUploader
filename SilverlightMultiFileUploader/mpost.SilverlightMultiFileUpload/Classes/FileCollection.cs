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
using System.Collections.ObjectModel;
using System.ComponentModel;

/*
 * Copyright Michiel Post
 * http://www.michielpost.nl
 * contact@michielpost.nl
 * */

namespace mpost.SilverlightMultiFileUpload.Classes
{
    public class FileCollection : ObservableCollection<UserFile>
    {
        private double _bytesUploaded = 0;
        private int _percentage = 0;
        private int _currentUpload = 0;
        private string _customParams;
        private int _maxUpload;
        

        public double BytesUploaded
        {
            get { return _bytesUploaded; }
            set
            {
                _bytesUploaded = value;

                this.OnPropertyChanged(new PropertyChangedEventArgs("BytesUploaded"));

            }
        }

        public int Percentage
        {
            get { return _percentage; }
            set
            {
                _percentage = value;

                this.OnPropertyChanged(new PropertyChangedEventArgs("Percentage"));
            }
        }

        public FileCollection(string customParams, int maxUploads)
        {
            _customParams = customParams;
            _maxUpload = maxUploads;

            this.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(FileCollection_CollectionChanged);
            
        }

       

        public new void Add(UserFile item)
        {
            //Listen to the property changed for each added item
            item.PropertyChanged += new PropertyChangedEventHandler(item_PropertyChanged);
            
            base.Add(item);
        }

        public void UploadFiles()
        {
            lock (this)
            {
                foreach (UserFile file in this)
                {
                    if (!file.IsDeleted && file.State == Constants.FileStates.Pending && _currentUpload < _maxUpload)
                    {
                        file.Upload(_customParams);
                        _currentUpload++;
                    }
                }
            }

        }

        /// <summary>
        /// Recount statistics
        /// </summary>
        private void RecountTotal()
        {
            //Recount total
            double totalSize = 0;
            double totalSizeDone = 0;

            foreach (UserFile file in this)
            {
                totalSize += file.FileSize;
                totalSizeDone += file.BytesUploaded;
            }

            double percentage = 0;

            if (totalSize > 0)
                percentage = 100 * totalSizeDone / totalSize;

            BytesUploaded = totalSizeDone;

            Percentage = (int)percentage;
        }

        void FileCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //Recount total when the collection changed (items added or deleted)
            RecountTotal();
        }

        /// <summary>
        /// Property of individual item changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //Check if deleted property is changed
            if (e.PropertyName == "IsDeleted")
            {
                UserFile file = (UserFile)sender;

                if (file.IsDeleted)
                {
                    if (file.State == Constants.FileStates.Uploading)
                    {
                        _currentUpload--;
                        UploadFiles();
                    }

                    this.Remove(file);

                    file = null;
                }
            }
            else if (e.PropertyName == "State")
            {
                UserFile file = (UserFile)sender;
                if (file.State == Constants.FileStates.Finished || file.State == Constants.FileStates.Error)
                {
                    _currentUpload--;
                    UploadFiles();
                }
            }
            else if (e.PropertyName == "BytesUploaded")
            {
                RecountTotal();
            }
        }
     
    }
}