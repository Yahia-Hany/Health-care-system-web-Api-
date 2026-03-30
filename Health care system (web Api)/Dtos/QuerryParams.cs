namespace Health_care_system__web_Api_.Dtos
{
    public class QuerryParams
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public string? Search { get; set; }
    }
}

