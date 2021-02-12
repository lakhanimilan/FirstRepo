using System;
using System.IO;
using System.Net;

namespace Remote_Shared
{
    public class HTTPClient
    {
        #region Variable/properties

        WeakReference _weaklistener;
        IHttpClient mListener
        {
            get
            {
                if (_weaklistener != null)
                {
                    return _weaklistener.Target as IHttpClient;
                }
                return null;
            }
            set
            {
                _weaklistener = new WeakReference(value);
            }
        }

        #endregion

        #region Constructors
        public HTTPClient(IHttpClient listener)
        {
            mListener = listener;
        }
        #endregion

        #region LifeCycleMethods

        #endregion

        #region PublicMethods

        public void callAPI (string url)
        {
            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;

            try
            {
                WebResponse webResponse = webRequest.GetResponse();

                StreamReader reader = new StreamReader(webResponse.GetResponseStream());
                string str = reader.ReadLine();
                while (str != null)
                {
                    Console.WriteLine(str);
                    str = reader.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception :::: " + ex.Message);
            }
        }

        public void callAPIAsync(string url)
        {
            
        }

        #endregion

        #region PrivateMethods

        #endregion
    }

    public interface IHttpClient
    {
        void onResponseReceived(string responsString);
    }
}
