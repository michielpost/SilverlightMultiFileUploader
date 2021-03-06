﻿<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >

<head>
    <title>mpost.SilverlightMultiFileUpload.Lite</title>
    <style type="text/css">
    html, body {
	    
	    overflow: auto;
    }
    body {
	    padding: 0;
	    margin: 0;
    }
    #silverlightControlHost {
	   
	    
    }
    </style>
   

  <script type="text/javascript">
      var slCtl = null;

      //DO NOT FORGET TO REGISTER THIS FUNCTION WITH THE SILVERIGHT CONTROL
      // <param name="onload" value="pluginLoaded" />
      function pluginLoaded(sender) {

          //IMPORTANT: Make sure this is the same ID as the ID in your <OBJECT tag (<object id="MultiFileUploader" etc)
          slCtl = document.getElementById("MultiFileUploader");


          //Register All Files Finished Uploading event
          slCtl.Content.Files.AllFilesFinished = AllFilesFinished;

          //Register single file finished event
          slCtl.Content.Files.SingleFileUploadFinished = SingleFileFinished;

          //Register Error occurred during uploading event
          slCtl.Content.Files.ErrorOccurred = ShowErrorDiv;

          //Register MaximumFileSizeReached during selecting files
          slCtl.Content.Control.MaximumFileSizeReached = ShowMaximumFileSizeDiv;

          slCtl.Content.Files.FileAdded = UpdateFileList;
          slCtl.Content.Files.FileRemoved = UpdateFileList;
          slCtl.Content.Files.StateChanged = UpdateFileList;

          slCtl.Content.Files.TotalPercentageChanged = UpdateTotalPercentage;

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

      function ShowMaximumFileSizeDiv() {
          document.getElementById('MaximumFileSizeDiv').style.display = 'block';
      }

      //Draws a list of files with the state of each file
      function UpdateFileList() {
          var list = "<table><tr><td>Name</td><td>Size</td><td>Status</td><td></td></tr>";
          var userFile;

          var i = 0;
          for (i = 0; i < slCtl.Content.Files.FileList.length; i++) {
              userFile = slCtl.Content.Files.FileList[i];
              list += "<tr><td>" + userFile.FileName + "</td><td>" + userFile.FileSize + "</td><td>" + userFile.StateString + "</td><td></td></tr>";
          }

          list += "</table>"

          document.getElementById('FileList').innerHTML = list;

          //Update the other statistics...
          UpdateTotalSelected();
          UpdateTotalPercentage();
          UpdateTotalUploaded();
      }

      //Updates the number of total files div element
      function UpdateTotalSelected() {
          document.getElementById('TotalSelected').innerHTML = slCtl.Content.Files.TotalFilesSelected + " files selected";
      }

      //Updates the total percentage div element
      function UpdateTotalPercentage() {
          document.getElementById('TotalPercentage').innerHTML = slCtl.Content.Files.Percentage *100 + "% uploaded";
      }

      //Updates the number of uploaded files div element
      function UpdateTotalUploaded() {
          document.getElementById('TotalUploaded').innerHTML = slCtl.Content.Files.TotalUploadedFiles + " files uploaded";
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

      function RemoveFile(index) {
          if (slCtl != null) {
              slCtl.Content.Control.RemoveAt(index);
          }
      }
        
    </script>

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
</head>
<body>
  
   <h1>Lite test page</h1>
    Only this button is a Silverlight control:
    <div id="silverlightControlHost">
        <object id="MultiFileUploader" data="data:application/x-silverlight-2," type="application/x-silverlight-2"
            width="100" height="25">
            <param name="source" value="ClientBin/mpost.SilverlightMultiFileUpload.Lite.xap" />
            <param name="onError" value="onSilverlightError" />
            <param name="initParams" value="MaxFileSizeKB=,MaxUploads=2,FileFilter=,ChunkSize=4194304,CustomParams=yourparameters,DefaultColor=White" />
            <param name="background" value="white" />
            <param name="onload" value="pluginLoaded" />
            <param name="minRuntimeVersion" value="5.0.61118.0" />
            <param name="autoUpgrade" value="true" />
            <a href="http://go.microsoft.com/fwlink/?LinkID=149156&v=5.0.61118.0" style="text-decoration: none">
                <img src="http://go.microsoft.com/fwlink/?LinkId=161376" alt="Get Microsoft Silverlight"
                    style="border-style: none" />
            </a>
        </object>
        <iframe id="_sl_historyFrame" style="visibility: hidden; height: 0px; width: 0px;
            border: 0px"></iframe>
    </div>

    (you can also drag and drop files on this button)
    <br /> <br />
    <h2>Optional progress bar Silverlight control:</h2>
    You can leave this out if you don't need/want it.
     <div id="Div1">
        <object id="Object1" data="data:application/x-silverlight-2," type="application/x-silverlight-2" width="180" height="20">
		  <param name="source" value="ClientBin/mpost.SilverlightMultiFileUpload.Progress.xap"/>
		        <param name="background" value="white" />         
		  <param name="minRuntimeVersion" value="4.0.50401.0" />
		  <param name="autoUpgrade" value="true" />
		  <a href="http://go.microsoft.com/fwlink/?LinkID=149156&v=4.0.50401.0" style="text-decoration:none">
 			  <img src="http://go.microsoft.com/fwlink/?LinkId=161376" alt="Get Microsoft Silverlight" style="border-style:none"/>
		  </a>
	    </object><iframe id="Iframe1" style="visibility:hidden;height:0px;width:0px;border:0px"></iframe></div>
            <br /> 
    <hr />
    Plain HTML and JavaScript:
     <div id="AllFinishedDiv" style="display: none;">
        All files finished (javascript triggered).</div>
    <div id="SingleFileFinishedDiv" style="display: none;">
        Single file upload finished (javascript triggered).</div>
    <div id="ErrorDiv" style="display: none;">
        Error occurred during upload (javascript triggered).</div>
    <div id="MaximumFileSizeDiv" style="display: none;">
        Selected file is bigger than maximum file size.</div>
    <br />
    <br />   
    <button onclick="StartUpload();">Start Upload</button> 
    <button onclick="ClearList();">Clear List</button><br />
    <button onclick="RemoveFile(1);">Remove only 2nd file</button><br />
    <hr />
    <div id="FileList">    </div>
    <hr />
    <div id="TotalSelected">    </div>
    <div id="TotalPercentage">    </div>
    <div id="TotalUploaded">    </div>
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <button onclick="ShowNumberOfFilesUploaded()">Javascript test: Show Number of files uploaded</button><br />
    <button onclick="ShowTotalNumberOfFilesSelected()">Javascript test: Show total number of files selected</button><br />
    <button onclick="ShowUploadProgress();">Javascript test: Show upload progress</button><br />
   
</body>
</html>
