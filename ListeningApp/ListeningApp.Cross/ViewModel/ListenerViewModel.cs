using ListeningApp.Model;
using ListeningApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ListeningApp.ViewModel
{
    public class ListenerViewModel : BaseViewModel
    {
        private bool _isListening = false;
        public bool IsListening
        {
            get { return _isListening; }
            set { SetProperty(ref _isListening, value); }
        }
        private SpeechResult _speechResult;
        public SpeechResult SpeechResult
        {
            get { return _speechResult; }
            set { SetProperty(ref _speechResult, value); }
        }

        // WITHOUT DIALOG
        ICommand startListeningWithoutDialogCommand;
        public ICommand StartListeningWithoutDialogCommand =>
            startListeningWithoutDialogCommand ?? (startListeningWithoutDialogCommand = new Command(() => ExecuteListeningWithoutDialogCommandCommand()));
        void ExecuteListeningWithoutDialogCommandCommand()
        {
            IsListening = true;
            MessagingCenter.Subscribe<SpeechResult>(this, "ReceiveSpeechText", (result) =>
            {
                SpeechResult = result;
                IsListening = false;
                MessagingCenter.Unsubscribe<SpeechResult>(this, "ReceiveSpeechText");
            });
            DependencyService.Get<ISpeechRecognizerService>().StartListeningWithoutDialog();

        }


        // WITH DIALOG
        ICommand startListeningWithDialogCommand;
        public ICommand StartListeningWithDialogCommand =>
            startListeningWithDialogCommand ?? (startListeningWithDialogCommand = new Command(() => ExecuteListeningWithDialogCommandCommand()));
        void ExecuteListeningWithDialogCommandCommand()
        {
            IsListening = true;
            MessagingCenter.Subscribe<SpeechResult>(this, "ReceiveSpeechText", (result) =>
            {
                SpeechResult = result;
                IsListening = false;
                MessagingCenter.Unsubscribe<SpeechResult>(this, "ReceiveSpeechText");
            });
            DependencyService.Get<ISpeechRecognizerService>().StartListeningWithDialog();

        }

        public ListenerViewModel()
        {
            SpeechResult = new SpeechResult { Message = "Press ANY button for recognize", Error = false };
            this.Title = "Listener";

        }
    }
}
