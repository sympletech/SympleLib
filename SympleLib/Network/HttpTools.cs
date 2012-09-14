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
        public enum RequestTypes { Get, Post, Put, Delete }

        public static string PerformGet(string url)
        {
            HttpWebRequest wRequest = (HttpWebRequest)WebRequest.Create(url);
            return PerformWebRequest(wRequest);
        }
        

        #region Post

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

            string responseRaw = "";
            //string responseRaw = PerformWebRequest(req);
            try
            {
                using (Stream s = response.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(s))
                    {
                        responseRaw = sr.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                using (WebResponse exResponse = ex.Response)
                {
                    StreamReader sReader = new StreamReader(response.GetResponseStream());
                    responseRaw = sReader.ReadToEnd();
                }
            }

            if (typeof(ReturnT) == typeof(string))
            {
                return (ReturnT)Convert.ChangeType(responseRaw, typeof(ReturnT));
            }

            return fastJSON.JSON.Instance.ToObject<ReturnT>(responseRaw);
        }

        //Converts an object to a name value collection (for posts)
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

        #endregion

        #region JSON Request

        public static ReturnT PerformJsonRequest<ReturnT>(string url, RequestTypes method, object postData)
        {
            //Initilize the http request
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.ContentType = "application/json";
            req.Method = Enum.GetName(typeof(RequestTypes), method);


            //If posting data - serialize it to a json object
            if (method != RequestTypes.Get)
            {
                StringBuilder sbJsonRequest = new StringBuilder();
                var T = postData.GetType();
                foreach (var prop in T.GetProperties())
                {
                    if (ApprovedTypes.Contains(prop.PropertyType))
                    {
                        sbJsonRequest.AppendFormat("\"{0}\":\"{1}\",", prop.Name.ToLower(), prop.GetValue(postData, null));
                    }
                }

                using (var sWriter = new StreamWriter(req.GetRequestStream()))
                {
                    sWriter.Write("{" + sbJsonRequest.ToString().TrimEnd(',') + "}");
                }
            }

            //Submit the Http Request
            string responseJson = "";
            try
            {
                using (var wResponse = req.GetResponse())
                {
                    StreamReader sReader = new StreamReader(wResponse.GetResponseStream());
                    responseJson = sReader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                using (WebResponse response = ex.Response)
                {
                    StreamReader sReader = new StreamReader(response.GetResponseStream());
                    responseJson = sReader.ReadToEnd();
                }
            }

            if (typeof(ReturnT) == typeof(string))
            {
                return (ReturnT)Convert.ChangeType(responseJson, typeof(ReturnT));
            }

            return fastJSON.JSON.Instance.ToObject<ReturnT>(responseJson);
        }

        //Approved Types for serialization
        public static List<Type> ApprovedTypes
        {
            get
            {
                var approvedTypes = new List<Type>();

                approvedTypes.Add(typeof(int));
                approvedTypes.Add(typeof(Int32));
                approvedTypes.Add(typeof(Int64));
                approvedTypes.Add(typeof(string));
                approvedTypes.Add(typeof(DateTime));
                approvedTypes.Add(typeof(double));
                approvedTypes.Add(typeof(decimal));
                approvedTypes.Add(typeof(float));
                approvedTypes.Add(typeof(List<>));
                approvedTypes.Add(typeof(bool));
                approvedTypes.Add(typeof(Boolean));

                approvedTypes.Add(typeof(int?));
                approvedTypes.Add(typeof(Int32?));
                approvedTypes.Add(typeof(Int64?));
                approvedTypes.Add(typeof(DateTime?));
                approvedTypes.Add(typeof(double?));
                approvedTypes.Add(typeof(decimal?));
                approvedTypes.Add(typeof(float?));
                approvedTypes.Add(typeof(bool?));
                approvedTypes.Add(typeof(Boolean?));

                return approvedTypes;
            }
        }

        #endregion


        private static string PerformWebRequest(HttpWebRequest wRequest)
        {
            string responseRaw = "";
            try
            {
                WebResponse resp = wRequest.GetResponse();
                StreamReader sr = new StreamReader(resp.GetResponseStream());
                responseRaw = sr.ReadToEnd().Trim();
            }
            catch (WebException ex)
            {
                using (WebResponse response = ex.Response)
                {
                    StreamReader sReader = new StreamReader(response.GetResponseStream());
                    responseRaw = sReader.ReadToEnd();
                }
            }
            return responseRaw;
        }    
    }
}
