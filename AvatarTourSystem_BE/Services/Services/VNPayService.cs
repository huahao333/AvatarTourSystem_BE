using AutoMapper;
using Azure;
using BusinessObjects.Enums;
using MailKit.Search;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
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
    public class VNPaySettings
    {
        public string? vnp_Url { get; set; }
        public string? ReturnUrl { get; set; }
        public string? vnp_TmnCode { get; set; }
        public string? vnp_HashSecret { get; set; }
    }

    public class VNPayService : IVNPayService
    {
        private readonly VNPaySettings _vnPaySettings;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public VNPayService(IOptions<VNPaySettings> vnPaySettings, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _vnPaySettings = vnPaySettings.Value;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<VnPayResponseModel> ConfirmPaymentAsync(IQueryCollection queryString)
        {
            var queryParameters = new Dictionary<string, string>();
            foreach (var key in queryString.Keys)
            {
                queryParameters[key] = queryString[key];
            }

            long orderId = Convert.ToInt64(queryParameters["vnp_TxnRef"]);
            string orderInfor = queryParameters["vnp_OrderInfo"];
            long vnpayTranId = Convert.ToInt64(queryParameters["vnp_TransactionNo"]);
            string vnp_ResponseCode = queryParameters["vnp_ResponseCode"];
            string vnp_SecureHash = queryParameters["vnp_SecureHash"];
            var rawData = new StringBuilder();
            foreach (var key in queryParameters.Keys.OrderBy(k => k))
            {
                if (key != "vnp_SecureHash")
                {
                    rawData.Append($"{key}={queryParameters[key]}&");
                }
            }
            // Remove the trailing '&'
            if (rawData.Length > 0)
            {
                rawData.Length -= 1;
            }

            bool checkSignature = ValidateSignature(rawData.ToString(), vnp_SecureHash, _vnPaySettings.vnp_HashSecret);

            if (checkSignature && _vnPaySettings.vnp_TmnCode == queryParameters["vnp_TmnCode"])
            {
                string id = orderInfor;
                var booking = await _unitOfWork.BookingRepository.GetByIdStringAsync(id);
                if (booking == null)
                {
                    return new VnPayResponseModel
                    {
                        IsSuccess = false,
                        Message = "No booking found",
                    };
                }

                if (vnp_ResponseCode == "00")
                {
                    // Payment successful
                    //booking.Status = (int?)EStatus.IsCompleted;
                    //_unitOfWork.Save();
                    return new VnPayResponseModel
                    {
                        IsSuccess = true,
                        PaymentMethod = "VnPay",
                        OrderDescription = orderInfor,
                        OrderId = orderId.ToString(),
                        TransactionId = vnpayTranId.ToString(),
                        Token = vnp_SecureHash,
                        VnPayResponseCode = vnp_ResponseCode.ToString(),
                    };
                }
                else
                {
                    // Payment failed
                    booking.Status = (int?)EStatus.IsPending;
                    _unitOfWork.Save();
                    return new VnPayResponseModel
                    {
                        Message = $"Payment Failed",
                        IsSuccess = false,
                    };
                }
            }
            else
            {
                return new VnPayResponseModel
                {
                    Message = "Invalid response",
                    IsSuccess = false,
                };
            }
        }

        public async Task<APIGenericResponseModel<string>> CreatePaymentRequestAsync(string bookingId)
        {
            // Check if order exists
            var booking = await _unitOfWork.BookingRepository.GetByIdStringAsync(bookingId);
            if (booking == null)
            {
                return new APIGenericResponseModel<string>
                {
                    Message = "No Booking Found",
                    IsSuccess = false,
                };
            }
            int amount = (int)(booking.TotalPrice ?? 0);
            string orderInfo = DateTime.Now.Ticks.ToString();
            string hostName = System.Net.Dns.GetHostName();
            string clientIPAddress = System.Net.Dns.GetHostAddresses(hostName).GetValue(0).ToString();
            VNPayHelper pay = new VNPayHelper();
            string formattedAmount = amount.ToString() + "00"; // Format amount from DB to match that of VNPay
            pay.AddRequestData("vnp_Version", "2.1.0");
            pay.AddRequestData("vnp_Command", "pay");
            pay.AddRequestData("vnp_TmnCode", _vnPaySettings.vnp_TmnCode);
            pay.AddRequestData("vnp_Amount", formattedAmount);
            pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", "VND");
            pay.AddRequestData("vnp_IpAddr", clientIPAddress);
            pay.AddRequestData("vnp_Locale", "vn");
            pay.AddRequestData("vnp_OrderInfo", booking.BookingId.ToString()); // Use booking id as order info
            pay.AddRequestData("vnp_OrderType", "other");
            pay.AddRequestData("vnp_ReturnUrl", _vnPaySettings.ReturnUrl);
            pay.AddRequestData("vnp_TxnRef", orderInfo);

            string paymentUrl = pay.CreateRequestUrl(_vnPaySettings.vnp_Url, _vnPaySettings.vnp_HashSecret);
            return new APIGenericResponseModel<string>
            {
                Message = "Get URL sucessful!",
                IsSuccess = true,
                Data = paymentUrl,
            };
        }

        private bool ValidateSignature(string rspraw, string inputHash, string secretKey)
        {
            string myChecksum = VNPayHelper.HmacSHA512(secretKey, rspraw);
            return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
