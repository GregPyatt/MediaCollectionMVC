namespace MediaCollectionMVC.Models
{
    public class MediaSortModel  // This class has no constructor, so it's properties are all null by default.
    {
        //public MediaSortModel() { }  // This constructor is required for the model binding to work correctly.

        // "Title", "Authors", "Categories", "PublishedDate", "Publisher", "Pages", "Isbn", "IsRead"
        public string GetSortOrder(string property)  // This method returns the sort order for the given property.
        {
            switch (property)
            {
                case "Title":
                    return Title;
                case "Authors":
                    return Authors;
                case "Categories":
                    return Categories;
                case "PublishedDate":
                    return PublishedDate;
                case "Publisher":
                    return Publisher;
                case "Pages":
                    return Pages;
                case "ISBN":
                    return ISBN;
                case "IsRead":
                    return IsRead;
                default:
                    return "Title_asc";
            }
        }

        public string Title { get; set; } = "Title_asc";  // This is the default sort order.
        public string Authors { get; set; } = "Authors_asc";
        public string Categories { get; set; } = "Categories_asc";
        public string PublishedDate { get; set; } = "PublishedDate_asc";
        public string Publisher { get; set; } = "Publisher_asc";
        public string Pages { get; set; } = "Pages_asc"; 
        public string ISBN { get; set; } = "ISBN_asc";
        public string IsRead { get; set; } = "IsRead_asc";

        public string LastSort { get; set; } = "Title_asc";  // This is the default sort order.
        public string LastSearch { get; set; } = string.Empty;
    }
}
