using AutoMapper;
using PhotoSharer.Business.Entities;
using PhotoSharer.MVC.ViewModels.Groups;

namespace PhotoSharer.MVC.Helpers
{
    public class EntityMapper
    {
        public static void ConfigureMappings()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<AppGroup, GroupViewModel>()
                    .ForMember(it => it.IsMember, option => option.UseValue(false))
                    .ForMember(it => it.IsCreator, option => option.UseValue(false));

                cfg.CreateMap<AppGroup, GroupListPageItemViewModel>();

                cfg.CreateMap<PhotoStream, PhotoStreamViewModel>();

                cfg.CreateMap<AppGroup, PhotoStreamsListViewModel>()
                    .ForMember(it => it.GroupName, option => option.MapFrom(it => it.Name))
                    .ForMember(it => it.GroupId, option => option.MapFrom(it => it.Id));
            });
        }
    }
}
