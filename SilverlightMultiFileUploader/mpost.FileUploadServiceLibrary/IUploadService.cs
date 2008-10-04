using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

/*
 * Copyright Michiel Post
 * http://www.michielpost.nl
 * contact@michielpost.nl
 * */

namespace mpost.FileUploadServiceLibrary
{
    [ServiceContract]
    public interface IUploadService
    {
        
        [OperationContract]
        void StoreFileAdvanced(string fileName, byte[] data, int dataLength, string parameters, bool firstChunk, bool lastChunk);

        [OperationContract]
        void CancelUpload(string filename);
    }

}
