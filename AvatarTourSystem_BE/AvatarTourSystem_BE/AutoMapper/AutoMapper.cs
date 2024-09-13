using AutoMapper;
using BusinessObjects.Models;
using BusinessObjects.ViewModels.Supplier;

namespace AvatarTourSystem_BE.AutoMapper
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            //Supplier
            CreateMap<Supplier, SupplierModel>().ReverseMap();
            CreateMap<Supplier, SupplierCreateModel>().ReverseMap();
            CreateMap<Supplier, SupplierUpdateModel>().ReverseMap();
        }
    }
}
