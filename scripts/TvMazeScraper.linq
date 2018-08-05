<Query Kind="Program">
  <NuGetReference>MongoDB.Driver</NuGetReference>
  <NuGetReference>RestSharp</NuGetReference>
  <NuGetReference>ValueInjecter</NuGetReference>
  <Namespace>MongoDB.Driver</Namespace>
  <Namespace>RestSharp</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Omu.ValueInjecter</Namespace>
</Query>

async Task Main()
{
	var show = GetShowWithCast(1);

	var collection = GetShowsCollection();

	await collection.InsertOneAsync(show);
}

Show GetShowWithCast(int id)
{
	var client = new RestClient("https://api.tvmaze.com");

	var showRequest = new RestRequest("shows/{id}", Method.GET);
	showRequest.AddUrlSegment("id", id);

	var showResponse = client.Execute<Show>(showRequest);

	var show = showResponse.Data;

	var castRequest = new RestRequest("shows/{id}/cast", Method.GET);
	castRequest.AddUrlSegment("id", id);

	var castResponse = client.Execute<List<CastResult>>(castRequest);
	var cast = new List<Cast>();

	foreach(var castResult in castResponse.Data)
	{
		cast.Add(CastMapper.Map(castResult));
	}

	show.Cast = cast.OrderByDescending(x => x.Birthday).ToList();

	return show;
}

MongoClient GetClient()
{
	var user = "mdb_usr";
	var password = "g546930umvM<$";
	var database = "tvmazescraper";

	var mongoDbConnectionString = $"mongodb://{user}:{password}@ds113482.mlab.com:13482/{database}";

	var client = new MongoClient(mongoDbConnectionString);

	return client;
}

IMongoCollection<Show> GetShowsCollection()
{
	var db = GetClient().GetDatabase("tvmazescraper");

	var collection = db.GetCollection<Show>("show");

	return collection;
}

public class Show
{
	public int Id { get; set; }
	public string Name { get; set; }

	public IList<Cast> Cast { get; set; }
}

public class Cast
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string Birthday { get; set; }
}

public class CastResult
{
	public PersonResult Person { get; set; }
}

public class PersonResult
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string Birthday { get; set; }
}

public static class CastMapper
{
	static CastMapper()
	{
		Mapper.AddMap<CastResult, Cast>(source =>
		{
			return new Cast()
			{
				Id = source.Person.Id,
				Name = source.Person.Name,
				Birthday = source.Person.Birthday
			};
		});
	}

	public static Cast Map(CastResult source) => Mapper.Map<Cast>(source);
}