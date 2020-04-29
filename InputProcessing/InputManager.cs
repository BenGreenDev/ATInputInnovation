using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Linq;

namespace InputProcessing
{
    public class InputManager
    {
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public IntPtr targetWindow;

        //Incoming instructions that have passed through the model has been instantiated as an instruction
        Queue<Instruction> incomingInstructions = new Queue<Instruction>();
        List<Instruction> liveInstructions = new List<Instruction>();

        Thread inputProcessingThread;
        bool isThreadActive = true;

        UserInputProfile userInputProfile;

        public void SetControlScheme(UserInputProfile _profile)
        {
            userInputProfile = _profile;
        }

        public void SetTargetWindow(IntPtr window)
        {
            targetWindow = window;
            foreach(var instruction in userInputProfile.actions)
            {
                instruction.input.targetWindow = targetWindow;
            }
        }
        public void StartProcessing()
        {
            isThreadActive = true;
            inputProcessingThread = new Thread(Update);
            inputProcessingThread.Start();
        }

        public void EndProcessing()
        {
            foreach(var instruction in userInputProfile.actions)
            {
                instruction.StopProcessing();
            }

            isThreadActive = false;
        }

        void Update()
        {
            while (isThreadActive)
            {
                //Add incomming instructions to active list
                ProcessIncomingInstructions();

                //Remove completed instructions
                CheckForCompletion();
            }
        }

        public void AddStringToIncomingInstructions(string incomingString)
        {
            Instruction incomingInstruction = FindRelevantInstruction(incomingString);

            incomingInstructions.Enqueue(incomingInstruction);

        }

        //We need to stop holding buttons if a release input for that same key is sent
        bool CheckForInstructionOverlap(Instruction incomingInstruction)
        {
            foreach(var instruction in liveInstructions)
            {
                switch (instruction.instructionType)
                {
                    case InstructionType.MOUSE:
                        {
                            MouseInput temp = (MouseInput)instruction.input;

                            if (temp.mouseInputType == MouseInputType.PRESS_MOUSE_BUTTON || temp.mouseInputType == MouseInputType.PRESS_AND_RELEASE)
                            {
                                MouseInput tempIncomming = (MouseInput)instruction.input;
                                //If the same key is being held that we want to release
                                if (tempIncomming.clickType == temp.clickType)
                                {
                                    instruction.StopProcessing();
                                    return true;
                                }
                            }

                            break;
                        }
                    case InstructionType.KEYBOARD:
                        {
                            KeyboardInput temp = (KeyboardInput)instruction.input;
                            if(temp.keyboardInputType == KeyboardInputType.PRESS_KEY || temp.keyboardInputType == KeyboardInputType.PRESS_AND_RELEASE)
                            {
                                KeyboardInput tempIncomming = (KeyboardInput)instruction.input;
                                if(tempIncomming.hotkey.KeyCode == temp.hotkey.KeyCode)
                                {
                                    instruction.StopProcessing();
                                    return true;
                                }
                            }
                            break;
                        }
                }
            }

            return false;
        }

        bool CheckForInstructionDuplication(Instruction newInstruction)
        {
            foreach(var instruction in liveInstructions)
            {
                if(instruction.id == newInstruction.id)
                {
                    instruction.ResetProcessing();
                    return true;
                }
            }

            return false;
        }

        Instruction FindRelevantInstruction(string activationWord)
        {
            return userInputProfile.actions.Where(x => x.activationKeyword == activationWord).FirstOrDefault();
        }

        void ProcessIncomingInstructions()
        {
            if(incomingInstructions.Count > 0)
            {
                Instruction newInstruction = incomingInstructions.Dequeue();
                if (!CheckForInstructionDuplication(newInstruction))
                {
                    CheckForInstructionOverlap(newInstruction);
                    liveInstructions.Add(newInstruction);
                    liveInstructions.Last().BeginProcessing();
                }
            }
        }

        void CheckForCompletion()
        {
            List<int> indexesToRemove = new List<int>();
            
            if(liveInstructions.Count() > 0)
            {
                int i = 0;

                foreach(var instruction in liveInstructions)
                {
                    if(instruction.input.isActive == false)
                    {
                        indexesToRemove.Add(i);
                    }

                    i++;
                }

                foreach(var removalIndex in indexesToRemove)
                {
                    liveInstructions.RemoveAt(removalIndex);
                }
            }
        }
    }
}
