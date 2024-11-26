using BusinessObjects.ViewModels.Booking;
using Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IZaloPayService
    {
        bool ValidateCallback(ZaloMiniAppCallback data);
        string GenerateMac(ZaloMiniAppCallback data);
        string GenerateOverallMac(ZaloMiniAppCallback data);
    }
}
