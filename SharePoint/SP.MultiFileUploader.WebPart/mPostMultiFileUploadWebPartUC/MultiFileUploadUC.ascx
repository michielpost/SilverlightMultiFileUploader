﻿<%@ Control Language="C#" AutoEventWireup="true" Inherits="mPost.MultiFileUploadWebPart.MultiFileUploadUC, mPost.MultiFileUploadWebPart, Culture=neutral, Version=1.0.0.0, PublicKeyToken=ccfef14e5dd8b6ea" %>
 <script type="text/javascript" src="Silverlight.js"></script>
    <script type="text/javascript">
        function onSilverlightError(sender, args) {
            var appSource = "";
            if (sender != null && sender != 0) {
                appSource = sender.getHost().Source;
            }

            var errorType = args.ErrorType;
            var iErrorCode = args.ErrorCode;

            if (errorType == "ImageError" || errorType == "MediaError") {
                return;
            }

            var errMsg = "Unhandled Error in Silverlight Application " + appSource + "\n";

            errMsg += "Code: " + iErrorCode + "    \n";
            errMsg += "Category: " + errorType + "       \n";
            errMsg += "Message: " + args.ErrorMessage + "     \n";

            if (errorType == "ParserError") {
                errMsg += "File: " + args.xamlFile + "     \n";
                errMsg += "Line: " + args.lineNumber + "     \n";
                errMsg += "Position: " + args.charPosition + "     \n";
            }
            else if (errorType == "RuntimeError") {
                if (args.lineNumber != 0) {
                    errMsg += "Line: " + args.lineNumber + "     \n";
                    errMsg += "Position: " + args.charPosition + "     \n";
                }
                errMsg += "MethodName: " + args.methodName + "     \n";
            }

            throw new Error(errMsg);
        }
    </script>

<div id="silverlightMultiFileUploadControlHost" >
        <object id="MultiFileUploader" data="data:application/x-silverlight-2," type="application/x-silverlight-2" width="450" height="280">
            <param name="source" value="/_controltemplates/mPost.MultiFileUpload/mpost.SilverlightMultiFileUpload.xap" />
            <param name="onerror" value="onSilverlightError" />
             <param name="initParams" value="MaxFileSizeKB=,MaxUploads=2,FileFilter=,ChunkSize=4194304,CustomParam=[site=<%=Site%>;docLib=<%=DocumentLibrary%>;httpService=<%=HttpService%>],DefaultColor=White" />
            <param name="background" value="white" />
            <param name="onload" value="pluginLoaded" />
             <param name="minRuntimeVersion" value="4.0.50401.0" />
		  <param name="autoUpgrade" value="true" />
		  <a href="http://go.microsoft.com/fwlink/?LinkID=149156&v=4.0.50401.0" style="text-decoration:none">
 			  <img src="http://go.microsoft.com/fwlink/?LinkId=161376" alt="Get Microsoft Silverlight" style="border-style:none"/>
		  </a>
        </object>
        <iframe style='visibility: hidden; height: 0; width: 0; border: 0px'></iframe>
    </div>