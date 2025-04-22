namespace SurvayBasket.Abstractions
{
    public class PaginationList<T>
    {

        public PaginationList(List<T> items, int PageNum ,int count ,  int PageSize  )
        {
            Items = items;
            PageNumber = PageNum;
            TotalPages = (int)Math.Ceiling(count / (double)PageSize); 
        }
        public List<T> Items { get;  private set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => PageNumber > 1; 
        public bool HasNextPage => PageNumber < TotalPages;

        public static async Task<PaginationList<T>> CreateAsync(IQueryable<T> source , int PageNum , int PageSize , CancellationToken cancellationToken = default!)
        {
            var count = await source.CountAsync();

            var respose = await source.Skip((PageNum-1)*PageSize).Take(PageSize).ToListAsync(cancellationToken);

            return new PaginationList<T> (respose, PageNum , count, PageSize);


        }

    }
}
