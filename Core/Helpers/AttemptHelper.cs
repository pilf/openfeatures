using System;
using System.Threading;

namespace OpenTable.Features.Core.Helpers
{
    public static class AttemptHelper
    {
        public static bool AttemptWaitRepeat(Func<bool> boolMethodToCall)
        {
            for (var i = 0; i < 5; i++)
            {
                var outcome = boolMethodToCall();
                if (outcome) return true;
                Thread.Sleep(1000);
            }

            return false;
        }
    }
}