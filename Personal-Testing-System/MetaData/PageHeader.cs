namespace Personal_Testing_System.MetaData
{
    public class PageHeader
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int TotalCountOfItems { get; private set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public PageHeader(int currentPage, int totalCountOfPages, int itemsPerPage)
        {
            CurrentPage = currentPage;
            TotalCountOfItems = totalCountOfPages;
            TotalPages = (int)Math.Ceiling(TotalCountOfItems / (double)itemsPerPage);
        }
    }
}
