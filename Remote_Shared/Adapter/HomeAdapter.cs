using System;
using System.Net;

namespace Remote_Shared.Adapter
{
    public class HomeAdapter : IUDPSocket, IHttpClient
    {
        #region Variable/properties

        UDPSocket mUDPSocket;

        IPEndPoint mEndPoint;

        HTTPClient httpClient;

        WeakReference _weaklistener;
        IHomeAdapter mListener
        {
            get
            {
                if (_weaklistener != null)
                {
                    return _weaklistener.Target as IHomeAdapter;
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

        public HomeAdapter()
        {
        }

        

        #endregion

        #region PublicMethods

        public void startLitening (int port)
        {
            Console.WriteLine(string.Format("Remote:: Button clicked with port : {0}", port));

            mUDPSocket = new UDPSocket(this);
            mUDPSocket.initSocketConnection();

            mUDPSocket.startListening(port);
        }

        public void switchOn ()
        {
            if (httpClient == null)
                httpClient = new HTTPClient(this);

            httpClient.callAPIAsync(getUrlString(1, true));
        }

        public void switchOff()
        {
            if (httpClient == null)
                httpClient = new HTTPClient(this);

            httpClient.callAPIAsync(getUrlString(1, false));
        }

        #endregion

        #region IUDPSocket

        public void onDataReceived(byte[] data, EndPoint endPoint)
        {
            mEndPoint = endPoint as IPEndPoint;

            if(mListener != null)
            {
                mListener.onIpEndPointReceived(mEndPoint);
            }
        }

        #endregion

        #region IHttpClient

        public void onResponseReceived(string responsString)
        {
            Console.WriteLine("RESPONSE RECEIVED : " + responsString);

            if (mListener != null)
                mListener.onHttpResponseReceive(responsString);
        }

        #endregion

        #region PrivateMethods

        string getUrlString (int relayNo, bool isSwithOn)
        {
            string baseUrl = mEndPoint.Address.ToString();
            string fullUrl = string.Format("http://{0}/digital/{1}/{2}", baseUrl, relayNo, isSwithOn?1:0);

                //http://192.168.0.100/digital/1/0
            return fullUrl;
        }

        #endregion

    }

    public interface IHomeAdapter
    {
        void onIpEndPointReceived(IPEndPoint endPoint);
        void onHttpResponseReceive(string responseString);
    }
}
