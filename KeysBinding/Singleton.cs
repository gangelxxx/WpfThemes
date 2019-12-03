#region

using System;
using System.Reflection;

#endregion

namespace KeysBinding
{
    public class Singleton<T> where T : class
    {
        #region Construct

        protected Singleton()
        { }

        #endregion

        #region Public

        public static T Inst => SingletonCreator<T>.CreatorInstance;

        #endregion

        #region  Class

        private sealed class SingletonCreator<TS> where TS : class
        {
            #region Public

            public static TS CreatorInstance { get; } = (TS) typeof(TS).GetConstructor(
                    BindingFlags.Instance | BindingFlags.NonPublic,
                    null,
                    new Type[0],
                    new ParameterModifier[0])
                ?.Invoke(null);

            #endregion
        }

        #endregion
    }
}