using System;
using System.Collections.Generic;
using System.Text;

namespace InputProcessing
{
    public class SaveProfile
    {
        public List<SavedInstruction> savedInstructions = new List<SavedInstruction>();
        public string profileId { get; set; }
    }

    public class SavedInstruction
    {
        Instruction instruction;
        KeyboardInput keyboardInput;
        MouseInput mouseInput;

        public SavedInstruction(Instruction _instruction, KeyboardInput _keyboardInput)
        {
            instruction = _instruction;
            keyboardInput = _keyboardInput;
        }

        public SavedInstruction(Instruction _instruction, MouseInput _mouseInput)
        {
            instruction = _instruction;
            mouseInput = _mouseInput;
        }
    }
}
