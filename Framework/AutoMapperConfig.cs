using AutoMapper;
using servicedesk.StatusManagementSystem.Domain;
using servicedesk.StatusManagementSystem.Dto;

namespace servicedesk.StatusManagementSystem.Framework
{
    public class AutoMapperConfig
    {
        public static IMapper InitializeMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<StatusEvent, StatusEventDto>();
                cfg.CreateMap<StatusSource, StatusSourceDto>();
                cfg.CreateMap<Status, StatusDto>();
            });

            return config.CreateMapper();
        }
    }
}