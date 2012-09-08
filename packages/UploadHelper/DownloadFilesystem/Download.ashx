<%@ WebHandler Language="C#" Class="Download" %>
using System;
using System.IO;
using System.Web;

public class Download : IHttpHandler
{   
    public void ProcessRequest(HttpContext context)
    {
        // Get the file name from the query string
        string queryFile = context.Request.QueryString["file"];
        
        // Ensure that we were passed a file name
        if (string.IsNullOrEmpty(queryFile))
            throw new HttpException(404, null);

        // Generate the server file name        
        string file = Path.Combine(context.Server.MapPath("~/Files"), queryFile);
        
        // Ensure that the file exists
        if (!File.Exists(file))
            throw new HttpException(404, null);
 
        // Set the content type
        // TODO: set based on extension
        context.Response.ContentType = "image/jpeg";
        
        // Set the filename
        context.Response.AddHeader("content-disposition", "attachment;filename=" + queryFile);
        
        // Stream the file to the client
        context.Response.WriteFile(file);
    }
 
    public bool IsReusable
    {
        get { return true; }
    }
}