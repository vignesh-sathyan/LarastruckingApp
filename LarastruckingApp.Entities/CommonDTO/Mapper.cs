using AutoMapper;
using System.Collections.Generic;

namespace LarastruckingApp.Entities.Common
{
    public class AutoMapperServices<Source, Destination> : Profile
    {
        public static Destination ReturnObject(Source sourceObject)
        {
            Mapper.Initialize(config => config.CreateMap<Source, Destination>());
            Destination objDestination = Mapper.Map<Destination>(sourceObject);
            return objDestination;
        }
        public static List<Destination> ReturnObjectList(List<Source> listSourceObject)
        {
            Mapper.Initialize(config => config.CreateMap<Source, Destination>());
            List<Destination> objDestinationList = Mapper.Map<List<Destination>>(listSourceObject);
            return objDestinationList;
        }
    }
}
