using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mephi.Cybernetics.Nm.TaskManager
{
    public class FuncInfo
    {
        /// <summary> Количество аргументов. </summary>
        private int _arity;
        /// <summary> Тип возвращаемого значения. </summary>
        private Type _returnType;

        private Type[] _argumentsType;
        private Delegate action;
        public Delegate FuncDelegate 
        {
            get
            {
                return action;
            }
            set
            {
                action = value;
            } 
        }

        public int Arity
        {
            get { return _arity; }
        }

        public Type ReturnType 
        {
            get { return _returnType; } 
        }

        public Type[] ArgumentsType
        {
            get { return _argumentsType; }
        }

        public FuncInfo(Delegate inpDelegate)
        {
            if (inpDelegate.Method.ReturnType == typeof (void))
            {
                this._returnType = inpDelegate.Method.ReturnType;
                this._argumentsType = inpDelegate.GetType().GetGenericArguments();
                this._arity = _argumentsType.Length;
            }
            else
            {
                this._argumentsType = inpDelegate.GetType().GetGenericArguments();
                this._returnType = this._argumentsType.Last();
                this._arity = _argumentsType.Length - 1;
            }
            FuncDelegate = inpDelegate;
        }

        public override string ToString()
        {
            return (string.Format("Args type:{0}, length: {1} \n returned:{2}", this.ArgumentsType, this.Arity, this.ReturnType));
        }

    }
}
