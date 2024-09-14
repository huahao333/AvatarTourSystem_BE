using AutoMapper;
using BusinessObjects.Enums;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.Supplier;
using BusinessObjects.ViewModels.TourSegment;
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
    public class TourSegmentService: ITourSegmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TourSegmentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<APIResponseModel> GetTourSegmentsAsync()
        {
            var list = await _unitOfWork.TourSegmentRepository.GetAllAsync();
            var count = list.Count();
            return new APIResponseModel
            {
                Message = $" Found {count} tourSegments ",
                IsSuccess = true,
                Data = list,
            };
        }

        public async Task<APIResponseModel> GetActiveTourSegmentsAsync()
        {
            var list = await _unitOfWork.TourSegmentRepository.GetByConditionAsync(s => s.Status != -1);
            var count = list.Count();
            return new APIResponseModel
            {
                Message = $" Found {count} tourSegments ",
                IsSuccess = true,
                Data = list,
            };
        }
        public async Task<TourSegmentModel> GetTourSegmentByIdAsync(string TourSegmentId)
        {
            var tourSegments = await _unitOfWork.TourSegmentRepository.GetByIdStringAsync(TourSegmentId);
            return _mapper.Map<TourSegmentModel>(tourSegments);
        }

        public async Task<APIResponseModel> CreateTourSegmentAsync(TourSegmentCreateModel createModel)
        {
            var tourSegment = _mapper.Map<TourSegment>(createModel);
            tourSegment.TourSegmentId = Guid.NewGuid().ToString();
            tourSegment.CreateDate = DateTime.Now;
            var result = await _unitOfWork.TourSegmentRepository.AddAsync(tourSegment);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = " TourSegment Created Successfully",
                IsSuccess = true,
                Data = tourSegment,
            };
        }

        public async Task<APIResponseModel> UpdateTourSegmentAsync(TourSegmentUpdateModel updateModel)
        {
            var tourSegment = _mapper.Map<TourSegment>(updateModel);
            tourSegment.UpdateDate = DateTime.Now;
            var result = await _unitOfWork.TourSegmentRepository.UpdateAsync(tourSegment);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = " TourSegment Updated Successfully",
                IsSuccess = true,
                Data = tourSegment,
            };
        }
        public async Task<APIResponseModel> DeleteTourSegment(string TourSegmentId)
        {
            var tourSegment = await _unitOfWork.TourSegmentRepository.GetByIdStringAsync(TourSegmentId);
            tourSegment.Status = (int?)EStatus.IsDeleted;
            var result = await _unitOfWork.TourSegmentRepository.UpdateAsync(tourSegment);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = " TourSegment Deleted Successfully",
                IsSuccess = true,
                Data = tourSegment,
            };
        }
    }
}
