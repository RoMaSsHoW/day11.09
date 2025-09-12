namespace HomeWork.Api.DTO_s
{
    public class SearchStudentDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public decimal TotalPaid { get; set; }
        public List<StudentGroupInfo> Groups { get; set; } = new();
    }
}
