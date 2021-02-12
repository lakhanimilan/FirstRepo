
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Remote_Shared.Adapter;

namespace Remote
{
    public class HomeFragment : Android.Support.V4.App.Fragment,
        View.IOnClickListener, IHomeAdapter
    {
        #region Variable/properties

        TextView mTxtViewIpAddress;
        TextView mTxtViewPort;
        TextView mTxtViewResponse;
        TextView mTxtViewLogs;

        LinearLayout mViewIp;

        HomeAdapter mAdapter;

        #endregion

        #region Constructors

        #endregion

        #region LifeCycleMethods

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            mAdapter = new HomeAdapter();

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            return inflater.Inflate(Resource.Layout.fragment_home, container, false);

            //return base.OnCreateView(inflater, container, savedInstanceState);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            initView(view);
        }

        #endregion

        #region PublicMethods

        #endregion

        #region PrivateMethods

        void initView (View view)
        {
            mTxtViewIpAddress = view.FindViewById<TextView>(Resource.Id.txt_ip);
            mTxtViewPort = view.FindViewById<TextView>(Resource.Id.txt_port);
            mTxtViewResponse = view.FindViewById<TextView>(Resource.Id.txt_resp);
            mTxtViewLogs = view.FindViewById<TextView>(Resource.Id.txt_log);
            

            Button btnStartListening = view.FindViewById<Button>(Resource.Id.btn_listening);
            btnStartListening.SetOnClickListener(this);

            Button btnOn = view.FindViewById<Button>(Resource.Id.btn_on);
            btnOn.SetOnClickListener(this);

            Button btnOff = view.FindViewById<Button>(Resource.Id.btn_off);
            btnOff.SetOnClickListener(this);

            mViewIp = view.FindViewById<LinearLayout>(Resource.Id.view_ipdetails);
            mViewIp.Visibility = ViewStates.Visible;
        }

        #endregion

        #region View.IOnClickListener

        public void OnClick(View v)
        {
            switch (v.Id)
            {

                case Resource.Id.btn_listening:
                    {
                        mAdapter.startLitening(Convert.ToInt32(mTxtViewPort.Text));
                    }
                    break;

                case Resource.Id.btn_on:
                    {
                        mAdapter.switchOn();
                    }
                    break;

                case Resource.Id.btn_off:
                    {
                        mAdapter.switchOff();
                    }
                    break;

                default:
                    break;
            }
        }



        #endregion

        #region IHomeAdapter

        public void onIpEndPointReceived(IPEndPoint endPoint)
        {
            mViewIp.PostDelayed(() => {
                mViewIp.Visibility = ViewStates.Visible;
                mTxtViewIpAddress.Text = string.Format("Ip Address : {0}", endPoint.Address);
                mTxtViewLogs.Text += string.Format("Ip Address : {0}\n", endPoint.Address);
            },
            100);

            
        }

        public void onHttpResponseReceive(string responseString)
        {
            mViewIp.PostDelayed(() => {
                mTxtViewLogs.Text += string.Format("HttpResponse : {0}\n", responseString);
            },
            100);
        }

        #endregion


    }
}
