namespace Alsalamony.Application.Common.Models;


public record RequestFilters
{
	public int PageNumber { get; init; } = 1;
	public int PageSize { get; init; } = 10;

}

