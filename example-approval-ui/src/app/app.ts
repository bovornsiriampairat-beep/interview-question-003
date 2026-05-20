import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DocumentService } from './services/document.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App implements OnInit {
  title = signal('example-approval-ui');
  
  documents = signal<any[]>([]); 
  showModal = false;
  modalTargetStatus = ''; 
  modalReason = '';

  constructor(private docService: DocumentService) {}

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.docService.getDocuments().subscribe({
      next: (res: any) => {
        const dataArray = Array.isArray(res) ? res : (res.data || []);
        const mappedData = dataArray.map((item: any) => ({
          id: item.id,
          title: item.title,
          reason: item.reason,
          status: item.status,
          selected: false 
        }));
        this.documents.set(mappedData);
      },
      error: (err) => {
        console.error('ไม่สามารถเรียกข้อมูลจากเซิร์ฟเวอร์หลังบ้านได้:', err);
      }
    });
  }

  hasSelectedPending(): boolean {
    return this.documents().some(d => d.selected && d.status === 'รออนุมัติ');
  }

  openModal(status: string): void {
    this.modalTargetStatus = status;
    this.modalReason = ''; 
    this.showModal = true;
  }

  closeModal(): void {
    this.showModal = false;
    this.modalTargetStatus = '';
    this.modalReason = '';
  }

 
  submitStatus(): void {
    const targetStatus = this.modalTargetStatus;
    const currentDocs = this.documents(); 

   
    const updatesPayload = currentDocs
      .filter(doc => doc.selected && doc.status === 'รออนุมัติ')
      .map(doc => ({
        id: doc.id,
        status: targetStatus,
        reason: this.modalReason || 'xxxxx'
      }));

  
    if (updatesPayload.length === 0) {
      this.closeModal();
      return;
    }

   
    this.docService.bulkUpdateStatus(updatesPayload).subscribe({
      next: (response) => {
        console.log('บันทึกข้อมูลและเซฟสถานะสำเร็จ:', response);
    
        this.loadData(); 
        this.closeModal();
      },
      error: (err) => {
        console.error('เกิดข้อผิดพลาดในการบันทึกค่าลงเซิร์ฟเวอร์หลังบ้าน:', err);
        alert('ระบบหลังบ้านเกิดข้อผิดพลาด ไม่สามารถส่งค่าสถานะไปอัปเดตได้');
      }
    });
  }
}