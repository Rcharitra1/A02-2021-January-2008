<Query Kind="Expression">
  <Connection>
    <ID>dafb55b9-c1f3-4196-b502-b211697f4c21</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>Chinook</Database>
  </Connection>
</Query>

//commenting ctrl + KC
//uncomment ctrl + KU

////Method syntax
//Albums
//	.Select(x => x)
//	
////Query syntax
//from x in Albums
//select x

////filtering
////where clause in query syntax
////.Where() method in method syntax
//
////Find all albums released in 1990
//from x in Albums
//where x.ReleaseYear == 1990
//select x
//
//Albums
//	.Where(x => x.ReleaseYear == 1990)
//	.Select(x => x)
//
////Find all albums release in the good old 70's
//from x in Albums
//where x.ReleaseYear < 1980 && x.ReleaseYear >= 1970
//select x
//
//Albums
//	.Where(x => x.ReleaseYear < 1980 && x.ReleaseYear >= 1970)
//	.Select(x => x)

////ordering
////List all albums by descending year of release, in alphabetically order of Title
//from x in Albums
//orderby x.ReleaseYear descending, x.Title
//select x
//
//Albums
//	.OrderByDescending(x => x.ReleaseYear)
//	.ThenBy(x => x.Title)
//	.Select(x => x)

//What about only certain fields (partial entity records or fields from another table)
//List all records from 1970's showing the title, artist name and year

from x in Albums
orderby x.ReleaseYear, x.Title
where x.ReleaseYear < 1980 && x.ReleaseYear >= 1970

select new
{
	Title = x.Title,
	Artist = x.Artist.Name,
	Year = x.ReleaseYear
}

Albums
	.OrderBy(x => x.ReleaseYear)
	.ThenBy(x => x.Title)
	.Where(x => x.ReleaseYear < 1980 && x.ReleaseYear >= 1970)

	.Select(x => new
				{
					Title = x.Title,
					Artist = x.Artist.Name,
					Year = x.ReleaseYear
				})





