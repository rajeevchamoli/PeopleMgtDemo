namespace PeopleMgt.Models
{
    /// <summary>
    /// Params used by client to make data call specific for the view it is called from.
    /// </summary>
    public class PageParameters
    {

        private int pageSize = ConfigurationConstants.DEFAULT_PAGE_SIZE;

        public string SortColumn { get; set; }

        public string SortOrder { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize
        {

            get { return pageSize; }
            set
            {
                pageSize = (value > ConfigurationConstants.MAX_PAGE_SIZE) ? ConfigurationConstants.MAX_PAGE_SIZE : value;
            }
        }

        public string Filter { get; set; }
    }
}
