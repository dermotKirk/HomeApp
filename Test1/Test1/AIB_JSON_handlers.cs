using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIB.JSON.Handlers
{

    public class SearchResults
    {
        public string Id { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string GeoHash { get; set; }
        public string GeoDirectoryMatch { get; set; }
        public string DisplayName { get; set; }
        public string Category { get; set; }
        public float SquareMetres { get; set; }
        public float Acres { get; set; }
        public int Range { get; set; }
        public string InfoLink { get; set; }
    }

    public class AmenitiesResultEnvelope
    {
        public string ResultSetType { get; set; }
        public DateTime SearchTime { get; set; }
        public string SearchQuery { get; set; }
        public int TotalResults { get; set; }
        public SearchResults[] SearchResults { get; set; }
    }



    public class bootstrapData
    {
        public County[] Counties { get; set; }
        public string[] AmenityFilters { get; set; }
        public string[] PropertyDetailFilters { get; set; }
    }

    public class County
    {
        public string Name { get; set; }
        public string[] Localities { get; set; }
    }


    public class Budget_Savings_JSON
    {
        public int Duration { get; set; }
        public bool FirstTimeBuyer { get; set; }
        public float TargetPurchasePrice { get; set; }
        public float TargetDeposit { get; set; }
        public object GiftAmount { get; set; }
        public float TargetSavings { get; set; }
        public bool IsInvalid { get; set; }
        public object InvalidMessage { get; set; }
    }


    public class PropertySummarySearchResultWrapper
    {
        public DateTime SearchTime { get; set; }
        public string SearchQuery { get; set; }
        public PropertySummarySearchResult[] PropertySummarySearchResults { get; set; }
    }

    public class PropertySummarySearchResult
    {
        public string CriteriaName { get; set; }
        public string Criteria { get; set; }
        public string SubCriteriaName { get; set; }
        public string SubCriteria { get; set; }
        public int RowCount { get; set; }
        public object Rows { get; set; }
    }

    public class PropertySearchResultAreaWrapper
    {
        public string ResultSetType { get; set; }
        public DateTime SearchTime { get; set; }
        public string SearchQuery { get; set; }
        public int TotalResults { get; set; }
        public POIResult[] SearchResults { get; set; }
    }

    public class POIResult
    {
        public string Id { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string GeoHash { get; set; }
        public decimal Acres { get; set; }
        public string DisplayName { get; set; }
        public string Category { get; set; }
        public float Price { get; set; }
        public int Range { get; set; }
        public string InfoLink { get; set; }
    }

    public class PropertyDetails
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string GeoHash { get; set; }
        public object GeoDirectoryMatch { get; set; }
        public string DisplayName { get; set; }
        public string Category { get; set; }
        public string FullAddress { get; set; }
        public string ExternalReferrer { get; set; }
        public string ExternalRef { get; set; }
        public string ExternalUrl { get; set; }
        public object Agent { get; set; }
        public object Floorspace { get; set; }
        public string Description { get; set; }
        public string BerRating { get; set; }
        public string BerCode { get; set; }
        public float BerEpi { get; set; }
        public string PropertyType { get; set; }
        public string HouseType { get; set; }
        public string SellingType { get; set; }
        public string PriceType { get; set; }
        public string Agreed { get; set; }
        public string Priority { get; set; }
        public string Price { get; set; }
        public string Bedrooms { get; set; }
        public string Bathrooms { get; set; }
        public string TaxSection { get; set; }
        public float SquareMetres { get; set; }
        public float Acres { get; set; }
        public string Address { get; set; }
        public string Area { get; set; }
        public string County { get; set; }
        public string City { get; set; }
        public string GeneralArea { get; set; }
        public object Postcode { get; set; }
        public string AgencyName { get; set; }
        public string ContactName { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Phoneinfo { get; set; }
        public string MainEmail { get; set; }
        public string CcEmail { get; set; }
        public string AuctionAddress { get; set; }
        public string StartDate { get; set; }
        public string ListingDate { get; set; }
        public string AgreedDate { get; set; }
        public string AuctionDate { get; set; }
        public string SmallThumbnailUrl { get; set; }
        public string MediumThumbnailUrl { get; set; }
        public string LargeThumbnailUrl { get; set; }
        public string Tags { get; set; }
        public object Features { get; set; }
        public AdditionalDetails AdditionalDetails { get; set; }
        public Links Links { get; set; }
    }

    public class AdditionalDetails
    {
    }

    public class Links
    {
    }


    public class SavingsBudget
    {
        public int Duration { get; set; }
        public bool FirstTimeBuyer { get; set; }
        public decimal TargetPurchasePrice { get; set; }
        public decimal TargetDeposit { get; set; }
        public decimal GiftAmount { get; set; }
        public decimal TargetSavings { get; set; }
        public bool IsInvalid { get; set; }
        public string InvalidMessage { get; set; }
    }

    public class StressBudget
    {
        public float Price { get; set; }
        public float Deposit { get; set; }
        public float MonthlySavings { get; set; }
        public float MonthlyRent { get; set; }
        public int MortgageTerm { get; set; }
    }

    public class BudgetStressResponse
    {
        public StressBudget BudgetStressRequest { get; set; }
        public decimal StressRate { get; set; }
        public decimal StressAmount { get; set; }
        public bool PassedStressTest { get; set; }
        public Ratesforprice[] RatesForPrice { get; set; }
    }

    public class Ratesforprice
    {
        public decimal PropertyPrice { get; set; }
        public int TermInYears { get; set; }
        public decimal StressPercent { get; set; }
        public decimal StressMonthlyAmount { get; set; }
    }

    public class ErrorResponse
    {
        public string ErrMessage { get; set; }
    }

}
