using CarCare_Companion.Core.Enums;
using CarCare_Companion.Infrastructure.Data.Models.BaseModels;
using CarCare_Companion.Infrastructure.Data.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarCare_Companion.Core.Contracts
{
    public interface ISortingService
    {
        public IQueryable<T> SortCostableRecords<T>(IQueryable<T> records, RecordsSorting sorting) where T : BaseDeletableModel<T>, ICostable;

        public IQueryable<T> SortOptionalCostableRecords<T>(IQueryable<T> records, RecordsSorting sorting) where T : BaseDeletableModel<T>, IOptionalCostable;

        public List<object> AdditionalSortOfAllResults<T>(List<object> records, RecordsSorting sorting);
    }
}
