// http://aspnetupload.com
// Copyright © 2009 Krystalware, Inc.
//
// This work is licensed under a Creative Commons Attribution-Share Alike 3.0 United States License
// http://creativecommons.org/licenses/by-sa/3.0/us/

using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;

public partial class _Default : System.Web.UI.Page 
{
    protected void uploadButton_Click(object sender, EventArgs e)
    {
        // Check to see if a file was actually selected
        if (uploadFile.PostedFile != null && uploadFile.PostedFile.ContentLength > 0)
        {
            // Get the filename and folder to write to
            string fileName = Path.GetFileName(uploadFile.PostedFile.FileName);
            string folder = Server.MapPath("~/Files/");
            
            // Ensure the folder exists
            Directory.CreateDirectory(folder);

            // Save the file to the folder
            uploadFile.PostedFile.SaveAs(Path.Combine(folder, fileName));

            Response.Write("Uploaded: " + fileName);
        }
    }
}
