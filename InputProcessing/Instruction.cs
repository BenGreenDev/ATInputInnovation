using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace InputProcessing
{
    public enum InstructionType
    {
        MOUSE,
        KEYBOARD
    }

    public class Instruction
    {
        public int priority { get; set; }
        public string activationKeyword { get; set; }
        public string UIInputRepresentation
        {
            get
            {
                if (input != null)
                {
                    return input.GetUIRepresentation();
                }
                else
                {
                    return "Input not setup";
                }
            }
        }

        public Input input = new Input();
        public string id { get; set; }

        public InstructionType instructionType;
        public Instruction(string _activationKeyword, Input _input, int _priority, InstructionType _instructionType)
        {
            instructionType = _instructionType;
            activationKeyword = _activationKeyword;
            input = _input;
            priority = _priority;
            id = Guid.NewGuid().ToString();
        }

        public void BeginProcessing()
        {
            input.StartInput();
        }

        public void StopProcessing()
        {
            input.isActive = false;
            input.StopInput();
        }

        public void ResetProcessing()
        {
            input.ResetInput();
        }
    }
}
