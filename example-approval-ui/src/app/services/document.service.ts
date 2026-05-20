import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AppConfigService } from '../app.config';

export interface DocumentItem {
  id: number;
  title: string;
  reason: string;
  status: string;
  selected?: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class DocumentService {
  private get apiUrl() {
    return `${AppConfigService.settings.apiUrl}/documents`;
  }

  constructor(private http: HttpClient) {}

  getDocuments(): Observable<DocumentItem[]> {
    return this.http.get<DocumentItem[]>(this.apiUrl);
  }

  bulkUpdateStatus(payload: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/bulk-approval`, payload);
  }
}