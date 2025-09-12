namespace HomeWork.Api.DTO_s
{
    public class StudentDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string? Phone { get; set; }
        public List<StudentGroupInfo> Groups { get; set; } = new();
    }
}
