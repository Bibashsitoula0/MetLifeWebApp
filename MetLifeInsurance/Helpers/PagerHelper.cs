namespace MetLifeInsurance.Helpers
{
    public class PagerHelper
    {
        public PagerHelper(int currentPage, long totalPost = 0, int startPage = 0, int endPage = 0, int pageSize = 10)
        {
            long tPages = (int)Math.Ceiling((decimal)totalPost / (decimal)pageSize);
            int cPage = currentPage;
            long sPage = currentPage - 5;
            long ePage = currentPage + 4;
            if (sPage <= 0)
            {
                ePage -= (startPage - 1);
                sPage = 1;
            }
            if (ePage > tPages)
            {
                ePage = (long)tPages;
                if (ePage > 10)
                {
                    sPage = ePage - 9;
                }
            }

            TotalPost = totalPost;
            CurrentPage = cPage;
            PageSize = pageSize;
            TotalPages = tPages;
            StartPage = sPage;
            EndPage = ePage;
        }

        public long TotalPost { get; set; }
        public int CurrentPage { get; set; }
        public long PageSize { get; set; }
        public long TotalPages { get; set; }
        public long StartPage { get; set; }
        public long EndPage { get; set; }
    }
}
