namespace HomeWork.Api.DTO_s
{
    public class StudentGroupInfo
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; } = null!;
        public string CourseTitle { get; set; } = null!;
        public DateTime StartDate { get; set; }
    }
}
