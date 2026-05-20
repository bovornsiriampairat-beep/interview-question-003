using Com.Example.Approval.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Com.Example.Approval.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Document> Documents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed ข้อมูลจำลองจำนวน 10 รายการตามโจทย์
            modelBuilder.Entity<Document>().HasData(
                new Document { Id = 1, Title = "รายการที่ 1", Reason = "xxxxx", Status = "รออนุมัติ" },
                new Document { Id = 2, Title = "รายการที่ 2", Reason = "xxxxx", Status = "อนุมัติ" },
                new Document { Id = 3, Title = "รายการที่ 3", Reason = "xxxxx", Status = "ไม่อนุมัติ" },
                new Document { Id = 4, Title = "รายการที่ 4", Reason = "xxxxx", Status = "รออนุมัติ" },
                new Document { Id = 5, Title = "รายการที่ 5", Reason = "xxxxx", Status = "อนุมัติ" },
                new Document { Id = 6, Title = "รายการที่ 6", Reason = "xxxxx", Status = "ไม่อนุมัติ" },
                new Document { Id = 7, Title = "รายการที่ 7", Reason = "xxxxx", Status = "รออนุมัติ" },
                new Document { Id = 8, Title = "รายการที่ 8", Reason = "xxxxx", Status = "รออนุมัติ" },
                new Document { Id = 9, Title = "รายการที่ 9", Reason = "xxxxx", Status = "รออนุมัติ" },
                new Document { Id = 10, Title = "รายการที่ 10", Reason = "xxxxx", Status = "รออนุมัติ" }
            );
        }
    }
}