using System;
using System.Data;

namespace OpenTable.Features.TestingUtilities.Builders.Legacy
{
    // ReSharper disable InconsistentNaming
    public class ReservationOffersTableBuilder
    {
        private readonly int _forMetroAreaId;
        private readonly DataTable _dataTable;
        private readonly RandomValueGenerator _gen = new RandomValueGenerator();

        public ReservationOffersTableBuilder(int forMetroAreaId)
        {
            _forMetroAreaId = forMetroAreaId;
            _dataTable = new DataTable();
            _dataTable.Columns.Add("RID", typeof(int));
            _dataTable.Columns.Add("MetroAreaID", typeof(int));
            _dataTable.Columns.Add("RName", typeof(string));
            _dataTable.Columns.Add("OfferName", typeof(string));
            _dataTable.Columns.Add("OfferDescription", typeof(string));
            _dataTable.Columns.Add("AverageOverallRating", typeof(decimal));
            _dataTable.Columns.Add("totaldffs", typeof(int));
            _dataTable.Columns.Add("NeighborhoodName", typeof(string));
            _dataTable.Columns.Add("FoodTypeName", typeof(string));
            _dataTable.Columns.Add("TotalSeatedStandardCovers", typeof(int));
            _dataTable.Columns.Add("RestaurantImageThumbnail", typeof(string));
            _dataTable.Columns.Add("Rank", typeof(int));
            _dataTable.Columns.Add("OfferStartDate", typeof(DateTime));
            _dataTable.Columns.Add("OfferEndDate", typeof(DateTime));
            _dataTable.Columns.Add("DisplayOnOTWebsite", typeof(int)); // suprising it's not bool, but there you are
            _dataTable.Columns.Add("OfferClassID", typeof(int));
            _dataTable.Columns.Add("OfferID", typeof(int));
			_dataTable.Columns.Add("NoteToDiners", typeof(string));
			_dataTable.Columns.Add("ExcludesTax", typeof(bool));
			_dataTable.Columns.Add("ExcludesService",  typeof(bool));
			_dataTable.Columns.Add("OfferTypeID", typeof(int));
            _dataTable.Columns.Add("MinPartySize", typeof(int));
            _dataTable.Columns.Add("MaxPartySize", typeof(int));
            _dataTable.Columns.Add("CreatedBy", typeof(string)); 
            _dataTable.Columns.Add("CreatedDtUTC", typeof(DateTime));
            _dataTable.Columns.Add("CurrencyTypeID", typeof(int));
            _dataTable.Columns.Add("OfferPrice", typeof(decimal));
            _dataTable.Columns.Add("UpdatedBy", typeof(string)); 
            _dataTable.Columns.Add("UpdatedDtUTC", typeof(DateTime)); 
        }

        public DataTable Build()
        {
            return _dataTable;
        }

        public ReservationOffersTableBuilder add_valid_offer()
        {
            return add_offer(o => 
                 o.offer_id(_gen.random_natural_int())
                .offer_class_id(_gen.random_natural_int())
                .offer_type_id(_gen.random_natural_int())
                .offer_name(_gen.random_string_upto_length(70))
                .offer_description(_gen.random_string_upto_length(500))
                .note_to_diner(_gen.random_string_upto_length(1000))
                .min_party_size(_gen.random_natural_int())
                .max_party_size(_gen.random_natural_int())
                .excludes_service(_gen.random_bool())
                .excludes_tax(_gen.random_bool())
                .offer_start_date(_gen.random_date())
                .offer_end_date(_gen.random_date())
                .display_on_ot_website(_gen.random_bool())
                .rid(_gen.random_natural_int())
                .created_by(_gen.random_string_upto_length(100))
                .created_dt_utc(_gen.random_date())
                .currency_type_id(_gen.random_natural_int().or_null())
                .offer_price(_gen.random_natural_int().or_null())
                .updated_by(_gen.random_string_upto_length(100).or_null_object())
                .updated_dt_utc(_gen.random_date().or_null()));
        }

        public ReservationOffersTableBuilder add_named_offer_row(string prebuiltTestOfferName)
        {
            switch (prebuiltTestOfferName)
            {
                case "Catch":
                    add_offer(o => o
                        .rid(50)
                        .restaurant_name("Catch")
                        .offer_name("Sunday Roast")
                        .rating_average(4.321m)
                        .total_reviews(37)
                        .neighborhood_name("Castro / Noe / Glen Park")
                        .food_type_name("Seafood")
                        .restaurant_thumb_filename("12345.jpg")
                        .offer_id(14)
                        .in_expected_metro());
                    break;
            }

            return this;
        }

        public ReservationOffersTableBuilder add_offer(Func<ReservationOffersRowBuilder, ReservationOffersRowBuilder> func)
        {
            var row = _dataTable.NewRow();
            func(new ReservationOffersRowBuilder(row, _forMetroAreaId));
            _dataTable.Rows.Add(row);
            return this;
        }
    }

    public class ReservationOffersRowBuilder
    {
        private readonly DataRow _row;
        private readonly int _forMetroAreaId;
        private static int _highestUsedRid = 0;

        public ReservationOffersRowBuilder(DataRow row, int forMetroAreaId)
        {
            _row = row;
            _forMetroAreaId = forMetroAreaId;

            in_expected_metro();
            class_id(4);
            offer_start_date(DateTime.Today);
        }

        public ReservationOffersRowBuilder total_seated_covers(int covers)
        {
            _row["TotalSeatedStandardCovers"] = covers;
            return this;
        }

