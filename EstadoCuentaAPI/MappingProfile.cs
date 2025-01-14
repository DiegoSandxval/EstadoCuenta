namespace EstadoCuentaAPI
{
    using AutoMapper;
    using EstadoCuentaAPI.Models;
    using EstadoCuentaAPI.Dtos;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Tarjeta, TarjetaDTO>();
            CreateMap<Movimiento, MovimientoDTO>();
        }
    }

}
