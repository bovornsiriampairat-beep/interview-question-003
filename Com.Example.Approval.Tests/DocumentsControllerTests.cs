using Com.Example.Approval.Api.Controllers;
using Com.Example.Approval.Api.Data;
using Com.Example.Approval.Api.DTOs;
using Com.Example.Approval.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Com.Example.Approval.Tests
{
    public class DocumentsControllerTests
    {
        private AppDbContext CreateDbContextInstance()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        public async Task BulkApproval_ShouldSuccessfullyUpdateStatus_WhenCurrentStatusIsPending()
        {
            var context = CreateDbContextInstance();
            context.Documents.RemoveRange(context.Documents);
            var doc = new Document { Id = 50, Title = "เอกสารจัดซื้อ", Status = "รออนุมัติ", Reason = "" };
            context.Documents.Add(doc);
            await context.SaveChangesAsync();

            var controller = new DocumentsController(context);
            var payload = new UpdateApprovalDto
            {
                DocumentIds = new List<int> { 50 },
                Status = "อนุมัติ",
                Reason = "ตรวจสอบเอกสารแล้วผ่าน"
            };

            var result = await controller.BulkApproval(payload);

            Assert.IsType<OkObjectResult>(result);
            var checkDoc = await context.Documents.FindAsync(50);
            Assert.Equal("อนุมัติ", checkDoc!.Status);
            Assert.Equal("ตรวจสอบเอกสารแล้วผ่าน", checkDoc.Reason);
        }

        [Fact]
        public async Task BulkApproval_ShouldNotOverrideStatus_WhenDocumentIsAlreadyApprovedOrRejected()
        {
            var context = CreateDbContextInstance();
            context.Documents.RemoveRange(context.Documents);
            var doc = new Document { Id = 60, Title = "เอกสารที่ผ่านแล้ว", Status = "อนุมัติ", Reason = "ผ่านรอบแรก" };
            context.Documents.Add(doc);
            await context.SaveChangesAsync();

            var controller = new DocumentsController(context);
            var payload = new UpdateApprovalDto
            {
                DocumentIds = new List<int> { 60 },
                Status = "ไม่อนุมัติ",
                Reason = "พยายามแก้ไขซ้ำซ้อน"
            };

            await controller.BulkApproval(payload);

            var checkDoc = await context.Documents.FindAsync(60);
            Assert.Equal("อนุมัติ", checkDoc!.Status); 
            Assert.Equal("ผ่านรอบแรก", checkDoc.Reason); 
        }
    }
}