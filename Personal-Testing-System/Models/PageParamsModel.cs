namespace Personal_Testing_System.Models
{
    public class PageParamsModel
    {
        //private const int _maxItemsPerPage = 50;

        //public int? PageNumber { get; set; }

        public int? PageNumber { get; set; } = 1;
        public int? ItemsPerPage{ get; set; }

        /*public int? ItemsPerPage
        {
            get => ItemsPerPage;
            set => ItemsPerPage = value > _maxItemsPerPage ? _maxItemsPerPage : value;
        }*/
    }
}
