﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace mpost.SilverlightSingleFileUpload {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class UserMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal UserMessages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("mpost.SilverlightSingleFileUpload.UserMessages", typeof(UserMessages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Wrong file filter configuration..
        /// </summary>
        public static string ErrorFileFilterConfig {
            get {
                return ResourceManager.GetString("ErrorFileFilterConfig", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No files to upload. Please select one or more files first..
        /// </summary>
        public static string ErrorNoFilesSelected {
            get {
                return ResourceManager.GetString("ErrorNoFilesSelected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Upload failed.
        /// </summary>
        public static string FileRowControlUploadFailed {
            get {
                return ResourceManager.GetString("FileRowControlUploadFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Maximum file size is: .
        /// </summary>
        public static string MaxFileSize {
            get {
                return ResourceManager.GetString("MaxFileSize", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Select File.
        /// </summary>
        public static string SelectFiles {
            get {
                return ResourceManager.GetString("SelectFiles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Upload.
        /// </summary>
        public static string Upload {
            get {
                return ResourceManager.GetString("Upload", resourceCulture);
            }
        }
    }
}
