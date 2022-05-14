using AutoMapper;
using APIProject.Service.Models;
using APIProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using APIProject.Service.Models.Authentication;
using APIProject.Service.Utils;
using APIProject.Common.Models.Product;
using APIProject.Common.Models.Category;
using APIProject.Service.Models.Address;
using APIProject.Common.Models.ReceiveAddress;
using APIProject.Common.Models.Cart;
using APIProject.Service.Models.Notification;
using APIProject.Service.Models.Storage;
using APIProject.Common.Models.News;
using APIProject.Common.Models.Users;
using APIProject.Common.Models.ProductStorage;
using APIProject.Service.Models.News;

namespace APIProject.Service
{
    // Cung cấp lớp map Ping to Profile : 
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            MappingEntityToViewModel();
            MappingViewModelToEntity();
        }

        private void MappingEntityToViewModel()
        {
            // case get data
            CreateMap<Customer, RegisterModel>();
            CreateMap<ProductItem, ProductItemModel>()
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => src.CustomerTypeID));
            CreateMap<Product, ProductWebModel>();
            CreateMap<ProductItem, ProductItemModel>()
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => src.CustomerTypeID));
            CreateMap<Product, ProductDetailWebModel>()
            .ForMember(dest => dest.ListProductItem, opt => opt.MapFrom(src => src.ProductItems))
            .ForMember(dest => dest.ListImage, opt => opt.MapFrom(src => src.ProductImages.Select(x => x.ImageUrl)));
            CreateMap<Category, CategoryModel>();

            CreateMap<Province, ProvinceModel>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.Code));
            CreateMap<District, DistrictModel>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.ProvinceID, opt => opt.MapFrom(src => src.ProvinceCode));
            CreateMap<Ward, WardModel>()
                .ForMember(dest => dest.DistrictID, opt => opt.MapFrom(src => src.District_id));
            CreateMap<Cart, CartModel>();
            CreateMap<Notification, NotificationModel>();
            CreateMap<User,UserModel>();
            CreateMap<News,NewsWebModel>();
            CreateMap<News,NewsModel>();
            CreateMap<User, UserInfoModel>();
            CreateMap<ProductStorage, ProductStorageDetailModel>();
        }

        private void MappingViewModelToEntity()
        {
            // case insert or update
            CreateMap<CreateProductModel, Product>()
                .ForMember(dest => dest.ProductItems, opt => opt.MapFrom(src => src.ListProductItem));
            CreateMap<ProductItemModel, ProductItem>()
                .ForMember(dest => dest.CustomerTypeID, opt => opt.MapFrom(src => src.CustomerType));
            CreateMap<UpdateProductModel, Product>();
            CreateMap<CreateCategoryModel, Category>();
            CreateMap<UpdateCategoryModel, Category>();
            CreateMap<AddReceiveAddressModel, ReceiveAddress>();
            CreateMap<UpdateReceiveAddressModel, ReceiveAddress>();
            CreateMap<UpdateCategoryModel, Category>();
            CreateMap<CartModel, Cart>();
            CreateMap<StorageModel, Storage>();

            CreateMap<UpdateNewsInputModel,UpdateNewsModel>();
            CreateMap< UserModel,User>();
        } 
    }
}
