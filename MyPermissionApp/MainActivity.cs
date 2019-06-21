using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android;
using Android.Util;
using Android.Support.V4.App;
using Android.Content.PM;
using System;
using Android.Support.Design.Widget;
using Android.Views;

namespace MyPermissionApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {

        public  string TAG
        {
            get
            {
                return "123456";
            }
        }
        static readonly int REQUEST_PHONE_STATE = 1;


        /**
     	* Root of the layout of this Activity.
     	*/
        View layout;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            layout = FindViewById(Resource.Id.sample_main_layout);

            checkPermission();
        }



        private void getInfo() {
            string serial;
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                serial = Build.GetSerial();
            }
            else
            {
                serial = Build.Serial;
            }

            Log.Info(TAG, "serial = " + serial);
        }

        public void checkPermission()
        {
            Log.Info(TAG, "Checking permission.");

            // Check if the  permission is already available.
            if (ActivityCompat.CheckSelfPermission(this, Manifest.Permission.ReadPhoneState) != (int)Permission.Granted)
            {

                //  permission has not been granted
                RequestPhoneStatePermission();
            }
            else
            {
                //  permissions is already available, show the camera preview.
                Log.Info(TAG, " permission has already been granted.");
                getInfo();
            }
        }

        private void RequestPhoneStatePermission()
        {
            Log.Info(TAG, "ReadPhoneState  permission has NOT been granted. Requesting permission.");

            if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.ReadPhoneState))
            {
                // Provide an additional rationale to the user if the permission was not granted
                // and the user would benefit from additional context for the use of the permission.
                // For example if the user has previously denied the permission.
                Log.Info(TAG, "Displaying ReadPhoneState  permission rationale to provide additional context.");

                Snackbar.Make(layout, Resource.String.permission_phonestate_rationale,
                    Snackbar.LengthIndefinite).SetAction(Resource.String.ok, new Action<View>(delegate (View obj) {
                        ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.ReadPhoneState }, REQUEST_PHONE_STATE);
                    })).Show();
            }
            else
            {
                // Camera permission has not been granted yet. Request it directly.
                ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.ReadPhoneState }, REQUEST_PHONE_STATE);
            }
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            if (requestCode == REQUEST_PHONE_STATE)
            {
                // Received permission result for camera permission.
                Log.Info(TAG, "Received response for phone state permission request.");

                // Check if the only required permission has been granted
                if (grantResults.Length == 1 && grantResults[0] == Permission.Granted)
                {
                    // Camera permission has been granted, preview can be displayed
                    Log.Info(TAG, "phonestate permission has now been granted. Showing preview.");
                    Snackbar.Make(layout, Resource.String.permission_available_phonestate, Snackbar.LengthShort).Show();

                    getInfo();

                }
                else
                {
                    Log.Info(TAG, "phonestate permission was NOT granted.");
                    Snackbar.Make(layout, Resource.String.permissions_not_granted, Snackbar.LengthShort).Show();
                }
            }
            else
            {
                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            }
        }
    }
}