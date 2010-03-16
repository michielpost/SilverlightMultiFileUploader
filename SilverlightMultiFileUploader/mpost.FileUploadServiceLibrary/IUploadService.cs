using System.ServiceModel;

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
