using System.Collections;

namespace MediaCollectionMVC.Models
{
    public class ScannedMediumViewModel
    {
        public List<ScannedMedium> ScannedMediaObjects { get; set; }
        public PaginationModel Pagination { get; set; }
        public MediaSortModel MediaSort { get; set; }
    }
}
