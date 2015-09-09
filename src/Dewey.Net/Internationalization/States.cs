using System.Collections.Generic;
using System.Linq;

namespace Axial.Lang.Internationalization
{
    public class States : List<State>
    {
        public State GetStateByName(string name)
        {
            return this
                .Where(t => t.Name == name)
                .FirstOrDefault();
        }

        public string GetCodeByName(string name)
        {
            return this
                .Where(t => t.Name == name)
                .FirstOrDefault()?
                .Code;
        }

        public State GetStateByCode(string code)
        {
            return this
                .Where(t => t.Code == code)
                .FirstOrDefault();
        }

        public string GetNameByCode(string code)
        {
            return this
                .Where(t => t.Code == code)
                .FirstOrDefault()?
                .Name;
        }
    }             
}
