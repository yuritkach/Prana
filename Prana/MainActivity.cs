using System;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Prana.src.infrared;

namespace Prana
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            
            FindViewById(Resource.Id.offButton).Click += (object sender, EventArgs e) =>
            {
                ConsumerIrManager manager = ConsumerIrManager.getSupportConsumerIrManager(this);

                // Check whether IrEmitter is avaiable on the device.
                if (!manager.hasIrEmitter())
                {
                    Log.Error("AndroidInfraredDemo", "Cannot found IR Emitter on the device");
                }

                // Build IR Command with predefined schemes.
                
                    IrCommand necCommand = IrCommand.NEC.BuildNEC(32, 0x00FF00FF);
                    manager.transmit(necCommand);
                
            };

        
                
        }

        



        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View) sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        }
	}
}