        public ReservationOffersRowBuilder rid(int rid)
        {
            _row["RID"] = rid;
            if (rid > _highestUsedRid) _highestUsedRid = rid;
            return this;
        }

        public ReservationOffersRowBuilder restaurant_name(string name)
        {
            _row["RName"] = name;
            return this;
        }

        public ReservationOffersRowBuilder offer_name(string offerName)
        {
            _row["OfferName"] = offerName;
            return this;
        }

        public ReservationOffersRowBuilder rating_average(decimal average)
        {
            _row["AverageOverallRating"] = average;
            return this;
        }

        public ReservationOffersRowBuilder total_reviews(int totalReviews)
        {
            _row["totaldffs"] = totalReviews;
            return this;
        }

        public ReservationOffersRowBuilder neighborhood_name(string neighborhoodName)
        {
            _row["NeighborhoodName"] = neighborhoodName;
            return this;
        }

        public ReservationOffersRowBuilder food_type_name(string name)
        {
            _row["FoodTypeName"] = name;
            return this;
        }

        public ReservationOffersRowBuilder restaurant_thumb_filename(string filename)
        {
            _row["RestaurantImageThumbnail"] = filename;
            return this;
        }

        public ReservationOffersRowBuilder metro_id(int metroId)
        {
            _row["MetroAreaID"] = metroId;
            return this;
        }

        public ReservationOffersRowBuilder in_expected_metro()
        {
            _row["MetroAreaID"] = _forMetroAreaId;
            return this;
        }

        public ReservationOffersRowBuilder not_in_expected_metro()
        {
            _row["MetroAreaID"] = _forMetroAreaId + 1;
            return this;
        }

        public ReservationOffersRowBuilder offer_rank(int rank)
        {
            _row["Rank"] = rank;
            return this;
        }

        public ReservationOffersRowBuilder unique_rid()
        {
            return rid(_highestUsedRid + 1);
        }

        public ReservationOffersRowBuilder offer_start_date(DateTime date)
        {
            _row["OfferStartDate"] = date;
            return this;
        }

        public ReservationOffersRowBuilder show_now(bool showNow)
        {
            _row["DisplayOnOTWebsite"] = (showNow) ? 1 : 0;
            return this;
        }

        public ReservationOffersRowBuilder class_id(int classId)
        {
            _row["OfferClassID"] = classId;
            return this;
        }

        public ReservationOffersRowBuilder offer_id(int offerId)
        {
            _row["OfferID"] = offerId;
            return this;
        }

    	public ReservationOffersRowBuilder note_to_diner(string noteToDiner)
    	{
			_row["NoteToDiners"] = noteToDiner;
			return this;
    	}

    	public ReservationOffersRowBuilder include_VAT(bool vat)
    	{
			_row["ExcludesTax"] = vat ? 1 : 0;
			return this;
    	}

    	public ReservationOffersRowBuilder include_service(bool service)
    	{
			_row["ExcludesService"] = service ? 1 : 0;
			return this;
    	}

        public ReservationOffersRowBuilder offer_class_id(int offerClassId)
        {
            _row["OfferClassID"] = offerClassId;
            return this;
        }

        public ReservationOffersRowBuilder offer_type_id(int offerTypeId)
        {
            _row["OfferTypeID"] = offerTypeId;
            return this;
        }

        public ReservationOffersRowBuilder offer_description(string offerDescription)
        {
            _row["OfferDescription"] = offerDescription;
            return this;
        }

        public ReservationOffersRowBuilder min_party_size(int minPartySize)
        {
            _row["MinPartySize"] = minPartySize;
            return this;
        }

        public ReservationOffersRowBuilder max_party_size(int maxPartySize)
        {
            _row["MaxPartySize"] = maxPartySize;
            return this;
        }

        public ReservationOffersRowBuilder excludes_service(bool excludesService)
        {
            _row["ExcludesService"] = excludesService;
            return this;
        }

        public ReservationOffersRowBuilder excludes_tax(bool excludesTax)
        {
            _row["ExcludesTax"] = excludesTax;
            return this;
        }

        public ReservationOffersRowBuilder offer_end_date(DateTime endDate)
        {
            _row["OfferEndDate"] = endDate;
            return this;
        }

        public ReservationOffersRowBuilder display_on_ot_website(bool displayOnOtWebsite)
        {
            _row["DisplayOnOTWebsite"] = displayOnOtWebsite;
            return this;
        }

        public ReservationOffersRowBuilder created_by(string createdBy)
        {
            _row["CreatedBy"] = createdBy;
            return this;
        }

        public ReservationOffersRowBuilder created_dt_utc(DateTime createdDtUtc)
        {
            _row["CreatedDtUTC"] = createdDtUtc;
            return this;
        }

        public ReservationOffersRowBuilder currency_type_id(int? currencyTypeId)
        {
            _row["CurrencyTypeID"] = currencyTypeId.HasValue ? (object) currencyTypeId.Value : DBNull.Value;
            return this;
        }

        public ReservationOffersRowBuilder offer_price(decimal? offerPrice)
        {
            _row["OfferPrice"] = offerPrice.HasValue ? (object) offerPrice.Value : DBNull.Value;
            return this;
        }

        public ReservationOffersRowBuilder updated_by(string updatedBy)
        {
            _row["UpdatedBy"] = (object) updatedBy ?? DBNull.Value;
            return this;
        }

        public ReservationOffersRowBuilder updated_dt_utc(DateTime? updatedDtUtc)
        {
            _row["UpdatedDtUTC"] = updatedDtUtc.HasValue ? (object) updatedDtUtc.Value : DBNull.Value;
            return this;
        }
    }
    // ReSharper restore InconsistentNaming
}