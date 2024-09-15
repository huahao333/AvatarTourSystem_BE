using AutoMapper;
using BusinessObjects.Enums;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.PackageTour;
using BusinessObjects.ViewModels.Supplier;
using Repositories.Interfaces;
using Services.Common;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class PackageTourService : IPackageTourService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PackageTourService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<APIResponseModel> GetPackageToursAsync()
        {
            var list = await _unitOfWork.PackageTourRepository.GetAllAsync();
            var count = list.Count();
            return new APIResponseModel
            {
                Message = $" Found {count} PackageTour ",
                IsSuccess = true,
                Data = list,
            };
        }
        public async Task<APIResponseModel> GetActivePackageToursAsync()
        {
            var list = await _unitOfWork.PackageTourRepository.GetByConditionAsync(s => s.Status != -1);
            var count = list.Count();
            return new APIResponseModel
            {
                Message = $" Found {count} PackageTour ",
                IsSuccess = true,
                Data = list,
            };
        }
        public async Task<PackageTourModel> GetPackageTourByIdAsync(string PackageTourId)
        {
            var packageTour = await _unitOfWork.PackageTourRepository.GetByIdStringAsync(PackageTourId);
            return _mapper.Map<PackageTourModel>(packageTour);
        }

        public async Task<APIResponseModel> CreatePackageTourAsync(PackageTourCreateModel createModel)
        {
            var packageTour = _mapper.Map<PackageTour>(createModel);
            packageTour.PackageTourId = Guid.NewGuid().ToString();
            packageTour.CreateDate = DateTime.Now;
            var result = await _unitOfWork.PackageTourRepository.AddAsync(packageTour);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = " PackageTour Created Successfully",
                IsSuccess = true,
                Data = packageTour,
            };
        }
        public async Task<APIResponseModel> UpdatePackageTourAsync(PackageTourUpdateModel updateModel)
        {
            var existingPackageTour = await _unitOfWork.PackageTourRepository.GetByIdGuidAsync(updateModel.PackageTourId);

            if (existingPackageTour == null)
            {
                return new APIResponseModel
                {
                    Message = "PackageTour not found",
                    IsSuccess = false
                };
            }
            var createDate = existingPackageTour.CreateDate;

            var packageTour = _mapper.Map(updateModel, existingPackageTour);
            packageTour.CreateDate = createDate;
            packageTour.UpdateDate = DateTime.Now;

            var result = await _unitOfWork.PackageTourRepository.UpdateAsync(packageTour);
            _unitOfWork.Save();

            return new APIResponseModel
            {
                Message = "PackageTour Updated Successfully",
                IsSuccess = true,
                Data = packageTour,
            };
        }
        public async Task<APIResponseModel> DeletePackageTour(string PackageTourId)
        {
            var packageTour = await _unitOfWork.PackageTourRepository.GetByIdStringAsync(PackageTourId);
            if (packageTour == null)
            {
                return new APIResponseModel
                {
                    Message = "PackageTour not found",
                    IsSuccess = false
                };
            }
            if (packageTour.Status == -1)
            {
                return new APIResponseModel
                {
                    Message = "PackageTour has been removed",
                    IsSuccess = false
                };
            }
            var createDate = packageTour.CreateDate;
            packageTour.Status = (int?)EStatus.IsDeleted;
            packageTour.CreateDate = createDate;
            packageTour.UpdateDate = DateTime.Now;
            var result = await _unitOfWork.PackageTourRepository.UpdateAsync(packageTour);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = " PackageTour Deleted Successfully",
                IsSuccess = true,
                Data = packageTour,
            };
        }
    }
}
