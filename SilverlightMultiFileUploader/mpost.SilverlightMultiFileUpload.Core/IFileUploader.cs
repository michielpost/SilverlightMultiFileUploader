using System;

/*
 * Copyright Michiel Post
 * http://www.michielpost.nl
 * contact@michielpost.nl
 * */

namespace mpost.SilverlightMultiFileUpload.Core
{
    /// <summary>
    /// Interface for different kind of file uploaders
    /// </summary>
    public interface IFileUploader
    {
        void StartUpload(string initParams);        
        void CancelUpload();

        event EventHandler UploadFinished;
    }
}
