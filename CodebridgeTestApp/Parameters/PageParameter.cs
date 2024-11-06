namespace CodebridgeTestApp.Parameters
{
    public class PageParameter
    {
        public int PageNumber { get; init; } = 1; 
        public int PageSize { get; init; } = int.MaxValue;

        public PageParameter() { }
        public PageParameter(int pageNumber = 1, int pageSize = int.MaxValue) { PageNumber = pageNumber; PageSize = pageSize; }

        public bool IsValid()
        {
            return PageNumber > 0 && PageSize > 0;
        }

    }
}
