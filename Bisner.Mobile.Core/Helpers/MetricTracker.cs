using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Bisner.Mobile.Core.Helpers
{
    public sealed class MetricTracker : IDisposable
    {
        private readonly string _methodName;
        private readonly Stopwatch _stopwatch;

        private MetricTracker(string methodName)
        {
            _methodName = methodName;
            _stopwatch = Stopwatch.StartNew();
        }

        void IDisposable.Dispose()
        {
            _stopwatch.Stop();
            LogToSomewhere();
        }

        private void LogToSomewhere()
        {
            Debug.WriteLine($"{_methodName} : {_stopwatch.ElapsedMilliseconds} ms");
        }

        public static MetricTracker Track([CallerMemberName]string methodName = null)
        {
            return new MetricTracker(methodName);
        }
    }
}