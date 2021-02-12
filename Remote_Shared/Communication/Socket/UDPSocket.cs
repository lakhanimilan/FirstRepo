using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Remote_Shared
{
    public class UDPSocket
    {
        #region Variables

        Socket mSocket;
        Thread mThread;

        int mReceiverPort;

        WeakReference _weaklistener;
        IUDPSocket mListener
        {
            get
            {
                if (_weaklistener != null)
                {
                    return _weaklistener.Target as IUDPSocket;
                }
                return null;
            }
            set
            {
                _weaklistener = new WeakReference(value);
            }
        }

        #endregion

        #region Constructor

        public UDPSocket(IUDPSocket listener)
        {
            mListener = listener;
        }

        #endregion

        #region PublicMethods

        public sbyte initSocketConnection ()
		{
            sbyte initResult = -1;

            mSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            initResult = 0;

            return initResult;
        }

        public void startListening (int port)
		{
            EndPoint localEndPoint = new IPEndPoint(IPAddress.Any, port);
            mSocket.Bind(localEndPoint);

            Console.WriteLine(string.Format("Remote:: Listening with : {0}", localEndPoint));

            mReceiverPort = port;
            startRecevingThread();
		}

        #endregion

        #region PrivateMethods

        void startRecevingThread ()
        {
            mThread = new Thread(new ThreadStart(onThreadStarted));
            mThread.Start();
        }

        void onThreadStarted ()
        {
            while(true)
            {
                int available = mSocket.Available;

                Console.WriteLine(string.Format("Remote:: AvailableData : {0}", available));

                if (available > 0)
                {
                    byte[] data = new byte[1325];
                    EndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);

                    try
                    {
                        int receivedBytes = mSocket.ReceiveFrom(data, ref endPoint);
                        if (receivedBytes > 0)
                        {
                            Console.WriteLine(string.Format("Remote:: DataReceived : {0}, From {1}",
                                receivedBytes, endPoint));

                            if (mListener != null)
                            {
                                mListener.onDataReceived(data, endPoint);
                            }

                            mSocket.Disconnect(false);
                            mSocket.Close();
                            mSocket.Dispose();
                            mSocket = null;
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception : " + ex.Message);
                    }

                    Thread.Sleep(200);
                }

                Thread.Sleep(200);
            }
        }
        #endregion
    }

    public interface IUDPSocket
    {
        void onDataReceived(byte[] data, EndPoint endPoint);
    }
}
