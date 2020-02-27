using AutoMapper;

namespace WissAppWebApi
{
    public class Mapping
    {
        public static readonly Mapper mapper;

        static Mapping()
        {
            MappingConfig mappingConfig = new MappingConfig();
            mapper = new Mapper(MappingConfig.mapperConfiguration);
        }
    }
}