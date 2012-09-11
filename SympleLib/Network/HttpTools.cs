using System.Dynamic;
using Krystalware.UploadHelper;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using SympleLib.Helpers;

namespace SympleLib.Network
{
    public class HttpTools
    {
        public static string PerformGet(string url)
        {
            WebRequest req = WebRequest.Create(url);
            WebResponse resp = req.GetResponse();
            StreamReader sr = new StreamReader(resp.GetResponseStream());
            return sr.ReadToEnd().Trim();
        }

        public static ReturnT PerformPost<ReturnT>(string url)
        {
            return PerformPost<object, ReturnT>(url, new object());
        }

        public static ReturnT PerformPost<PostT, ReturnT>(string url, PostT postData)
        {
            return PerformPost<PostT, ReturnT>(url, postData, new NameValueCollection());
        }
        public static ReturnT PerformPost<PostT, ReturnT>(string url, PostT postData, NameValueCollection files)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

            NameValueCollection nvpPost;
            if (typeof(PostT) == typeof(NameValueCollection))
            {
                nvpPost = postData as NameValueCollection;
            }
            else
            {
                nvpPost = ObjectToNameValueCollection<PostT>(postData);
            }

            List<UploadFile> postFiles = new List<UploadFile>();
            foreach (var fKey in files.AllKeys)
            {
                FileStream fs = File.OpenRead(files[fKey]);
                postFiles.Add(new UploadFile(fs, fKey, files[fKey], "application/octet-stream"));
            }

            var response = HttpUploadHelper.Upload(req, postFiles.ToArray(), nvpPost);

            using (Stream s = response.GetResponseStream())
            using (StreamReader sr = new StreamReader(s))
            {
                var responseJson = sr.ReadToEnd();
                if (typeof(ReturnT) == typeof(string))
                {
                    return (ReturnT)Convert.ChangeType(responseJson, typeof(ReturnT));
                }

                return fastJSON.JSON.Instance.ToObject<ReturnT>(responseJson);
            }
        }


        public static ReturnT PerformJsonRequest<PostT, ReturnT>(string url, string method, PostT postData)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.ContentType = "application/json";
            req.Method = method;

            StringBuilder sbJsonRequest = new StringBuilder();
            var T = typeof(PostT);
            foreach (var prop in T.GetProperties())
            {
                if (ObjectHelpers.NativeTypes.Contains(prop.PropertyType))
                {
                    sbJsonRequest.AppendFormat("\"{0}\":\"{1}\",", prop.Name.ToLower(), prop.GetValue(postData, null));
                }
            }

            if (method != "get")
            {
                using (var sWriter = new StreamWriter(req.GetRequestStream()))
                {
                    sWriter.Write("{" + sbJsonRequest.ToString().TrimEnd(',') + "}");
                }
            }

            using (var wResponse = req.GetResponse())
            {
                StreamReader sReader = new StreamReader(wResponse.GetResponseStream());
                var responseJson = sReader.ReadToEnd();
                if (typeof(ReturnT) == typeof(string))
                {
                    return (ReturnT)Convert.ChangeType(responseJson, typeof(ReturnT));
                }

                return fastJSON.JSON.Instance.ToObject<ReturnT>(responseJson);
            }
        }

        private static NameValueCollection ObjectToNameValueCollection<T>(T obj)
        {
            NameValueCollection results = new NameValueCollection();

            var oType = typeof(T);
            foreach (var prop in oType.GetProperties())
            {
                string pVal = "";
                try
                {
                    pVal = oType.GetProperty(prop.Name).GetValue(obj, null).ToString();
                }
                catch { }
                results[prop.Name] = pVal;
            }

            return results;
        }
    }
}
