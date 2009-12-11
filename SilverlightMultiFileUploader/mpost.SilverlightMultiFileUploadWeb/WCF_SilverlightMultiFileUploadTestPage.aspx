<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Register Assembly="System.Web.Silverlight" Namespace="System.Web.UI.SilverlightControls"
    TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" style="height: 100%;">
<head runat="server">
    <title>Test Page For mpost.SilverlightMultiFileUpload</title>
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
    <script type="text/javascript">
        var slCtl = null;

        //DO NOT FORGET TO REGISTER THIS FUNCTION WITH THE SILVERIGHT CONTROL
        // OnPluginLoaded="pluginLoaded"
        function pluginLoaded(sender) {
            //IMPORTANT: Make sure this is the same ID as the ID in your <OBJECT tag (<object id="MultiFileUploader" etc)
            slCtl = document.getElementById("MultiFileUploader");

            //Register All Files Finished Uploading event
            slCtl.Content.Files.AllFilesFinished = AllFilesFinished;
            
            //Register single file finished event
            slCtl.Content.Files.SingleFileUploadFinished = SingleFileFinished;
            
            //Register Error occurred during uploading event
            slCtl.Content.Files.ErrorOccurred = ShowErrorDiv;

            //Set your custom parameter using javascript
            //This parameter will be available in the webservice and you can use it for your business logic
            //Or use it to identity the upload to a sinle row in your database
            //slCtl.Content.Files.CustomParams = "custom_id=1"; 
        }
        

        function ShowNumberOfFilesUploaded() {
            if (slCtl != null) {
                alert("Total Files Uploaded: " + slCtl.Content.Files.TotalUploadedFiles);
            }
        }

        function ShowTotalNumberOfFilesSelected() {
            if (slCtl != null) {
                alert("Total Files Selected: " + slCtl.Content.Files.TotalFilesSelected);
            }
        }

        function ShowUploadProgress() {
            if (slCtl != null) {
                alert("Progress: " + slCtl.Content.Files.Percentage);
                }
        }



        //This function is registred in the pluginLoaded function (slCtl.Content.Files.AllFilesFinished = AllFilesFinished;)
        function AllFilesFinished() {
            document.getElementById('AllFinishedDiv').style.display = 'block';
        }

        //This function is registred in the pluginLoaded function (slCtl.Content.Files.SingleFileUploadFinished = SingleFileFinished;)
        function SingleFileFinished() {
            document.getElementById('SingleFileFinishedDiv').style.display = 'block';
        }

        //This function is registred in the pluginLoaded function (slCtl.Content.Files.ErrorOccurred = ShowErrorDiv;)
        function ShowErrorDiv() {
            document.getElementById('ErrorDiv').style.display = 'block';
        }


        //Actions
        function StartUpload() {
            if (slCtl != null) {
                slCtl.Content.Control.StartUpload();
            }
        }

        function ClearList() {
            if (slCtl != null) {
                slCtl.Content.Control.ClearList();
            }
        }

       
        
    </script>

</head>
<body style="height: 400px; margin: 10;">
    <form id="form1" runat="server" style="height: 100%;">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <h1>WCF Webservice Upload</h1>
    
     <div id="silverlightControlHost" >
        <object id="MultiFileUploader" data="data:application/x-silverlight-2," type="application/x-silverlight-2" width="450" height="280">
            <param name="source" value="ClientBin/mpost.SilverlightMultiFileUpload.xap" />
            <param name="onerror" value="onSilverlightError" />
             <param name="initParams" value="HttpUploader=false,MaxFileSizeKB=,MaxUploads=2,FileFilter=,CustomParam=yourparameters,DefaultColor=White" />
            <param name="background" value="white" />
            <param name="onload" value="pluginLoaded" />
             <param name="minRuntimeVersion" value="4.0.41108.0" />
            <param name="autoUpgrade" value="true" />           
			 <a href="http://go.microsoft.com/fwlink/?LinkID=149156&v=4.0.41108.0" style="text-decoration:none">
 			  <img src="http://go.microsoft.com/fwlink/?LinkId=161376" alt="Get Microsoft Silverlight" style="border-style:none"/>
		  </a>
        </object>
        <iframe style='visibility: hidden; height: 0; width: 0; border: 0px'></iframe>
    </div>
 
    </form>
    
    <div id="AllFinishedDiv" style="display:none;">All files finished (javascript triggered).</div>
     <div id="SingleFileFinishedDiv" style="display:none;">Single file upload finished (javascript triggered).</div>
    <div id="ErrorDiv" style="display:none;">Error occurred during upload (javascript triggered).</div>
   
    <button onclick="ShowNumberOfFilesUploaded()">Javascript test: Show Number of files uploaded</button><br />
    <button onclick="ShowTotalNumberOfFilesSelected()">Javascript test: Show total number of files selected</button><br />
    <button onclick="ShowUploadProgress();">Javascript test: Show upload progress</button><br />
      
      <br />
      <br />
      <button onclick="StartUpload();">Start Upload</button><br />
       <button onclick="ClearList();">Clear List</button><br />
    
</body>
</html>
