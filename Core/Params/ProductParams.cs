namespace Core.Params
{
    public class ProductParams
    {
        public string Sort { get; set; }
        public int? TypeId { get; set; }
        public int? BrandId { get; set; }

        // Max page size, user can gets
        private const int MaxPageSize = 50;
        // Current Page
        public int PageIndex = 1;

        // Limit user gets row.
        private int _pageSize = 6;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        private string _search;
        public string Search
        {
            get => _search;
            set => _search = value.ToLower();
        }
    }
}