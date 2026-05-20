namespace Com.Example.Approval.Api.Models
{
    public class Document
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string Status { get; set; } = "รออนุมัติ"; // มี 3 สถานะ: รออนุมัติ, อนุมัติ, ไม่อนุมัติ
    }
}