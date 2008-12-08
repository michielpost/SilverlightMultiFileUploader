<%@ WebHandler Language="C#" Class="HttpUploadHandler" %>

using System;
using System.Web;
using System.IO;
using System.Web.Hosting;

/*
 * Copyright Michiel Post
 * http://www.michielpost.nl
 * contact@michielpost.nl
 * */

public class HttpUploadHandler : IHttpHandler {

    private HttpContext _httpContext;
    private string _tempExtension = "_temp";
    private string _fileName;
    private string _parameters;
    private bool _lastChunk;
    private bool _firstChunk;
    private long _startByte;
    


    public void ProcessRequest(HttpContext context)
    {
        _httpContext = context;

        GetQueryStringParameters();

        string uploadFolder = GetUploadFolder();
        string tempFileName = _fileName + _tempExtension;

        //Is it the first chunk? Prepare by deleting any existing files with the same name
        if (_firstChunk)
        {
            WriteDebugMessage("First Chunk Arrived at Webservice");

            //Delete temp file
            if (File.Exists(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + tempFileName))
                File.Delete(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + tempFileName);

            //Delete target file
            if (File.Exists(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + _fileName))
                File.Delete(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + _fileName);

        }
        
        //Write the file
        WriteDebugMessage(string.Format("Write data to disk FOLDER: {0}", uploadFolder));
        using (FileStream fs = File.Create(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + tempFileName))
        {
            SaveFile(context.Request.InputStream, fs);
            fs.Close();
        }
        WriteDebugMessage("Write data to disk SUCCESS");

        //Is it the last chunk? Then finish up...
        if (_lastChunk)
        {
            //Rename file to original file
            File.Move(HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + tempFileName, HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + _fileName);

            //Finish stuff....
            FinishedFileUpload(_fileName, _parameters);
        }

    }

    /// <summary>
    /// Get the querystring parameters
    /// </summary>
    private void GetQueryStringParameters()
    {
        _fileName = _httpContext.Request.QueryString["file"];
        _parameters = _httpContext.Request.QueryString["param"];
        _lastChunk = string.IsNullOrEmpty(_httpContext.Request.QueryString["last"]) ? true : bool.Parse(_httpContext.Request.QueryString["last"]);
        _firstChunk = string.IsNullOrEmpty(_httpContext.Request.QueryString["first"]) ? true : bool.Parse(_httpContext.Request.QueryString["first"]);
        _startByte = string.IsNullOrEmpty(_httpContext.Request.QueryString["offset"]) ? 0 : long.Parse(_httpContext.Request.QueryString["offset"]); ;
    }

    /// <summary>
    /// Save the contents of the Stream to a file
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="fs"></param>
    private void SaveFile(Stream stream, FileStream fs)
    {
        byte[] buffer = new byte[4096];
        int bytesRead;
        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
        {
            fs.Write(buffer, 0, bytesRead);
        }
    }

    /// <summary>
    /// Do your own stuff here when the file is finished uploading
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="parameters"></param>
    protected virtual void FinishedFileUpload(string fileName, string parameters)
    {
    }

    
    protected virtual string GetUploadFolder()
    {
        string folder = System.Configuration.ConfigurationSettings.AppSettings["UploadFolder"];
        if (string.IsNullOrEmpty(folder))
            folder = "Upload";

        return folder;
    }

    /// <summary>
    /// Only write some DEBUG messages in DEBUG mode
    /// </summary>
    /// <param name="message"></param>
    [System.Diagnostics.Conditional("DEBUG")]
    private void WriteDebugMessage(string message)
    {
        FileInfo t = new FileInfo("debug.txt");

        using (StreamWriter Tex = t.AppendText())
        {
            Tex.WriteLine(string.Format("{0} | {1}", DateTime.Now, message));
            Tex.Close();
        }
    }


 
    public bool IsReusable {
        get {
            return false;
        }
    }

}