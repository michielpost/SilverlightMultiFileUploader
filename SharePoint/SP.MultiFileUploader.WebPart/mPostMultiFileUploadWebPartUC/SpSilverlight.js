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
function createSL(divid, swidth, sheight, source, initparameters) {
    var pluginid = divid + "Plugin";
    var divElement = document.getElementById(divid);
    var altHTML = divElement.innerHTML;
    if (swidth == null)
        swidth = '100%';
    if (sheight == null)
        sheight = '750px';
    Silverlight.createObjectEx(
    {
        source: source,
        parentElement: divElement,
        id: pluginid,
        properties:
        {
            // Plug-in properties
            width: swidth,
            height: sheight,
            minRuntimeVersion: '2.0.31005.0'
        },
        events:
        {
            OnError: onSilverlightError, // OnError property value -- event-handler function name.
            OnLoad: msgBox // value -- event-handler function name.
        },
        initParams: initparameters
    });
}

function msgBox() {
    alert("ONLOAD");
}
