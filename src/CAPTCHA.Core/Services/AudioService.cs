using System.Speech.Synthesis;

namespace CAPTCHA.Core.Services
{
    public class AudioService
    {
        public static byte[] ConvertSentenceToAudioBytes(string sentence)
        {
            using var synth = new SpeechSynthesizer();
            using var memoryStream = new MemoryStream();
            synth.SetOutputToWaveStream(memoryStream);
            synth.Speak(sentence);

            return memoryStream.ToArray();
        }
    }
}
