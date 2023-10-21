using MediaCollectionMVC.Models;
using System.Collections;

namespace MediaCollectionMVC
{
    public class ScannedMediumViewModel
    {
        public List<ScannedMedium> ScannedMediaObjects { get; set; }
        public PaginationModel Pagination { get; set; }
    }
}
