namespace BugStore.Domain.Common
{
    public abstract class RequestParameters
    {
        private const int MaxPageSize = 100;

        private int _pageSize = 10;
        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }

            set
            {
                _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
            }
        }
    }
}
