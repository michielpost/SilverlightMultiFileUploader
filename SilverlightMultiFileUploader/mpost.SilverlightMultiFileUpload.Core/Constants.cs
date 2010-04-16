
/*
 * Copyright Michiel Post
 * http://www.michielpost.nl
 * contact@michielpost.nl
 * */

namespace mpost.SilverlightMultiFileUpload.Core
{
    public static class Constants
    {
        /// <summary>
        /// Possible States
        /// </summary>
        public enum FileStates
        {
            Pending = 0,
            Uploading = 1,
            Finished = 2,
            Deleted = 3,
            Error = 4
        }
    }
}
