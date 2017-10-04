using HTTPSimulator.DataGenerator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HTTPSimulator
{
    class HTTPRequester
    {
        private static IGenerator generator = null;
        private static bool threadFlag = false;
        private static int interval = 3000;
        private static List<string> urlList = new List<string>();
        
        private static bool initialized = HTTPRequester.Initialize();

        private static bool Initialize()
        {
            (new Thread(ThreadProc)).Start();
            return true;
        }
        
        private static void ThreadProc()
        {
            while (true)
            {
                Thread.Sleep(HTTPRequester.interval);

                try
                {
                    if (threadFlag && HTTPRequester.generator != null)
                    {
                        var content = HTTPRequester.generator.Generate();
                        var contentByte = Encoding.UTF8.GetBytes(content);

                        HTTPRequester.urlList.AsParallel().ForAll(url =>
                        {
                            try
                            {
                                var request = (HttpWebRequest)WebRequest.Create(url);
                                request.Method = "POST";
                                request.ContentType = "application/json";
                                request.ContentLength = contentByte.Length;

                                using (var rs = request.GetRequestStream())
                                {
                                    rs.Write(contentByte, 0, contentByte.Length);
                                }

                                using (var reader = new StreamReader(request.GetResponse().GetResponseStream()))
                                {
                                    reader.ReadToEnd();
                                }
                            } catch { }
                        });
                    }
                } catch { }
            }
        }

        public static void SetGenerator(IGenerator generator)
        {
            HTTPRequester.generator = generator;
        }

        public static KeyValuePair<int, bool> SetMode(int interval)
        {
            return new KeyValuePair<int, bool>(
                interval <= 0 ? HTTPRequester.interval = 500 : HTTPRequester.interval = interval,
                HTTPRequester.threadFlag ^= true);
        }

        public static void SetList(List<string> list)
        {
            HTTPRequester.urlList = list;
        }
    }
}
