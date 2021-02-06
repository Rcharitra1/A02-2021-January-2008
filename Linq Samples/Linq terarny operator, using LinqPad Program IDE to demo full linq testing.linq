<Query Kind="Program">
  <Connection>
    <ID>dafb55b9-c1f3-4196-b502-b211697f4c21</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>Chinook</Database>
  </Connection>
</Query>

void Main()
{
	////list all customers in alphabetic order by last name then first name who
	////  live in the US and have an email of yahoo. List their fullname, email, city and state.
	//
	////query syntax
	//// the code inside the new { ..... } is call the initializer list
	
	//webapp
	string country = "USA";
	string email = "yahoo";
	GetCustomersForCountryEmail(country, email).Dump();
}

//a method located somewhere (maybe a controller)
public IEnumerable<CustomersOfCountryEmail> GetCustomersForCountryEmail(string country, string email)
{
//var resultsA = from x in Customers
//					where x.Country.Contains(country) && x.Email.Contains(email)
//					orderby x.LastName, x.FirstName
//					select new CustomersOfCountryEmail
//					{
//						Name = x.LastName + ", " + x.FirstName,
//						Email = x.Email,
//						City = x.City,
//						State = x.State,
//						Country = x.Country
//					};
//	//within LinqPad to see the contents of a variable
//	//  you will use the LinqPad method .Dump()
//	resultsA.Dump();
	
	//
	////method syntax
	IEnumerable<CustomersOfCountryEmail> resultsB = Customers
		.Where(x => x.Country.Contains(country) && x.Email.Contains(email))
		.OrderBy(x => x.LastName)
		.ThenBy(x => x.FirstName)
		.Select( x => new CustomersOfCountryEmail
				{
					Name = x.LastName + ", " + x.FirstName,
					Email = x.Email,
					City = x.City,
					State = x.State,
					Country = x.Country
				});
	return resultsB;
}
////create an alphbetic list of Albums by ReleaseLabel.
////show the Title and ReleaseLabel
////Missing album labels will be listed as "Unknown"
////Note: ReleaseLabel is nullable
//from x in Albums
//orderby x.ReleaseLabel
//select new
//{
//	Title = x.Title,
//	Label = x.ReleaseLabel == null ? "Unknown": x.ReleaseLabel
//}
//
//Albums
//	.OrderBy(x => x.ReleaseLabel)
//	.Select(x => new
//			{
//				Title = x.Title,
//				Label = x.ReleaseLabel == null ? "Unknown" : x.ReleaseLabel
//			})

////Create an alphabetic list of Albums Stating the
////  album decade for the 70's, 80's and 90's
////List the Title, Year, and its decade
//
//from x in Albums
//where x.ReleaseYear >= 1970 && x.ReleaseYear < 2000
//orderby x.Title
//select new
//{
//	Title = x.Title,
//	Year = x.ReleaseYear,
//	Decade = x.ReleaseYear >= 1970 && x.ReleaseYear < 1980 ? "70's" :
//			 (x.ReleaseYear >= 1980 && x.ReleaseYear < 1990 ? "80's" : "90's")
//}


// You can define other methods, fields, classes and namespaces here

//classes are strongly specified developer-definied datatypes
public class CustomersOfCountryEmail
{
	public string Name{get; set;}
	public string Email{get;set;}
	public string City{get;set;}
	public string State{get;set;}
	public string Country{get;set;}
}




