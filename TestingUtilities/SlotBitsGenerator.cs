namespace OpenTable.Features.TestingUtilities
{
    public class SlotBitsGenerator
    {
        private readonly RandomValueGenerator _gen;

        public SlotBitsGenerator(RandomValueGenerator gen)
        {
            _gen = gen;
        }

        public byte[] GenerateSlotBits()
        {
            return new[] { _gen.random_byte(), _gen.random_byte(), _gen.random_byte(), _gen.random_byte() };
        }

        /// <summary>
        /// Slot 3 exhibits different behaviour from the first two.  If the least signifcant bit is set the whole of the
        /// last byte seems to get ignored.  Presumably this is a bug since it precludes ever having an offer set for 23:45.
        /// However I cannot tell if correcting this would cause more damage due to general lack of test coverage.
        /// PC:20120716
        /// </summary>
        public byte[] GenerateSlot3Bits()
        {
            return new[] { _gen.random_byte(), _gen.random_byte(), _gen.random_byte(), (byte)(_gen.random_byte() & 0xfe) };
        }
    }
}