using System.Diagnostics;

namespace Bisner.Mobile.Core
{
    public static class Logger
    {
        public static void Log(string message)
        {
            WriteMessage(message);
        }

        private static void WriteMessage(string message)
        {
            Debug.WriteLine(message);
        }
    }
}