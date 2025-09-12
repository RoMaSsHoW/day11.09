using Dapper;
using HomeWork.Api.DTO_s;
using System.Data;

namespace HomeWork.Api.Repositories
{
    public class Repository : IRepository
    {
        private readonly IDbConnection _dbConnection;

        public Repository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<StudentDto>> GetStudentsWithGroupsAsync()
        {
            var sql = @"
                select 
                    s.id as Id, 
                    s.full_name as FullName, 
                    s.phone,
                    g.id as GroupId, 
                    g.group_name as GroupName, 
                    g.start_date as StartDate,
                    c.title as CourseTitle
                from students s
                left join student_groups sg on sg.student_id = s.id
                left join groups g on g.id = sg.group_id
                left join courses c on c.id = g.course_id
                order by s.id;";

            var studentDictionary = new Dictionary<int, StudentDto>();

            var result = await _dbConnection.QueryAsync<StudentDto, StudentGroupInfo, StudentDto>(
                sql,
                (student, group) =>
                {
                    if (!studentDictionary.TryGetValue(student.Id, out var currentStudent))
                    {
                        currentStudent = student;
                        currentStudent.Groups = new List<StudentGroupInfo>();
                        studentDictionary.Add(currentStudent.Id, currentStudent);
                    }

                    if (group != null && group.GroupId != 0)
                    {
                        currentStudent.Groups.Add(group);
                    }

                    return currentStudent;
                },
                splitOn: "GroupId");

            return studentDictionary.Values;
        }

        public Task<IEnumerable<PaymentDto>> GetPaymentsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CourseStatsDto>> GetCourseStatsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<StudentDto>> SearchStudentsAsync(string? name, string? course, decimal? minPayment)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PaymentDto>> GetMissingAttendanceAsync(DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}
