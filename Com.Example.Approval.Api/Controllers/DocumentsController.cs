using Com.Example.Approval.Api.Data;
using Com.Example.Approval.Api.Models;
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
        public async Task<IActionResult> GetDocuments()
        {
            var docs = await _context.Documents.ToListAsync();
            return Ok(docs);
        }

  
        [HttpPost("bulk-approval")]
        public async Task<IActionResult> BulkApproval([FromBody] List<DocumentUpdateDto> updates)
        {
            if (updates == null || !updates.Any()) 
            {
                return BadRequest("ไม่มีข้อมูลส่งมาประมวลผล");
            }

            foreach (var update in updates)
            {
              
                var doc = await _context.Documents.FindAsync(update.Id);
            
                if (doc != null && doc.Status == "รออนุมัติ")
                {
                    doc.Status = update.Status;
                    doc.Reason = update.Reason;
                }
            }

            await _context.SaveChangesAsync();
            
            return Ok(new { message = "บันทึกและอัปเดตสถานะสำเร็จ" });
        }
    }

    public class DocumentUpdateDto
    {
        public int Id { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
    }
}