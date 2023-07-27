namespace Talabat.Core.Specefications
{
    public class ProductSpecParams
    {
        private const int MaxPagesize = 10;
        private int pageSize=6 ;
        private string? search;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > MaxPagesize ? MaxPagesize : value; }
        }
         
        public string? Search
        {
            get { return search; }
            set { search = value.ToLower(); }
        }


        public int PageIndex { get; set; } = 1;
        public string? Sort { get; set; }
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }

         

    }
}
