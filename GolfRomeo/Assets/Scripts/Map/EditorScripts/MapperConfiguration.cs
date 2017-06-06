using AutoMapper;

public class MapperConfiguration
{
    public static void Configure()
    {
        Mapper.Initialize(cfg =>
        {
            cfg.CreateMap<Track, TrackDTO>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.Metadata.ID))
                .ForMember(dest => dest.MapName, opt => opt.MapFrom(src => src.Metadata.Name))
                .ForMember(dest => dest.MapObjects, opt => opt.MapFrom(src => src.MapObjects))
                .ForMember(dest => dest.Checkpoints, opt => opt.MapFrom(src => src.LapTracker.Checkpoints))
                .ForMember(dest => dest.Waypoints, opt => opt.MapFrom(src => src.WayPointCircuit.GetComponentsInChildren<WaypointNode>()));

            cfg.CreateMap<TrackDTO, TrackMetadata>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.MapName))
                .ForMember(dest => dest.TrackRecord, opt => opt.MapFrom(src => src.TrackRecord));

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