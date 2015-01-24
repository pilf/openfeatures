using System;
using System.Data;

namespace OpenTable.Features.TestingUtilities.Builders.Legacy
{
    public class OfferDayLevelScheduleTableBuilder
    {
        private readonly DataTable _dataTable;

        public OfferDayLevelScheduleTableBuilder()
        {
            _dataTable = new DataTable();
            _dataTable.Columns.Add("RestaurantOfferID", typeof(int));
            _dataTable.Columns.Add("DOW", typeof(int));
            _dataTable.Columns.Add("Date", typeof(DateTime));
            _dataTable.Columns.Add("MaxInventory", typeof(int));
            _dataTable.Columns.Add("MinPartySize", typeof(int));
            _dataTable.Columns.Add("MaxPartySize", typeof(int));
            _dataTable.Columns.Add("SlotBits1", typeof(byte[]));
            _dataTable.Columns.Add("SlotBits2", typeof(byte[]));
            _dataTable.Columns.Add("SlotBits3", typeof(byte[]));
        }

        public DataTable Build()
        {
            return _dataTable;
        }

        public OfferDayLevelScheduleTableBuilder add_any_valid_schedule()
        {
            return add_schedule_row(o => o.using_valid_random_values());
        }

        public OfferDayLevelScheduleTableBuilder add_schedule_row(Func<OfferDayLevelScheduleRowBuilder, OfferDayLevelScheduleRowBuilder> func)
        {
            var row = func(new OfferDayLevelScheduleRowBuilder(_dataTable)).Build();
            _dataTable.Rows.Add(row);
            return this;
        }

        public OfferDayLevelScheduleTableBuilder empty()
        {
            _dataTable.Rows.Clear();
            return this;
        }
    }
}