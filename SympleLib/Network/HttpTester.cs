using Krystalware.UploadHelper;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using SympleLib.Network;

namespace SympleLib.Network
{
    public class HttpTester
    {
        public enum HttpVerb
        {
            GET,
            POST,
            PUT,
            DELETE
        };

        //-- URL only Get Assumed

        public static string PerformTest(string url)
        {
            return PerformTest<string>(url, HttpVerb.GET);
        }

        public static ReturnT PerformTest<ReturnT>(string url)
        {
            return PerformTest<ReturnT>(url, HttpVerb.GET);
        }

        //URL and Request Type without any postData

        public static string PerformTest(string url, HttpVerb method)
        {
            return PerformTest<string>(url, method);
        }

        public static ReturnT PerformTest<ReturnT>(string url, HttpVerb method)
        {
            return PerformTest<ReturnT>(url, method, null);
        }

        //URL, request type, and postData

        public static string PerformTest(string url, HttpVerb method, NameValueCollection postData)
        {
            return PerformTest<string>(url, method, postData);
        }

        public static ReturnT PerformTest<ReturnT>(string url, HttpVerb method, NameValueCollection postData)
        {
            return PerformTest<ReturnT>(url, method, postData, null);
        }

        //URL, request type, postData and files

        public static string PerformTest(string url, HttpVerb method, NameValueCollection postData, NameValueCollection postedFiles)
        {
            return PerformTest<string>(url, method, postData, postedFiles);
        }

        public static ReturnT PerformTest<ReturnT>(string url, HttpVerb method, NameValueCollection postData, NameValueCollection postedFiles)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = Enum.GetName(typeof(HttpVerb), method);

            List<UploadFile> postFiles = new List<UploadFile>();
            if (postedFiles != null)
            {
                foreach (var fKey in postedFiles.AllKeys)
                {
                    FileStream fs = File.OpenRead(postedFiles[fKey]);
                    postFiles.Add(new UploadFile(fs, fKey, postedFiles[fKey], "application/octet-stream"));
                }
            }

            HttpWebResponse webResponse = HttpUploadHelper.Upload(req, postFiles.ToArray(), postData ?? new NameValueCollection());

            using (Stream s = webResponse.GetResponseStream())
            {
                StreamReader sr = new StreamReader(s);
                var responseString = sr.ReadToEnd();

                if (typeof(ReturnT) == typeof(string))
                {
                    return (ReturnT)Convert.ChangeType(responseString, typeof(ReturnT));
                }

                return fastJSON.JSON.Instance.ToObject<ReturnT>(responseString);
            }
        }

        //-- Convert any object into a name value collection
        public static NameValueCollection ObjectToNameValueCollection(object obj)
        {
            NameValueCollection results = new NameValueCollection();

            var oType = obj.GetType();
            foreach (var prop in oType.GetProperties())
            {
                string pVal = "";
                try
                {
                    pVal = oType.GetProperty(prop.Name).GetValue(obj, null).ToString();
                }
                catch
                {
                }
                results[prop.Name] = pVal;
            }

            return results;
        }
    }
}
