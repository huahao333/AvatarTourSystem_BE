using AutoMapper;
using BusinessObjects.Enums;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.Supplier;
using Org.BouncyCastle.Tls;
using Repositories.Interfaces;
using Services.Common;
using Services.Interfaces;

namespace Services.Services
{
    public class SupplierService : ISupplierService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SupplierService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<APIResponseModel> GetSuppliersAsync()
        {
            var list = await _unitOfWork.SupplierRepository.GetAllAsync();
            var count = list.Count();
            return new APIResponseModel
            {
                Message = $" Found {count} suppliers ",
                IsSuccess = true,
                Data = list,
            };
        }
        public async Task<APIResponseModel> GetActiveSuppliersAsync()
        {
            var list = await _unitOfWork.SupplierRepository.GetByConditionAsync(s => s.Status != -1);
            var count = list.Count();
            return new APIResponseModel
            {
                Message = $" Found {count} suppliers ",
                IsSuccess = true,
                Data = list,
            };
        }
        public async Task<SupplierModel> GetSupplierByIdAsync(string SupplierId)
        {
            var supplier = await _unitOfWork.SupplierRepository.GetByIdStringAsync(SupplierId);
            return _mapper.Map<SupplierModel>(supplier);
        }

        public async Task<APIResponseModel> CreateSupplierAsync(SupplierCreateModel createModel)
        {
            var supplier = _mapper.Map<Supplier>(createModel);
            supplier.SupplierId = Guid.NewGuid().ToString();
            supplier.CreateDate = DateTime.Now;
            var result = await _unitOfWork.SupplierRepository.AddAsync(supplier);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = " Supplier Created Successfully",
                IsSuccess = true,
                Data = supplier,
            };
        }
        public async Task<APIResponseModel> UpdateSupplierAsync(SupplierUpdateModel updateModel)
        {
            var supplier = _mapper.Map<Supplier>(updateModel);
            supplier.UpdateDate = DateTime.Now;
            var result = await _unitOfWork.SupplierRepository.UpdateAsync(supplier);
            _unitOfWork.Save();
            return new  APIResponseModel
            {
                Message = " Supplier Updated Successfully",
                IsSuccess = true,
                Data = supplier,
            };
        }
        public async Task<APIResponseModel> DeleteSupplier(string SupplierId)
        {
            var supplier = await _unitOfWork.SupplierRepository.GetByIdStringAsync(SupplierId);
            supplier.Status = (int?)EStatus.IsDeleted;
            var result = await _unitOfWork.SupplierRepository.UpdateAsync(supplier);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = " Supplier Deleted Successfully",
                IsSuccess = true,
                Data = supplier,
            };
        }
    }
}
