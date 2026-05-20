using Com.Example.Approval.Api.Data;
using Com.Example.Approval.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Com.Example.Approval.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DocumentsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var docs = await _context.Documents.OrderBy(d => d.Id).ToListAsync();
            return Ok(docs);
        }

        [HttpPost("bulk-approval")]
        public async Task<IActionResult> BulkApproval([FromBody] UpdateApprovalDto dto)
        {
            if (dto.Status != "อนุมัติ" && dto.Status != "ไม่อนุมัติ")
            {
                return BadRequest(new { message = "สถานะไม่ถูกต้อง" });
            }

            var targets = await _context.Documents
                .Where(d => dto.DocumentIds.Contains(d.Id))
                .ToListAsync();

            foreach (var doc in targets)
            {
                if (doc.Status == "รออนุมัติ")
                {
                    doc.Status = dto.Status;
                    doc.Reason = dto.Reason;
                }
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "อัปเดตสถานะเสร็จสิ้น" });
        }
    }
}