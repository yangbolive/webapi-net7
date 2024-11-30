using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
namespace webapi.net7.sqlsugar.BLL
{
    public class DxComObject : IDisposable
    {
        private Type _ObjType;

        public object ComInstance;

        public Type ComType
        {
            get
            {
                return _ObjType;
            }
        }

        public object this[string propName]
        {
            get
            {
                return _ObjType.InvokeMember(propName, BindingFlags.GetProperty, null, ComInstance, null);
            }
            set
            {
                _ObjType.InvokeMember(propName, BindingFlags.SetProperty, null, ComInstance, new object[1]
                {
                value
                });
            }
        }

        public object this[string filedName, BindingFlags flags]
        {
            get
            {
                bool flag = 0 == 0;
                return _ObjType.InvokeMember(filedName, flags, null, ComInstance, null);
            }
            set
            {
                bool flag = 0 == 0;
                _ObjType.InvokeMember(filedName, flags, null, ComInstance, new object[1]
                {
                value
                });
            }
        }

        public DxComObject(string ComName)
        {
            _ObjType = Type.GetTypeFromProgID(ComName, false);
            if (_ObjType == null)
            {
                throw new Exception("指定的COM对象名称无效");
            }
            ComInstance = Activator.CreateInstance(_ObjType);
        }

        public object DoMethod(string MethodName, object[] args)
        {
            return ComType.InvokeMember(MethodName, BindingFlags.InvokeMethod, null, ComInstance, args);
        }

        public object DoMethod(string MethodName, object[] args, object[] paramMods)
        {
            ParameterModifier parameterModifier = new ParameterModifier(paramMods.Length);
            for (int i = 0; i < paramMods.Length; i++)
            {
                parameterModifier[i] = Convert.ToBoolean(paramMods[i]);
            }
            ParameterModifier[] modifiers = new ParameterModifier[1]
            {
            parameterModifier
            };
            return ComType.InvokeMember(MethodName, BindingFlags.InvokeMethod, null, ComInstance, args, modifiers, CultureInfo.CurrentCulture, null);
        }

        public void Dispose()
        {
            Marshal.ReleaseComObject(ComInstance);
        }
    }
}