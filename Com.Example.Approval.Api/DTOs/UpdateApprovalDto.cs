namespace Com.Example.Approval.Api.DTOs
{
    public class UpdateApprovalDto
    {
        public List<int> DocumentIds { get; set; } = new();
        public string Reason { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty; // "อนุมัติ" หรือ "ไม่อนุมัติ"
    }
}