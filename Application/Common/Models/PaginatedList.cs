namespace Alsalamony.Application.Common.Models;
public class PaginatedList<T>
{
	public PaginatedList(List<T> items, int pageNumber, int count, int pageSize)
	{
		Items = items;
		PageNumber = pageNumber;
		TotalPages = (int)Math.Ceiling(count / (double)pageSize);
	}

	public List<T> Items { get; private set; }
	public int PageNumber { get; private set; }
	public int TotalPages { get; private set; }
	public bool HasPreviousPage => PageNumber > 1;
	public bool HasNextPage => PageNumber < TotalPages;

	//public static PaginatedList<T> Create(IList<T> source, int pageNumber, int pageSize)
	//{
	//	var count = source.Count;

	//	return new PaginatedList<T>(source, pageNumber, count, pageSize);

	//}
}