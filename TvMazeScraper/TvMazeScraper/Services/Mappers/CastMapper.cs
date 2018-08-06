using Omu.ValueInjecter;
using TvMazeScraper.Models;

namespace TvMazeScraper.Services.Mappers
{
    public class CastMapper
    {
        static CastMapper()
        {
            Mapper.AddMap<CastResult, Cast>(source => new Cast()
            {
                Id = source.Person.Id,
                Name = source.Person.Name,
                Birthday = source.Person.Birthday
            });
        }

        public static Cast Map(CastResult source) => Mapper.Map<Cast>(source);
    }
}