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

        public async Task<IEnumerable<StudentDto>> GetStudentsWithGroupsByIdAsync(int studentId)
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
                where s.id = @studentId
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
                new { studentId },
                splitOn: "GroupId");

            return studentDictionary.Values;
        }

        public async Task<IEnumerable<PaymentDto>> GetPaymentsAsync()
        {
            var sql = @"
                select 
                    s.full_name as StudentName,
                    g.group_name as GroupName,
                    c.title as CourseTitle,
                    p.amount as Amount,
                    p.paid_at as PaidAt
                from payments p
                join student_groups sg on sg.id = p.student_group_id
                join students s on s.id = sg.student_id
                join groups g on g.id = sg.group_id
                join courses c on c.id = g.course_id
                order by p.paid_at desc;";

            return await _dbConnection.QueryAsync<PaymentDto>(sql);
        }

        public async Task<IEnumerable<CourseStatsDto>> GetCourseStatsAsync()
        {
            var sql = @"
                select 
                    c.title as CourseTitle,
                    count(distinct sg.student_id) as StudentsCount,
                    coalesce(sum(p.amount), 0) as TotalPayments
                from courses c
                left join groups g on g.course_id = c.id
                left join student_groups sg on sg.group_id = g.id
                left join payments p on p.student_group_id = sg.id
                group by c.title
                order by c.title;";

            return await _dbConnection.QueryAsync<CourseStatsDto>(sql);
        }

        public async Task<IEnumerable<SearchStudentDto>> SearchStudentsAsync(string? name, string? course, decimal? minPayment)
        {
            var sql = @"
                select 
                    s.id as Id,
                    s.full_name as FullName,
                    s.phone,
                    coalesce(sum(p.amount), 0) as TotalPaid,
                    g.id as GroupId,
                    g.group_name as GroupName,
                    c.title as CourseTitle
                from students s
                left join student_groups sg on sg.student_id = s.id
                left join groups g on g.id = sg.group_id
                left join courses c on c.id = g.course_id
                left join payments p on p.student_group_id = sg.id
                where 1=1
            ";

            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(name))
            {
                sql += " and s.full_name ilike @name";
                parameters.Add("name", $"%{name}%");
            }

            if (!string.IsNullOrWhiteSpace(course))
            {
                sql += " and c.title = @course";
                parameters.Add("course", course);
            }

            sql += @"
                group by s.id, s.full_name, s.phone, g.id, g.group_name, c.title
            ";

            if (minPayment.HasValue)
            {
                sql = $"select * from ({sql}) t where t.TotalPaid >= @minPayment";
                parameters.Add("minPayment", minPayment.Value);
            }

            var studentDictionary = new Dictionary<int, SearchStudentDto>();

            var result = await _dbConnection.QueryAsync<SearchStudentDto, StudentGroupInfo, SearchStudentDto>(
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
                parameters,
                splitOn: "GroupId");

            return studentDictionary.Values;
        }

        public async Task<IEnumerable<PaymentDto>> GetMissingAttendanceAsync(DateTime date)
        {
            var sql = @"
                select
                    s.full_name as StudentName,
                    g.group_name as GroupName,
                    c.title as CourseTitle,
                    a.lesson_date as PaidAt
                from attendance a
                join student_groups sg on sg.id = a.student_group_id
                join students s on s.id = sg.student_id
                join groups g on g.id = sg.group_id
                join courses c on c.id = g.course_id
                where a.is_present = false and
                      a.lesson_date = @date;";

            var parameters = new DynamicParameters();
            parameters.Add("date", date.Date, DbType.Date);

            return await _dbConnection.QueryAsync<PaymentDto>(sql, parameters);
        }
    }
}
