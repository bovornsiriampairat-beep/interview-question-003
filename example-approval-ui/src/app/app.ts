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

    const updatedDocs = this.documents().map(doc => {
      if (doc.selected && doc.status === 'รออนุมัติ') {
        return { 
          ...doc, 
          status: targetStatus, 
          reason: this.modalReason || 'xxxxx', 
          selected: false 
        };
      }
      return doc;
    });

    this.documents.set(updatedDocs);
    this.closeModal();
  }
}