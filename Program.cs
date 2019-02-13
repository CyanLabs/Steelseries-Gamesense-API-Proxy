using System;
using System.IO;
using System.Net;
using System.Text;

namespace DumpHttpRequests
{
    internal class Program
    {
        private static void Main(string[] args)
        {

            string jsonText = System.IO.File.ReadAllText(@"C:\ProgramData\SteelSeries\SteelSeries Engine 3\coreProps.json");
            char[] separators = new char[] { '{', '}', ',', '"' };
            string[] coreProps = jsonText.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://*:12345/");
            listener.Start();
            Console.WriteLine("Listening...");

                while (true)
                {
                try
                {

                    HttpListenerContext context = listener.GetContext();
                    HttpListenerRequest request = context.Request;
                    string ProxiedData;
                    using (Stream receiveStream = request.InputStream)
                    {
                        using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
                        {
                            ProxiedData = readStream.ReadToEnd();
                        }
                    }

                    HttpListenerResponse response = context.Response;
                    string responseString = ProxiedData;
                    Console.WriteLine(ProxiedData);
                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                    response.ContentLength64 = buffer.Length;
                    System.IO.Stream output = response.OutputStream;
                    output.Write(buffer, 0, buffer.Length);
                    output.Close();
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://" + coreProps[2] + request.Url.PathAndQuery);
                    httpWebRequest.ContentType = "application/json; charset=utf-8";
                    httpWebRequest.Method = "POST";

                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        streamWriter.Write(ProxiedData);
                        streamWriter.Flush();
                    }
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                }
                catch (WebException webex)
                {
                    Console.WriteLine(webex);
                }
            }

        }
    }
}