<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Register Assembly="System.Web.Silverlight" Namespace="System.Web.UI.SilverlightControls"
    TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" style="height: 100%;">
<head runat="server">
    <title>Test Page For mpost.SilverlightMultiFileUpload</title>

    <script type="text/javascript">
        var slCtl = null;

        //DO NOT FORGET TO REGISTER THIS FUNCTION WITH THE SILVERIGHT CONTROL
        // OnPluginLoaded="pluginLoaded"
        function pluginLoaded(sender) {
            slCtl = sender.get_element();

            //Register All Files Finished Uploading event
            slCtl.Content.Files.AllFilesFinished = AllFilesFinished;
            
            //Register single file finished event
            slCtl.Content.Files.SingleFileUploadFinished = SingleFileFinished;
            
            //Register Error occurred during uploading event
            slCtl.Content.Files.ErrorOccurred = ShowErrorDiv;
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

        function SelectFiles() {
            if (slCtl != null) {
                slCtl.Content.Control.SelectFiles();
            }
        }

        
    </script>

</head>
<body style="height: 400px; margin: 0;">
    <form id="form1" runat="server" style="height: 100%;">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="height: 80%;">
        <asp:Silverlight ID="Xaml1" runat="server" Source="~/ClientBin/mpost.SilverlightMultiFileUpload.xap"
            MinimumVersion="2.0.31005.0" Width="415" Height="280" OnPluginLoaded="pluginLoaded" InitParameters="MaxFileSizeKB=,MaxUploads=2,FileFilter=,CustomParam=yourparameters,DefaultColor=LightBlue" />
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
      <button onclick="SelectFiles();">Select Files</button><br />
      <button onclick="StartUpload();">Start Upload</button><br />
       <button onclick="ClearList();">Clear List</button><br />
    
</body>
</html>
