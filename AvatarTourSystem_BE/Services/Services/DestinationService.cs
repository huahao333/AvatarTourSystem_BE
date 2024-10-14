using AutoMapper;
using BusinessObjects.Enums;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.Destination;
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
    public class DestinationService : IDestinationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly GoogleMapsService _googleMapsService;
        public DestinationService(IUnitOfWork unitOfWork, IMapper mapper, GoogleMapsService googleMapsService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _googleMapsService = googleMapsService;
        }
        public async Task<APIResponseModel> GetActiveDestinationsAsync()
        {
            var list = await _unitOfWork.DestinationRepository.GetByConditionAsync(s => s.Status != -1);
            var count = list.Count();
            return new APIResponseModel
            {
                Message = $" Found {count} Destinations ",
                IsSuccess = true,
                Data = list,
            };
        }

        public async Task<APIResponseModel> GetDestinationsAsync()
        {
            var list = await _unitOfWork.DestinationRepository.GetAllAsync();
            var count = list.Count();
            return new APIResponseModel
            {
                Message = $" Found {count} Destinations ",
                IsSuccess = true,
                Data = list,
            };
        }
        public async Task<APIResponseModel> GetDestinationByIdAsync(string destinationId)
        {
            var destinations = await _unitOfWork.DestinationRepository.GetByConditionAsync(x => x.DestinationId == destinationId);
            if (destinations == null || !destinations.Any())
            {
                return new APIResponseModel
                {
                    Message = "Destination not found.",
                    IsSuccess = false,
                    Data = null
                };
            }
            return new APIResponseModel
            {
                Message = "Get Destination by Destination Id Successfully",
                IsSuccess = true,
                Data = destinations,
            };
        }

        //Embed code 
        public async Task<APIResponseModel> CreateDestinationAsync(DestinationCreateModel createModel)
        {
            //var destination = _mapper.Map<Destination>(createModel);
            //destination.DestinationId = Guid.NewGuid().ToString();
            //destination.CreateDate = DateTime.Now;

            var destination = new Destination
            {
                DestinationId = Guid.NewGuid().ToString(),
                CityId = createModel.CityId,
                DestinationName = createModel.DestinationName,
              //  DestinationImgUrl = createModel.DestinationImgUrl,
                Status = createModel.Status,
                DestinationHotline = createModel.DestinationHotline,
                DestinationGoogleMap = createModel.DestinationGoogleMap,
                DestinationAddress = createModel.DestinationAddress,
                DestinationOpeningHours = createModel.DestinationOpeningHours,
                DestinationClosingHours = createModel.DestinationClosingHours,
                DestinationOpeningDate = createModel.DestinationOpeningDate,
                DestinationClosingDate = createModel.DestinationClosingDate,

                CreateDate = DateTime.Now
            };

            if (!string.IsNullOrEmpty(createModel.DestinationName))
            {
                var embedCode = await _googleMapsService.GetEmbedCodesAsync(createModel.DestinationName);
                destination.DestinationImgUrl = embedCode;
            }

            await _unitOfWork.DestinationRepository.AddAsync(destination);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = " Destination Created Successfully",
                IsSuccess = true,
                Data = destination,
            };
        }


        public async Task<APIResponseModel> UpdateDestinationAsync(DestinationUpdateModel updateModel)
        {
            var existingDestination = await _unitOfWork.DestinationRepository.GetByIdGuidAsync(updateModel.DestinationId);

            if (existingDestination == null)
            {
                return new APIResponseModel
                {
                    Message = "Destination not found",
                    IsSuccess = false
                };
            }
            var createDate = existingDestination.CreateDate;

            var destination = _mapper.Map(updateModel, existingDestination);
            destination.CreateDate = createDate;
            destination.UpdateDate = DateTime.Now;

            var result = await _unitOfWork.DestinationRepository.UpdateAsync(destination);
            _unitOfWork.Save();

            return new APIResponseModel
            {
                Message = "Destination Updated Successfully",
                IsSuccess = true,
                Data = destination,
            };
        }
        public async Task<APIResponseModel> DeleteDestination(string destinationId)
        {
            var destination = await _unitOfWork.DestinationRepository.GetByIdStringAsync(destinationId);
            destination.Status = (int?)EStatus.IsDeleted;
            var result = await _unitOfWork.DestinationRepository.UpdateAsync(destination);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = "Destination Deleted Successfully",
                IsSuccess = true,
                Data = destination,
            };
        }
    }
}
