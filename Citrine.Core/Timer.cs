using System;
using System.Diagnostics;

namespace Citrine.Core
{
    public static class Timer
    {
        public static TimeSpan TimeAction(Action action)
        {
            var stopwatch = Stopwatch.StartNew();
            action();
            return stopwatch.Elapsed;
        }
    }
}
