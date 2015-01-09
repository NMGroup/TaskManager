using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mephi.Cybernetics.Nm.TaskManager
{
    public class ResultValue
    {

        private object _value;
        
        public object Value
        {
            get
            {
                return _value;
            }
            set { _value = value; }
        }

        public bool TryGetValue(out Object v)
        {
            v = _value;
            return v != null;
        }

        public bool HasValue()
        {
            return _value != null;
        }

        public ResultValue Copy()
        {
            var r = new ResultValue();
            r._value = this._value;
            return r;
        }
    }
}
