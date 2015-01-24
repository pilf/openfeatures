using System;
using System.Data;

namespace OpenTable.Features.TestingUtilities.Builders.Legacy
{
    public class GetPromotedOfferDatasetBuilder
    {
        private DataTable _restaurantOffersTable;
        private int _metroID = 70;
        private DataTable _offerTypeAttributeValueTable;
        private DataTable _offerDayLevelScheduleTable;

        public DataSet Build()
        {
            var dataset = new DataSet();

            if (_offerDayLevelScheduleTable != null)
                return BuildWith3Tables(dataset);

            if (_offerTypeAttributeValueTable != null)
                return BuildWith2Tables(dataset);

            if (_restaurantOffersTable != null)
                return BuildWith1Table(dataset);
                    
            return dataset;
        }

        public GetPromotedOfferDatasetBuilder empty_result()
        {
            return this;
        }

        public GetPromotedOfferDatasetBuilder with_restaurant_offers(Func<ReservationOffersTableBuilder, ReservationOffersTableBuilder> func)
        {
            _restaurantOffersTable = BuildReservationOffersTable(func);
            return this;
        }
        
        public GetPromotedOfferDatasetBuilder offer_type_attribute_value_table(Func<OfferTypeAttributeValueTableBuilder, OfferTypeAttributeValueTableBuilder> func)
        {
            _offerTypeAttributeValueTable = BuildOfferTypeAttributeValueTable(func);
            return this;
        }

        public GetPromotedOfferDatasetBuilder offer_day_level_schedule_table(Func<OfferDayLevelScheduleTableBuilder, OfferDayLevelScheduleTableBuilder> func)
        {
            _offerDayLevelScheduleTable = BuildOfferDayLevelScheduleTableBuilder(func);
            return this;
        }

        private DataSet BuildWith3Tables(DataSet dataset)
        {
            dataset = BuildWith2Tables(dataset);
            dataset.Tables.Add(
                _offerDayLevelScheduleTable ?? BuildOfferDayLevelScheduleTableBuilder(t => t.add_any_valid_schedule()));

            return dataset;
        }

        private DataSet BuildWith2Tables(DataSet dataset)
        {
            dataset = BuildWith1Table(dataset);
            dataset.Tables.Add(
                _offerTypeAttributeValueTable ?? BuildOfferTypeAttributeValueTable(t => t.empty_table()));

            return dataset;
        }

        private DataSet BuildWith1Table(DataSet dataset)
        {
            dataset.Tables.Add(
                _restaurantOffersTable ?? BuildReservationOffersTable(t => t.add_valid_offer()));

            return dataset;
        }

        private DataTable BuildReservationOffersTable(Func<ReservationOffersTableBuilder, ReservationOffersTableBuilder> func)
        {
            return func(new ReservationOffersTableBuilder(_metroID)).Build();
        }

        private static DataTable BuildOfferTypeAttributeValueTable(Func<OfferTypeAttributeValueTableBuilder, OfferTypeAttributeValueTableBuilder> func)
        {
            return func(new OfferTypeAttributeValueTableBuilder()).Build();
        }

        private static DataTable BuildOfferDayLevelScheduleTableBuilder(Func<OfferDayLevelScheduleTableBuilder, OfferDayLevelScheduleTableBuilder> func)
        {
            return func(new OfferDayLevelScheduleTableBuilder()).Build();
        }
    }
}
