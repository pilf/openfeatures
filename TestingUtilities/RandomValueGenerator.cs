using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTable.Features.TestingUtilities
{
    public class RandomValueGenerator
    {
        private static Random _random;
        private static RandomValueGenerator _globalInstance;

        public RandomValueGenerator() : this((int)(DateTime.Now.Ticks % int.MaxValue))
        {
        }

        public RandomValueGenerator(int seed)
        {
            _random = new Random(seed);
        }

        public int random_natural_int()
        {
            return _random.Next();
        }

        public long random_natural_long()
        {
            return (long) (_random.NextDouble()*long.MaxValue);
        }

        public bool random_bool()
        {
            return (random_natural_int() % 2) == 1;
        }

        public string random_string_upto_length(int maxLength)
        {
            return random_string_upto_length(0, maxLength);
        }

        public string random_string_upto_length(int minLength, int maxLength)
        {
            var sb = new StringBuilder(100);
	        var length = (random_natural_int() % maxLength);
	        for (var i = minLength; i < length; i++)
                sb.Append((char)(random_natural_int() % 255));
            return sb.ToString();
        }

        public Dictionary<string, string> random_string_string_dictionary(int count)
        {
            return Enumerable
                .Range(1, count + 10) // overselect in case of clashes
                .Select(s => random_string_upto_length(5, 20))
                .Distinct()
                .Select(s => new { key= s, value = random_string_upto_length(100)})
                .Take(count)
                .ToDictionary(k => k.key, v => v.value);
        }

        public DateTime random_date()
        {
            return random_date_time().Date;
        }

        public DateTime random_date_time()
        {
            return DateTime.MinValue.AddTicks(random_natural_long() % DateTime.MaxValue.Ticks);
        }

        public int random_int_inclusive_range(int min, int max)
        {
            return _random.Next(min, max+1);
        }

        public byte random_byte()
        {
            return (byte) random_int_inclusive_range(0, 255);
        }

        public static RandomValueGenerator GlobalInstance()
        {
            return _globalInstance = _globalInstance ?? new RandomValueGenerator();
        }

        public DayOfWeek random_day_of_week()
        {
            return (DayOfWeek) random_int_inclusive_range(0, 6);
        }
    }
}