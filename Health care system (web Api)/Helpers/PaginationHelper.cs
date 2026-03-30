namespace Health_care_system__web_Api_.Helpers
{
    public class PaginationHelper
    {
            public static IQueryable<T> ApplyPagination<T>(IQueryable<T> query, int page, int pageSize)
            {
                if (page <= 0) page = 1;
                if (pageSize <= 0) pageSize = 5;
                if (pageSize > 50) pageSize = 50;

                return query.Skip((page - 1) * pageSize).Take(pageSize);
            }
    }
}
