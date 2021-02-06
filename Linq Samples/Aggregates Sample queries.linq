<Query Kind="Statements">
  <Connection>
    <ID>dafb55b9-c1f3-4196-b502-b211697f4c21</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>Chinook</Database>
  </Connection>
</Query>

//List all albums showing their title, artist name, and the number of
//tracks for that album. Show only albums of the 60's. Order by the number of
//tracks from most to least.
//
//how to analyize the questinon
// title -> Albums
// artist name -> Albums.Artist (single value child to parent)
// Count() -> Album.Tracks (collection  parent -> child)
// where ReleaseYear of Albums
var ex1qa = from x in Albums
			where x.ReleaseYear > 1959 && x.ReleaseYear < 1970
			orderby x.Tracks.Count descending
			select new
			{
				Title = x.Title,
				Artist = x.Artist.Name,
				Year = x.ReleaseYear,
				NumberOfTracks = x.Tracks.Count()
			};
//ex1qa.Dump();
var ex1qb = from x in Albums
			where x.ReleaseYear > 1959 && x.ReleaseYear < 1970
			orderby x.Tracks.Count descending
			select new
			{
				Title = x.Title,
				Artist = x.Artist.Name,
				Year = x.ReleaseYear,
				NumberOfTracks = (from y in x.Tracks
								  select y).Count()
			};
//ex1qb.Dump();
var ex1qc = from x in Albums
			where x.ReleaseYear > 1959 && x.ReleaseYear < 1970
			orderby x.Tracks.Count descending
			select new
			{
				Title = x.Title,
				Artist = x.Artist.Name,
				Year = x.ReleaseYear,
				NumberOfTracks = (from y in Tracks
								   where y.AlbumId == x.AlbumId
								  select y).Count()
			};
//ex1qc.Dump();
var ex1ma = Albums
			.Where(x => x.ReleaseYear > 1959 && x.ReleaseYear < 1970)
			.OrderByDescending(x => x.Tracks.Count)
			.Select(x => new
			{
				Title = x.Title,
				Artist = x.Artist.Name,
				Year = x.ReleaseYear,
				NumberOfTracks = x.Tracks.Count()
			});
//ex1ma.Dump();
			
//Produce a list of 60's albums which have tracks showing
//their title, artist, number of tracks on album,
//total price of all tracks on album, the longest album track,
//the shortest album track and the average track length.

//title -> Albums
//artist -> x.Artist....
//#tracks -> x.Tracks.Count
//totalprice -> x.Tracks.Sum(tr.unitprice)
//longtrack -> x.Tracks.Max(tr.milliseconds)
//shorttrack -> x.Tracks.Min(tr.milliseconds)
//avgtrack -> x.Tracks.Average(tr.milliseconds)
//conditions -> which have tracks

var ex2m = Albums
			.Where(x => (x.ReleaseYear > 1959 && x.ReleaseYear < 1970)
				&& (x.Tracks.Count() > 0))
			.Select(x => new
			{
				Title = x.Title,
				Artist = x.Artist.Name,
				Year = x.ReleaseYear,
				NumberOfTracks = x.Tracks.Count(),
				totalprice = x.Tracks.Sum(tr => tr.UnitPrice),
				longestm = x.Tracks.Max(tr => tr.Milliseconds),
				longestq = (from y in x.Tracks
							select y.Milliseconds).Max(),
				longesttrackname = (from y in x.Tracks
									where y.Milliseconds == x.Tracks.Max(tr => tr.Milliseconds)
									select y.Name).FirstOrDefault(),
				shortestq = (from y in x.Tracks
							select y.Milliseconds / 1000.0).Min(),
				averagelength = x.Tracks.Average(tr => tr.Milliseconds)			
			});
ex2m.Dump();






