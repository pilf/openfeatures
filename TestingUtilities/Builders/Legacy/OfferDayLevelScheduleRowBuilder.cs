using System.Data;

namespace OpenTable.Features.TestingUtilities.Builders.Legacy
{
    public class OfferDayLevelScheduleRowBuilder
    {
        private DataRow _row;
        
        public OfferDayLevelScheduleRowBuilder(DataTable sourceDataTable)
        {
            _row = sourceDataTable.NewRow();
        }

        public DataRow Build()
        {
            return _row;
        }

        public OfferDayLevelScheduleRowBuilder restaurant_offer_id(int restaurantOfferId)
        {
            _row["RestaurantOfferID"] = restaurantOfferId;
            return this;
        }

        public OfferDayLevelScheduleRowBuilder day_of_week(int dow)
        {
            _row["DOW"] = dow;
            return this;
        }

        public OfferDayLevelScheduleRowBuilder max_inventory(int maxInventory)
        {
            _row["MaxInventory"] = maxInventory;
            return this;
        }

        public OfferDayLevelScheduleRowBuilder min_party_size(int minPartySize)
        {
            _row["MinPartySize"] = minPartySize;
            return this;
        }

        public OfferDayLevelScheduleRowBuilder max_party_size(int maxPartySize)
        {
            _row["MaxPartySize"] = maxPartySize;
            return this;
        }

        public OfferDayLevelScheduleRowBuilder slot_bits_1(byte[] slotBits)
        {
            _row["SlotBits1"] = slotBits;
            return this;
        }

        public OfferDayLevelScheduleRowBuilder slot_bits_2(byte[] slotBits)
        {
            _row["SlotBits2"] = slotBits;
            return this;
        }

        public OfferDayLevelScheduleRowBuilder slot_bits_3(byte[] slotBits)
        {
            _row["SlotBits3"] = slotBits;
            return this;
        }

        public OfferDayLevelScheduleRowBuilder using_valid_random_values()
        {
            var slotBitsGenerator = new SlotBitsGenerator(gen());
            var minPartySize = gen().random_int_inclusive_range(1, 19);

            return
                restaurant_offer_id(gen().random_natural_int())
                    .day_of_week(gen().random_int_inclusive_range(1, 7))
                    .max_inventory(gen().random_int_inclusive_range(1, 1000))
                    .slot_bits_1(slotBitsGenerator.GenerateSlotBits())
                    .slot_bits_2(slotBitsGenerator.GenerateSlotBits())
                    .slot_bits_3(slotBitsGenerator.GenerateSlot3Bits())
                    .min_party_size(minPartySize)
                    .max_party_size(gen().random_int_inclusive_range(minPartySize, 20));
        }

        private RandomValueGenerator gen()
        {
            return RandomValueGenerator.GlobalInstance();
        }

        public OfferDayLevelScheduleRowBuilder except()
        {
            return this;
        }
    }
}