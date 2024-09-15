using AutoMapper;
using BusinessObjects.Enums;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.Ticket;
using BusinessObjects.ViewModels.TicketType;
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
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TicketService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<APIResponseModel> GetTicketsAsync()
        {
            var list = await _unitOfWork.TicketRepository.GetAllAsync();
            var count = list.Count();
            return new APIResponseModel
            {
                Message = $" Found {count} Ticket ",
                IsSuccess = true,
                Data = list,
            };
        }
        public async Task<APIResponseModel> GetActiveTicketsAsync()
        {
            var list = await _unitOfWork.TicketRepository.GetByConditionAsync(s => s.Status != -1);
            var count = list.Count();
            return new APIResponseModel
            {
                Message = $" Found {count} Ticket ",
                IsSuccess = true,
                Data = list,
            };
        }
        public async Task<TicketModel> GetTicketByIdAsync(string TicketId)
        {
            var ticket = await _unitOfWork.TicketRepository.GetByIdStringAsync(TicketId);
            return _mapper.Map<TicketModel>(ticket);
        }

        public async Task<APIResponseModel> CreateTicketAsync(TicketCreateModel createModel)
        {
            var ticket = _mapper.Map<Ticket>(createModel);
            ticket.TicketId = Guid.NewGuid().ToString();
            ticket.CreateDate = DateTime.Now;
            var result = await _unitOfWork.TicketRepository.AddAsync(ticket);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = " Ticket Created Successfully",
                IsSuccess = true,
                Data = ticket,
            };
        }
        public async Task<APIResponseModel> UpdateTicketAsync(TicketUpdateModel updateModel)
        {
            var existingTicket = await _unitOfWork.TicketRepository.GetByIdGuidAsync(updateModel.TicketId);

            if (existingTicket == null)
            {
                return new APIResponseModel
                {
                    Message = "Ticket not found",
                    IsSuccess = false
                };
            }
            var createDate = existingTicket.CreateDate;

            var ticket = _mapper.Map(updateModel, existingTicket);
            ticket.CreateDate = createDate;
            ticket.UpdateDate = DateTime.Now;

            var result = await _unitOfWork.TicketRepository.UpdateAsync(ticket);
            _unitOfWork.Save();

            return new APIResponseModel
            {
                Message = "Ticket Updated Successfully",
                IsSuccess = true,
                Data = ticket,
            };
        }
        public async Task<APIResponseModel> DeleteTicket(string TicketId)
        {
            var ticket = await _unitOfWork.TicketRepository.GetByIdStringAsync(TicketId);
            if (ticket == null)
            {
                return new APIResponseModel
                {
                    Message = "Ticket not found",
                    IsSuccess = false
                };
            }
            if (ticket.Status == -1)
            {
                return new APIResponseModel
                {
                    Message = "Ticket has been removed",
                    IsSuccess = false
                };
            }
            var createDate = ticket.CreateDate;
            ticket.Status = (int?)EStatus.IsDeleted;
            ticket.CreateDate = createDate;
            ticket.UpdateDate = DateTime.Now;
            var result = await _unitOfWork.TicketRepository.UpdateAsync(ticket);
            _unitOfWork.Save();
            return new APIResponseModel
            {
                Message = " Ticket Deleted Successfully",
                IsSuccess = true,
                Data = ticket,
            };
        }
    }
}
