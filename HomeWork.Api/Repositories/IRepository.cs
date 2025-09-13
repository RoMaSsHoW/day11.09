using HomeWork.Api.DTO_s;

namespace HomeWork.Api.Repositories
{
    public interface IRepository
    {
        Task<IEnumerable<StudentDto>> GetStudentsWithGroupsAsync();
        Task<IEnumerable<StudentDto>> GetStudentsWithGroupsByIdAsync(int studentId);
        Task<IEnumerable<PaymentDto>> GetPaymentsAsync();
        Task<IEnumerable<CourseStatsDto>> GetCourseStatsAsync();
        Task<IEnumerable<SearchStudentDto>> SearchStudentsAsync(string? name, string? course, decimal? minPayment);
        Task<IEnumerable<PaymentDto>> GetMissingAttendanceAsync(DateTime date);
    }
}
