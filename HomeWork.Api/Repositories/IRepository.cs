using HomeWork.Api.DTO_s;

namespace HomeWork.Api.Repositories
{
    public interface IRepository
    {
        Task<IEnumerable<StudentDto>> GetStudentsWithGroupsAsync();
        Task<IEnumerable<PaymentDto>> GetPaymentsAsync();
        Task<IEnumerable<CourseStatsDto>> GetCourseStatsAsync();
        Task<IEnumerable<StudentDto>> SearchStudentsAsync(string? name, string? course, decimal? minPayment);
        Task<IEnumerable<PaymentDto>> GetMissingAttendanceAsync(DateTime date);
    }
}
