namespace MediaCollectionMVC.Models
{
    public class MediaSortModel
    {
        public string TitleSortOrder { get; set; }
        public string AuthorSortOrder { get; set; }
        public string CategorySortOrder { get; set; }
        public string PublishedDateSortOrder { get; set; }
        public string PublisherSortOrder { get; set; }
        public string PagesSortOrder { get; set; }
        public string ISBNSortOrder { get; set; }
        public string IsReadSortOrder { get; set; }
        public string LastSort { get; set; }
    }
}
