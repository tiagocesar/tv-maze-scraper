using Omu.ValueInjecter;
using TvMazeScraper.Domain.Models;
using TvMazeScraper.Domain.Responses;

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