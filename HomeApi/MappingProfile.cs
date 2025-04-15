using AutoMapper;
using HomeApi.Configuration;
using HomeApi.Contracts.Models.Devices;
using HomeApi.Contracts.Models.Home;
using HomeApi.Contracts.Models.Rooms;
using HomeApi.Data.Models;

namespace HomeApi;



public class MappingProfile : Profile
{
     public MappingProfile()
     {
          // Маппинг адреса
          CreateMap<Adress, AddressInfo>();
          
          // Маппинг основных настроек с преобразованием адреса
          CreateMap<HomeOptions, InfoResponse>()
               .ForMember(m => m.AddressInfo, opt => 
                    opt.MapFrom(src => src.Adress ));
          
          // Маппинг запроса добавления устройства
          CreateMap<AddDeviceRequest, Device>()
               .ForMember(d => d.Location, opt =>
                         opt.MapFrom(r => r.RoomLocation));
          
          // Маппинг запроса добавления комнаты
          CreateMap<AddRoomRequest, Room>();
          
          // Маппинг устройства для отображения
          CreateMap<Device, DeviceView>();
          //Маппинг для редактирования комнаты
          CreateMap<EditRoomRequest, Room>()
               .ForMember(r => r.Name,opt=>
                    opt.MapFrom((src => src.NewName)));
     }
}