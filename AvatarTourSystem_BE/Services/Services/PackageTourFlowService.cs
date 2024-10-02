using AutoMapper;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.PackageTourFlow;
using Microsoft.EntityFrameworkCore;
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
    public class PackageTourFlowService : IPackageTourFlowService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PackageTourFlowService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<APIResponseModel> CreatePackageTourFlowAsync(FPackageTourCreateModel createModel)
        {
            // Initialize the package tour
            var packageTour = new PackageTour
            {
                PackageTourId = Guid.NewGuid().ToString(), // Generate new ID
                PackageTourName = createModel.PackageTourName,
                CityId = createModel.CityId,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                Status = createModel.Status,
                TourSegments = new List<TourSegment>() // Initialize TourSegments list
            };

            // Initialize total price
            float? totalPrice = 0;

            // Iterate through each destination
            foreach (var destinationModel in createModel.Destinations)
            {
                var destination = new Destination
                {
                    DestinationId = Guid.NewGuid().ToString(), // Generate new ID for Destination
                    DestinationName = destinationModel.DestinationName,
                    CityId = destinationModel.CityId,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    Locations = new List<Location>() // Initialize Locations list
                };

                // Iterate through each location in the destination
                foreach (var locationModel in destinationModel.Locations)
                {
                    var location = new Location
                    {
                        LocationId = Guid.NewGuid().ToString(), // Generate new ID for Location
                        LocationName = locationModel.LocationName,
                        LocationType = locationModel.LocationType,
                        DestinationId = destination.DestinationId, // Link to the destination
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        Services = new List<Service>() // Initialize Services list
                    };

                    // Iterate through each service in the location
                    foreach (var serviceModel in locationModel.Services)
                    {
                        var service = new Service
                        {
                            ServiceId = Guid.NewGuid().ToString(), // Generate new ID for Service
                            ServiceName = serviceModel.ServiceName,
                            ServicePrice = serviceModel.ServicePrice,
                            CreateDate = DateTime.Now,
                            UpdateDate = DateTime.Now,
                        };

                        // Add service price to total price
                        totalPrice += service.ServicePrice;

                        // Add service to the location
                        location.Services.Add(service);
                    }

                    // Add location to the destination
                    destination.Locations.Add(location);
                }

                // Add TourSegment for the destination
                var tourSegment = new TourSegment
                {
                    TourSegmentId = Guid.NewGuid().ToString(), // Generate new ID for TourSegment
                    DestinationId = destination.DestinationId, // Set the destination ID
                    PackageTourId = packageTour.PackageTourId, // Set the package tour ID
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    Status = 1 // Default status
                };

                packageTour.TourSegments.Add(tourSegment);

                await _unitOfWork.DestinationRepository.AddAsync(destination);
            }

            packageTour.PackageTourPrice = totalPrice;

            await _unitOfWork.PackageTourRepository.AddAsync(packageTour);
             _unitOfWork.Save(); 

            // Return the API response
            return new APIResponseModel
            {
                Message = "PackageTour created successfully",
                IsSuccess = true,
                Data = packageTour
              
                
            };
        }

        public Task<APIResponseModel> GetPackageTourFlowAsync()
        {
            throw new NotImplementedException();
        }

        public Task<APIResponseModel> GetPackageTourFlowByIdAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
