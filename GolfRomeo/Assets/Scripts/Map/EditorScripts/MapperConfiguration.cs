using AutoMapper;

public class MapperConfiguration
{
    public static void Configure()
    {
        Mapper.Initialize(cfg =>
        {
            cfg.CreateMap<Track, TrackDTO>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.MapName, opt => opt.MapFrom(src => src.Name));

            cfg.CreateMap<TrackDTO, Track>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.MapName));

            cfg.CreateMap<TrackObject, TrackObjectDTO>()
                 .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                 .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                 .ForMember(dest => dest.Rotation, opt => opt.MapFrom(src => src.Rotation));

            cfg.CreateMap<TrackObjectDTO, TrackObject>()
                 .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                 .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                 .ForMember(dest => dest.Rotation, opt => opt.MapFrom(src => src.Rotation));

            cfg.CreateMap<Checkpoint, CheckpointDTO>()
                 .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.CheckpointOrder))
                 .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                 .ForMember(dest => dest.Rotation, opt => opt.MapFrom(src => src.Rotation));

            cfg.CreateMap<CheckpointDTO, Checkpoint>()
                 .ForMember(dest => dest.CheckpointOrder, opt => opt.MapFrom(src => src.Order))
                 .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                 .ForMember(dest => dest.Rotation, opt => opt.MapFrom(src => src.Rotation));

            cfg.CreateMap<WaypointNode, WaypointDTO>()
                 .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position));

            cfg.CreateMap<WaypointDTO, WaypointNode>()
                 .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position));
        });
    }
}