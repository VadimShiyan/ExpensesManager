namespace ExpensesManager.Domain
{
    public class PagedResult<T>
    {
        public required List<T> Items { get; set; }
        public int TotalCount { get; set; }
    }
}
