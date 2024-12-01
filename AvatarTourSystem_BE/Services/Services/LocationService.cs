using AutoMapper;
using BusinessObjects.Enums;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.Location;
using BusinessObjects.ViewModels.Rate;
using Repositories.Interfaces;
using Services.Common;
using Services.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class LocationService : ILocationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly GoogleMapsService _googleMapsService;
        public LocationService(IUnitOfWork unitOfWork, IMapper mapper, GoogleMapsService googleMapsService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _googleMapsService = googleMapsService;
        }

        public async Task<APIResponseModel> GetLocationsAsync()
        {
            try
            {
                var list = await _unitOfWork.LocationRepository.GetAllAsync();
                var embedCodeCache = new ConcurrentDictionary<string, string>();

                // Sử dụng SemaphoreSlim để giới hạn số lượng task song song
                var semaphore = new SemaphoreSlim(10);  // Giới hạn 10 task song song
                var tasks = new List<Task>();

                var listLocation = new List<LocationInforViewModel>();

                foreach (var location in list)
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        await semaphore.WaitAsync(); // Đảm bảo rằng chỉ có tối đa 10 task chạy song song
                        try
                        {
                            string embedCode;

                            // Kiểm tra cache trước khi gọi API
                            if (!embedCodeCache.TryGetValue(location.LocationGoogleMap, out embedCode))
                            {
                                embedCode = await _googleMapsService.GetEmbedCodesAsync(location.LocationGoogleMap);
                                embedCodeCache[location.LocationGoogleMap] = embedCode; // Lưu vào cache
                            }

                            var locationInfo = new LocationInforViewModel
                            {
                                LocationId = location.LocationId,
                                LocationName = location.LocationName,
                                LocationImgUrl = location.LocationImgUrl,
                                LocationHotline = location.LocationHotline,
                                Address = location.LocationGoogleMap,
                                LocationGoogleMap = embedCode,
                                LocationClosingHours = location.LocationClosingHours,
                                LocationOpeningHours = location.LocationOpeningHours,
                                DestinationId = location.DestinationId,
                                Status = location.Status,
                            };

                            lock (listLocation)  // Dùng lock để đảm bảo không có vấn đề về đồng bộ khi thêm vào list
                            {
                                listLocation.Add(locationInfo);
                            }
                        }
                        finally
                        {
                            semaphore.Release(); // Giải phóng semaphore sau khi task hoàn thành
                        }
                    }));
                }

                await Task.WhenAll(tasks); // Đợi tất cả các task hoàn thành

                return new APIResponseModel
                {
                    Message = $"Found Locations",
                    IsSuccess = true,
                    Data = listLocation,
                };
            }
            catch (Exception ex)
            {
                return new APIResponseModel
                {
                    Message = $"Error getting Locations: {ex.Message}",
                    IsSuccess = false,
                };
            }
        }
        public async Task<APIResponseModel> GetActiveLocationsAsync()
        {
            var list = await _unitOfWork.LocationRepository.GetByConditionAsync(s => s.Status != -1);
            var count = list.Count();
            return new APIResponseModel
            {
                Message = $" Found {count} Location ",
                IsSuccess = true,
                Data = list,
            };
        }
        public async Task<APIResponseModel> GetLocationByIdAsync(string locationId)
        {
            var location = await _unitOfWork.LocationRepository.GetByIdStringAsync(locationId);
            if (location == null)
            {
                return new APIResponseModel
                {
                    Message = "Location not found",
                    IsSuccess = false
                };
            }
            return new APIResponseModel
            {
                Message = "Location found",
                IsSuccess = true,
                Data = location,
            };
        }

        public async Task<APIResponseModel> CreateLocationAsync(LocationCreateModel createModel)
        {
            try
            {
                var destinationId = await _unitOfWork.DestinationRepository.GetFirstOrDefaultAsync(query => query.Where(d=>d.DestinationId==createModel.DestinationId));
                if (destinationId == null)
                {
                    return new APIResponseModel
                    {
                        Message = "destinationId not found",
                        IsSuccess = false
                    };
                }

                if (string.IsNullOrEmpty(createModel.LocationAddress))
                {
                    return new APIResponseModel
                    {
                        Message = "Address cannot be left blank",
                        IsSuccess = false
                    };
                }
                if (string.IsNullOrEmpty(createModel.LocationName))
                {
                    return new APIResponseModel
                    {
                        Message = "Name location cannot be left blank",
                        IsSuccess = false
                    };
                }

                if (!createModel.LocationClosingHours.HasValue)
                {
                    return new APIResponseModel
                    {
                        Message = "Location closing hours cannot be left blank",
                        IsSuccess = false
                    };
                }

                if (!createModel.LocationOpeningHours.HasValue)
                {
                    return new APIResponseModel
                    {
                        Message = "Location open hours cannot be left blank",
                        IsSuccess = false
                    };
                }

                var destinationOpeningHours = destinationId.DestinationOpeningHours?.TimeOfDay;
                var destinationClosingHours = destinationId.DestinationClosingHours?.TimeOfDay;
                var locationOpeningHours = createModel.LocationOpeningHours?.TimeOfDay;
                var locationClosingHours = createModel.LocationClosingHours?.TimeOfDay;

                if (locationOpeningHours < destinationOpeningHours || locationOpeningHours > destinationClosingHours)
                {
                    return new APIResponseModel
                    {
                        Message = "Location Opening Hours must be within the Destination's opening hours",
                        IsSuccess = false
                    };
                }

                if (locationClosingHours < destinationOpeningHours || locationClosingHours > destinationClosingHours)
                {
                    return new APIResponseModel
                    {
                        Message = "Location Closing Hours must be within the Destination's closing hours",
                        IsSuccess = false
                    };
                }

            //    var embedCode = await _googleMapsService.GetEmbedCodesAsync(createModel.LocationAddress);

             //   var locationGGMap = embedCode;
                var location = new Location
                {
                    LocationId = Guid.NewGuid().ToString(),
                    LocationName = createModel.LocationName,
                    LocationGoogleMap = createModel.LocationAddress,
                    LocationImgUrl = createModel.LocationImgUrl,
                    LocationHotline = createModel.LocationHotline,
                    DestinationId = createModel.DestinationId,
                    LocationOpeningHours = createModel.LocationOpeningHours,
                    LocationClosingHours = createModel.LocationClosingHours,
                    Status = (int)EStatus.Active,
                    CreateDate = DateTime.Now,
                };
                await _unitOfWork.LocationRepository.AddAsync(location);
                _unitOfWork.Save();
                return new APIResponseModel
                {
                    Message = " Location Created Successfully",
                    IsSuccess = true,
                    Data = location,
                };
            }
            catch (Exception ex)
            {
                return new APIResponseModel
                {
                    Message = "Location Google Map is required",
                    IsSuccess = false
                };
            }  
            
        }


        public async Task<APIResponseModel> UpdateLocation(LocationUpdateViewModel locationUpdateViewModel)
        {
            try
            {
                var locationId = await _unitOfWork.LocationRepository.GetFirstOrDefaultAsync(query => query.Where(l=> l.LocationId==locationUpdateViewModel.LocationId));
                if (locationId == null)
                {
                    return new APIResponseModel
                    {
                        Message = "LocationId not found",
                        IsSuccess = false
                    };
                }

                var destinationId = await _unitOfWork.DestinationRepository.GetFirstOrDefaultAsync(query => query.Where(d => d.DestinationId == locationUpdateViewModel.DestinationId));
                if (destinationId == null)
                {
                    return new APIResponseModel
                    {
                        Message = "destinationId not found",
                        IsSuccess = false
                    };
                }

                if (string.IsNullOrEmpty(locationUpdateViewModel.LocationAddress))
                {
                    return new APIResponseModel
                    {
                        Message = "Address cannot be left blank",
                        IsSuccess = false
                    };
                }
                if (string.IsNullOrEmpty(locationUpdateViewModel.LocationName))
                {
                    return new APIResponseModel
                    {
                        Message = "Name location cannot be left blank",
                        IsSuccess = false
                    };
                }

                if (!locationUpdateViewModel.LocationClosingHours.HasValue)
                {
                    return new APIResponseModel
                    {
                        Message = "Location closing hours cannot be left blank",
                        IsSuccess = false
                    };
                }

                if (!locationUpdateViewModel.LocationOpeningHours.HasValue)
                {
                    return new APIResponseModel
                    {
                        Message = "Location open hours cannot be left blank",
                        IsSuccess = false
                    };
                }

                var destinationOpeningHours = destinationId.DestinationOpeningHours?.TimeOfDay;
                var destinationClosingHours = destinationId.DestinationClosingHours?.TimeOfDay;
                var locationOpeningHours = locationUpdateViewModel.LocationOpeningHours?.TimeOfDay;
                var locationClosingHours = locationUpdateViewModel.LocationClosingHours?.TimeOfDay;

                if (locationOpeningHours < destinationOpeningHours || locationOpeningHours > destinationClosingHours)
                {
                    return new APIResponseModel
                    {
                        Message = "Location Opening Hours must be within the Destination's opening hours",
                        IsSuccess = false
                    };
                }

                if (locationClosingHours < destinationOpeningHours || locationClosingHours > destinationClosingHours)
                {
                    return new APIResponseModel
                    {
                        Message = "Location Closing Hours must be within the Destination's closing hours",
                        IsSuccess = false
                    };
                }

                //var embedCode = await _googleMapsService.GetEmbedCodesAsync(locationUpdateViewModel.LocationAddress);

                //var locationGGMap = embedCode;
                
                locationId.LocationName = locationUpdateViewModel.LocationName;
                locationId.LocationGoogleMap = locationUpdateViewModel.LocationAddress;
                locationId.LocationImgUrl = locationUpdateViewModel.LocationImgUrl;
                locationId.LocationHotline = locationUpdateViewModel.LocationHotline;
                locationId.DestinationId = locationUpdateViewModel.DestinationId;
                locationId.LocationClosingHours = locationUpdateViewModel.LocationClosingHours;
                locationId.LocationOpeningHours = locationUpdateViewModel.LocationOpeningHours;
                locationId.Status = locationUpdateViewModel.Status;

                await _unitOfWork.LocationRepository.UpdateAsync(locationId);
                _unitOfWork.Save();
                return new APIResponseModel
                {
                    Message = " Location updated Successfully",
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                return new APIResponseModel
                {
                    Message = "Location Google Map is required",
                    IsSuccess = false
                };
            }

        }

        public async Task<APIResponseModel> UpdateLocationAsync(LocationUpdateModel updateModel)
        {
            var existinglocation = await _unitOfWork.LocationRepository.GetByIdGuidAsync(updateModel.LocationId);

            if (existinglocation == null)
            {
                return new APIResponseModel
                {
                    Message = "Location not found",
                    IsSuccess = false
                };
            }
            var createDate = existinglocation.CreateDate;

            var location = _mapper.Map(updateModel, existinglocation);
            location.CreateDate = createDate;
            location.UpdateDate = DateTime.Now;

            var result = await _unitOfWork.LocationRepository.UpdateAsync(location);
            _unitOfWork.Save();

            return new APIResponseModel
            {
                Message = "Location Updated Successfully",
                IsSuccess = true,
                Data = location,
            };
        }
        public async Task<APIResponseModel> DeleteLocation(string locationId)
        {
            var location = await _unitOfWork.LocationRepository.GetByIdStringAsync(locationId);
            if (location == null)
            {
                return new APIResponseModel
                {
                    Message = "Location not found",
                    IsSuccess = false
                };
            }
            if (location.Status == -1)
            {
                return new APIResponseModel
                {
                    Message = "Location has been removed",
                    IsSuccess = false
                };
            }
            var createDate = location.CreateDate;
            location.Status = (int?)EStatus.IsDeleted;
            location.CreateDate = createDate;
            location.UpdateDate = DateTime.Now;
            var result = await _unitOfWork.LocationRepository.UpdateAsync(location);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = " Location Deleted Successfully",
                IsSuccess = true,
                Data = location,
            };
        }
    }
}
