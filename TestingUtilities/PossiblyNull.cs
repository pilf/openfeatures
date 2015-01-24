namespace OpenTable.Features.TestingUtilities
{
    public static class PossiblyNull
    {
        private static readonly RandomValueGenerator _randomizer = new RandomValueGenerator();

        public static T? or_null<T>(this T value) where T : struct 
        {
            return _randomizer.random_bool() ? (T?) value : null;
        }

        public static T or_null_object<T>(this T value) where T : class 
        {
            return _randomizer.random_bool() ? value : null;
        }

    }
}