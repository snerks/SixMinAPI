using AutoMapper;
using SixMinApi.Dtos;
using SixMinApi.Models;

namespace SixMinApi.Profiles
{
    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            // Source -> Target
            _ = CreateMap<Command, CommandReadDto>();
            _ = CreateMap<CommandCreateDto, Command>();
            _ = CreateMap<CommandUpdateDto, Command>();
        }
    }
}