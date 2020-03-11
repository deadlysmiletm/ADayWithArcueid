using UnityEngine;

namespace ParadoxFramework.Exceptions
{
    public static class ParadoxException
    {
        /// <summary>
        /// Exception utilizado en la librería ParadoxFramework como una extención.
        /// </summary>
        /// <param name="printCall">El Object que llama a la extención.</param>
        /// <param name="message">Mensaje a comunicar en consola.</param>
        /// <param name="type">Tipo de Log a comunicar.</param>
        public static void PrintException(this UnityEngine.Object printCall, string message, LogType type)
        {
            string exception = $"{printCall.GetType().Name}: {message}.";

            switch (type)
            {
                case LogType.Log:
                    Debug.Log(exception);
                    break;
                case LogType.Warning:
                    Debug.LogWarning(exception);
                    break;
                case LogType.Error:
                    Debug.LogError(exception);
                    break;
            }
        }
        /// <summary>
        /// Exception utilizado en la librería ParadoxFramework como una extención.
        /// </summary>
        /// <param name="printCall">El Object que llama a la extención.</param>
        /// <param name="message">Mensaje a comunicar en consola.</param>
        /// <param name="type">Tipo de Log a comunicar.</param>
        public static void PrintException(this object printCall, string message, LogType type)
        {
            string exception = $"{printCall.GetType().Name}: {message}.";

            switch (type)
            {
                case LogType.Log:
                    Debug.Log(exception);
                    break;
                case LogType.Warning:
                    Debug.LogWarning(exception);
                    break;
                case LogType.Error:
                    Debug.LogError(exception);
                    break;
            }
        }
    }
}