namespace HomeWork.Api.DTO_s
{
    public class CourseStatsDto
    {
        public string CourseTitle { get; set; } = null!;
        public int StudentsCount { get; set; }
        public decimal TotalPayments { get; set; }
    }
}
