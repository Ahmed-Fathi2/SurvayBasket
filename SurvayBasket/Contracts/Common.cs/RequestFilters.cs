using System.ComponentModel.DataAnnotations;

namespace SurvayBasket.Contracts.Common.cs
{
    public record RequestFilters
    {  public int PageNum { get; set; } = 1;
      

        [Range(1, 10)]
        public int PageSize { get; set; } = 10;

        public string? SearchValue { get; set; }

        public string? SortColumn { get; init; }
        
        public string? SortDirection { get; init; } = "ASC";
    }
}
