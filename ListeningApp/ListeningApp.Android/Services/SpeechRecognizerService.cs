using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Speech;
using Android.Views;
using Android.Widget;
using ListeningApp.Droid.Services;
using ListeningApp.Model;
using ListeningApp.Services;
using Plugin.CurrentActivity;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(SpeechRecognizerService))]
namespace ListeningApp.Droid.Services
{
    class SpeechRecognizerService : Java.Lang.Object, ISpeechRecognizerService, IRecognitionListener
    {
        private SpeechRecognizer speech = null;
        private Activity context;
        private readonly int VOICE = 10;
        private string _userCommand;


        public void StartListeningWithoutDialog()
        {
            context = CrossCurrentActivity.Current.Activity;
            if (SpeechRecognizer.IsRecognitionAvailable(context))
               speech = SpeechRecognizer.CreateSpeechRecognizer(context, ComponentName.UnflattenFromString("com.google.android.googlequicksearchbox/com.google.android.voicesearch.serviceapi.GoogleRecognitionService"));

            // por algum motivo quando uso o create usando a MainActivity ocorre o erro 'SpeechRecognizer: no selected voice recognition service'
            //speech = SpeechRecognizer.CreateSpeechRecognizer(context);
            speech.SetRecognitionListener(this);

            var mSpeechRecognizerIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            mSpeechRecognizerIntent.PutExtra(RecognizerIntent.ExtraCallingPackage, "com.matheusgigliotti.ListeningApp");
            mSpeechRecognizerIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
            // put a message on the modal dialog
            //voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, "Speak now");
            // end speech if 1.5 secs have passed
            mSpeechRecognizerIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1500);
            mSpeechRecognizerIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);
            mSpeechRecognizerIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
            mSpeechRecognizerIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1500);
            mSpeechRecognizerIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);


            speech.StartListening(mSpeechRecognizerIntent);
        }

        public void StartListeningWithDialog()
        {
            var mSpeechRecognizerIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            mSpeechRecognizerIntent.PutExtra(RecognizerIntent.ExtraPrompt, "Speak now");

            // end speech if 1.5 secs have passed
            mSpeechRecognizerIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1500);
            mSpeechRecognizerIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);
            mSpeechRecognizerIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
            mSpeechRecognizerIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1500);

            mSpeechRecognizerIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);

            CrossCurrentActivity.Current.Activity.StartActivityForResult(mSpeechRecognizerIntent, VOICE);
        }


        #region IRecognitionListener


        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public void OnBeginningOfSpeech()
        {
            //throw new NotImplementedException();

        }

        public void OnBufferReceived(byte[] buffer)
        {
            //throw new NotImplementedException();
        }

        public void OnEndOfSpeech()
        {
            //throw new NotImplementedException();
        }

        public void OnError([GeneratedEnum] SpeechRecognizerError error)
        {
            String errorMessage = getErrorText(error);
            SpeechResult result = new SpeechResult { Message = errorMessage, Error = true };

            MessagingCenter.Send(result, "ReceiveSpeechText");
        }

        public void OnEvent(int eventType, Bundle @params)
        {
            //throw new NotImplementedException();
        }

        public void OnPartialResults(Bundle partialResults)
        {
            //throw new NotImplementedException();
        }

        public void OnReadyForSpeech(Bundle @params)
        {
            //throw new NotImplementedException();
        }

        public void OnResults(Bundle results)
        {
            var matches = results.GetStringArrayList(SpeechRecognizer.ResultsRecognition);
            if (matches.Count != 0)
            {
                _userCommand = matches[0];
                SpeechResult result = new SpeechResult { Message = _userCommand, Error = false };

                MessagingCenter.Send(result, "ReceiveSpeechText");
            }


        }

        public void OnRmsChanged(float rmsdB)
        {
            //throw new NotImplementedException();
        }

        public static String getErrorText(SpeechRecognizerError error)
        {
            String message;
            switch (error)
            {
                case SpeechRecognizerError.Audio:
                    message = "Audio recording error";
                    break;
                case SpeechRecognizerError.Client:
                    message = "Client side error";
                    break;
                case SpeechRecognizerError.InsufficientPermissions:
                    message = "Insufficient permissions";
                    break;
                case SpeechRecognizerError.Network:
                    message = "Network error";
                    break;
                case SpeechRecognizerError.NetworkTimeout:
                    message = "Network timeout";
                    break;
                case SpeechRecognizerError.NoMatch:
                    message = "No match";
                    break;
                case SpeechRecognizerError.RecognizerBusy:
                    message = "RecognitionService busy";
                    break;
                case SpeechRecognizerError.Server:
                    message = "error from server";
                    break;
                case SpeechRecognizerError.SpeechTimeout:
                    message = "No speech input";
                    break;
                default:
                    message = "Didn't understand, please try again.";
                    break;
            }
            return message;
        }

        #endregion
    }
}