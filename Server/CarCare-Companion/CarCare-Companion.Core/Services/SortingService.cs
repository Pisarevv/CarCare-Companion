using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Enums;
using CarCare_Companion.Core.Models.Search;
using CarCare_Companion.Infrastructure.Data.Models.BaseModels;
using CarCare_Companion.Infrastructure.Data.Models.Contracts;

namespace CarCare_Companion.Core.Services
{
    public class SortingService : ISortingService
    {

        public IQueryable<T> SortCostableRecords<T>(IQueryable<T> records, RecordsSorting sorting) where T : BaseDeletableModel<T>, ICostable
        {
            return sorting switch
            {
                RecordsSorting.Newest => records
                                         .OrderByDescending(r => r.CreatedOn),
                RecordsSorting.Oldest => records
                                         .OrderBy(r => r.CreatedOn),
                RecordsSorting.MostExpensive => records
                                                .OrderByDescending(r => r.Cost),
                RecordsSorting.LeastExpensive => records
                                                .OrderBy(r => r.Cost),
                _ => records
                     .OrderByDescending(r => r.Id)
            };
        }

        public IQueryable<T> SortOptionalCostableRecords<T>(IQueryable<T> records, RecordsSorting sorting) where T : BaseDeletableModel<T>, IOptionalCostable
        {
            return sorting switch
            {
                RecordsSorting.Newest => records
                                         .OrderByDescending(r => r.CreatedOn),
                RecordsSorting.Oldest => records
                                         .OrderBy(r => r.CreatedOn),
                RecordsSorting.MostExpensive => records
                                                .OrderByDescending(r => r.Cost),
                RecordsSorting.LeastExpensive => records
                                                .OrderBy(r => r.Cost),
                _ => records
                     .OrderByDescending(r => r.Id)
            };
        }
        public List<object> AdditionalSortOfAllResults<T>(List<object> records, RecordsSorting sorting)
        {

            return sorting switch
            {
                RecordsSorting.Newest => records
                                         .OrderByDescending(GetSortValueByDate).ToList(),
                RecordsSorting.Oldest => records
                                         .OrderBy(GetSortValueByDate).ToList(),
                RecordsSorting.MostExpensive => records
                                                .OrderByDescending(GetSortValueByCost).ToList(),
                RecordsSorting.LeastExpensive => records
                                                .OrderBy(GetSortValueByCost).ToList(),
                _ => records
                     .OrderByDescending(GetSortValueDefault<T>).ToList()
            };

        }

        private static decimal GetSortValueByCost(object item)
        {
            switch (item)
            {
                case TaxRecordDetailsQueryResponseModel costItem:
                    return costItem.Cost;
                case ServiceRecordDetailsQueryResponseModel costItem:
                    return costItem.Cost;
                case TripDetailsByUserQueryResponseModel optionalCostItem:
                    return optionalCostItem.Cost ?? 0;
                default:
                    throw new InvalidOperationException("Unknown item type");
            }
        }

        private static DateTime GetSortValueByDate(object item)
        {
            switch (item)
            {
                case TaxRecordDetailsQueryResponseModel dateItem:
                    return dateItem.DateCreated;
                case ServiceRecordDetailsQueryResponseModel dateItem:
                    return dateItem.DateCreated;
                case TripDetailsByUserQueryResponseModel dateItem:
                    return dateItem.DateCreated;
                default:
                    throw new InvalidOperationException("Unknown item type");
            }
        }

        private static Guid GetSortValueDefault<T>(object item)
        {
            switch (item)
            {
                case TaxRecordDetailsQueryResponseModel objectItem:
                    return Guid.Parse(objectItem.Id);
                case ServiceRecordDetailsQueryResponseModel objectItem:
                    return Guid.Parse(objectItem.Id);
                case TripDetailsByUserQueryResponseModel objectItem:
                    return Guid.Parse(objectItem.Id);
                default:
                    throw new InvalidOperationException("Unknown item type");
            }
        }
    }
}
