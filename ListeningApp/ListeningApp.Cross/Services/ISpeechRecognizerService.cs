
namespace ListeningApp.Services
{
    public interface ISpeechRecognizerService
    {
        void StartListeningWithoutDialog();
        void StartListeningWithDialog();
    }
}
