
Want to use this control on SSL? You'll need to make the following changes

1) Change the Web.config
Find the following line:
<endpoint address="" binding="basicHttpBinding" contract="mpost.FileUploadServiceLibrary.IUploadService">

Add this configuration option to the line: bindingConfiguration="HttpsBinding"
The line will now look like this:
<endpoint address="" binding="basicHttpBinding" bindingConfiguration="HttpsBinding" contract="mpost.FileUploadServiceLibrary.IUploadService">


2) Change the Silverlight Source
Open the file WcfFileUploader.cs in the Classes folder in the mpost.SilverlightMultiFileUpload project.

//Enable this line for HTTPS
_client = new UploadService.UploadServiceClient();

And remove the following lines:
//Create WCF connection
//BasicHttpBinding binding = new BasicHttpBinding();
//EndpointAddress address = new EndpointAddress(new CustomUri("SilverlightUploadService.svc"));
//_client = new UploadService.UploadServiceClient(binding, address);

3) Change the ServiceReferences.ClientConfig

Change the mode in mode="Transport" for HTTPS
<security mode="None" />

Change your endpoint location in the correct URL for the endpoint:
<!--Make sure this URL is correct if you're using this on HTTPS-->
            <endpoint address="https://some-server/SilverlightUploadService.svc"


4) Recompile, this will result in a new Silverlight XAP file in the Web project

5) You're done. It now works on https



            

