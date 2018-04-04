using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using Android.Content;
using Android.Speech;
using ListeningApp.Model;

namespace ListeningApp.Droid
{
    [Activity(Label = "ListeningApp", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());


            //MessagingCenter.Subscribe<string>(this, "SendVoiceRequest", message =>
            //{
            //    try
            //    {

            //        //var intent = new Intent(this, typeof(VoiceListener));
            //        //StartActivity(intent);
            //        VoiceIntent();

            //    }
            //    catch (Exception ex)
            //    {

            //        Console.Write(ex.Message);
            //    }
            //});
        }
        private readonly int VOICE = 10;
        private string _userCommand;
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == VOICE)
            {
                if (resultCode == Result.Canceled)
                {
                    SpeechResult result = new SpeechResult { Message = "Canceled", Error = true };

                    MessagingCenter.Send(result, "ReceiveSpeechText");
                }
                else
                {
                    var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
                    if (matches.Count != 0)
                    {
                        _userCommand = matches[0];
                        SpeechResult result = new SpeechResult { Message = _userCommand, Error = false };

                        MessagingCenter.Send(result, "ReceiveSpeechText");
                    }
                    else
                    {
                        SpeechResult result = new SpeechResult { Message = "No match", Error = true };

                        MessagingCenter.Send(result, "ReceiveSpeechText");
                    }
                }
            }
            base.OnActivityResult(requestCode, resultCode, data);
        }
    }
}

