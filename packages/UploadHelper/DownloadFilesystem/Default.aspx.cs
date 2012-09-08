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
    protected void Page_Load(object sender, EventArgs e)
    {
        // Get a list of all the files in the Files subdirectory
        string[] files = Directory.GetFiles(Server.MapPath("~/Files"));

        // Modify the list to remove path information
        for (int i = 0; i < files.Length; i++)
            files[i] = Path.GetFileName(files[i]);

        fileRepeater.DataSource = files;
        fileRepeater.DataBind();
    }
}
