using System;
using System.Collections.Generic;
using System.Text;

namespace InputProcessing
{
    public class UserInputProfile
    {
        public List<Instruction> actions = new List<Instruction>();
        public string profileId { get; set; }

        public UserInputProfile()
        {
            profileId = Guid.NewGuid().ToString();
        }
    }
}
