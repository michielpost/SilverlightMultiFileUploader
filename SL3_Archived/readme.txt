/*
 * Copyright Michiel Post
 * http://www.michielpost.nl
 * contact@michielpost.nl
 *
 * http://www.codeplex.com/SLFileUpload/
 *
 * */

Silverlight 3

Free Silverlight Multi File Uploader (v3.0)

MaxFileSizeKB: 	File size in KBs.
MaxUploads: 	Maximum number of simultaneous uploads
FileFilter:	File filter, for example ony jpeg use: FileFilter=Jpeg (*.jpg) |*.jpg
CustomParam: Your custom parameter, anything here will be available in the WCF webservice
DefaultColor: The default color for the control, for example: LightBlue
HttpUploader:	If false, it will use the SilverlightUploadService.svc (WCF) to upload the file instead of the HttpHandler (default)
UploadHandlerName: Custom specified name of the HttpUploadHandler, for example this can be "PHPUpload.php" to use the PHP upload handler.


Possible parameters:
 <asp:Silverlight ID="Xaml1" runat="server" Source="~/ClientBin/mpost.SilverlightMultiFileUpload.xap" MinimumVersion="2.0.30523"  Width="415" Height="280" 
InitParameters="MaxFileSizeKB=1000,MaxUploads=2,FileFilter=,CustomParam=1,DefaultColor=LightBlue"
 />


JavaScript Events
For a better integration with your own website, the Silveright Multi File Uploader control has the following JavaScript events and properties:

Events:

AllFilesFinished - Fires when all files are finished uploading (does not fire when Errors Occurred during upload) 
SingleFileUploadFinished - Fires when a single file succesfully uploaded 
ErrorOccurred - Fires when an error occurred during uploading 

Properties:
The following properties can be read at any time

TotalUploadedFiles: Total number of files uploaded 
TotalFilesSelected: Total number of files in the list 
Percentage: Percentage of total upload progress 

Actions:
The following actions can be triggered using JavaScript:
SelectFiles:	Opens the Select Files window
StartUpload:	Starts the upload process
ClearList:	Clears the list


See the included testpage (WCF_SilverlightMultiFileUploadTestPage.aspx) for example usage.

----------
PHP usage
You can use the PHPUpload.php handler to support uploads to a PHP server.
Configure the Silverlight Multi File Uploader to use the HttpUploader and specify the name of the PHP script.

Example:
<param name="initParams" value="HttpUploader=true,UploadHandlerName=PHPUpload.php,DefaultColor=Green" />

See the included PHP testpage (PHP_SilverlightMultiFileUploadTestPage.html) for example usage.

----------

Drop me a line on contact@michielpost.nl if this was useful for you or if you made something nice out of this.


