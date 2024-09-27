using BusinessObjects.ViewModels.Supplier;
using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ISupplierService
    {
        Task<APIResponseModel> GetSuppliersAsync();
        Task<APIResponseModel> GetActiveSuppliersAsync();
        Task<SupplierModel> GetSupplierByIdAsync(string SupplierId);
        Task<APIResponseModel> CreateSupplierAsync(SupplierCreateModel createModel);
        Task<APIResponseModel> UpdateSupplierAsync(SupplierUpdateModel updateModel);
        Task<APIResponseModel> DeleteSupplier(string SupplierId);
    }
}
