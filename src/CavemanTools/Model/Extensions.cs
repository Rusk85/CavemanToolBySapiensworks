namespace CavemanTools.Model
{
    public static class Extensions
    {
        public static Pagination ToPagination(this IPagedInput input, int pageSize = 15)
        {
            return new Pagination(input.Page, pageSize);
        }

    }
}