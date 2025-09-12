namespace HomeWork.Api.DTO_s
{
    public class PaymentDto
    {
        public string StudentName { get; set; } = null!;
        public string GroupName { get; set; } = null!;
        public string CourseTitle { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTime PaidAt { get; set; }
    }
}
