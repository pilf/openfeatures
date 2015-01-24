using System;

using NUnit.Framework;

namespace OpenTable.Features.TestingUtilities
{
    public static class AssertException
    {
        public static void ThrowWithMessage(Type exceptionType, Action action, string errorMessage)
        {
            try
            {
                action();
                Assert.Fail(); // no exception rised
            }
            catch (Exception ex)
            {
                if (ex.GetType() == exceptionType && string.Compare(ex.Message, errorMessage) == 0)
                {
                    return;
                }
                Assert.Fail(); // wrong exception
            }
        }

        public static T Catch<T>(Action action) where T : Exception
        {
            try
            {
                action();
            }
            catch (T ex)
            {
                return ex;
            }

            return null; // no exception thrown
        }
    }
}
