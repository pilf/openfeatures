using System.Data;

namespace OpenTable.Features.TestingUtilities.Builders.Legacy
{
    public class OfferTypeAttributeValueTableBuilder
    {
        private readonly DataTable _dataTable;

        public OfferTypeAttributeValueTableBuilder()
        {
            _dataTable = new DataTable();
            _dataTable.Columns.Add("RestaurantOfferID", typeof(int));
            _dataTable.Columns.Add("OfferTypeAttributeKeyID", typeof(int));
            _dataTable.Columns.Add("ValueText", typeof(int));
            _dataTable.Columns.Add("ValueInt", typeof(int));
        }

        public DataTable Build()
        {
            return _dataTable;
        }

        public OfferTypeAttributeValueTableBuilder empty_table()
        {
            return this;
        }
    }
}