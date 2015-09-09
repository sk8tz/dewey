using System;

namespace Axial.Lang.Internationalization
{
    public class State
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public static implicit operator State(string state)
        {
            var stateSplit = state.Split(':');

            if(stateSplit == null || stateSplit.Length != 2) {
                throw new Exception("State code must be in the format Code:Name");
            }

            return new State {
                Code = stateSplit[0],
                Name = stateSplit[1]
            };
        }
    }
}
