using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Recognition;
using System.Runtime.InteropServices;

namespace SpeechRecognition
{

    public class SpeechManager
    {
        SpeechRecognitionEngine recognitionEngine;

        Queue<string> messageQueue = new Queue<string>();
        List<string> wordsToRecognise;

        public void LoadCommands(List<string> _wordToRecognise)
        {
            recognitionEngine = new SpeechRecognitionEngine();
            wordsToRecognise = _wordToRecognise;
            recognitionEngine = new SpeechRecognitionEngine();
            Choices commands = new Choices();

            foreach(var word in _wordToRecognise)
            {
                if (word != "")
                {
                    commands.Add(word);
                }
            }

            GrammarBuilder grammarBuilder = new GrammarBuilder();
            grammarBuilder.Append(commands);

            Grammar grammar = new Grammar(grammarBuilder);

            recognitionEngine.LoadGrammarAsync(grammar);

            //Set to current microphone
            recognitionEngine.SetInputToDefaultAudioDevice();

            recognitionEngine.SpeechRecognized += SpeechRecognized;
        }

        public void StartRecognising()
        {
            recognitionEngine.RecognizeAsync(RecognizeMode.Multiple);
        }

        public void StopRecognising()
        {
            recognitionEngine.RecognizeAsyncStop();
        }

        public void SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            messageQueue.Enqueue(e.Result.Text);
        }

        public bool CheckIfMessageIsInQueue()
        {
            if (messageQueue != null)
            {
                if (messageQueue.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public string GetMessageFromQueue()
        {
            if (messageQueue != null)
            {
                return messageQueue.Dequeue();
            }
            else
            {
                return "No String Available";
            }
        }
    }
}
