using System;
using System.Collections;
using System.Collections.Generic;

namespace Axial.Lang.Enums
{
    public class Enum2
    {
        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    private class EnumList<T>
    {
        private readonly T _value;
        private readonly string _displayName;

    }
}
