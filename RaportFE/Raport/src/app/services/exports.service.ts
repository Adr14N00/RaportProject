import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment.development';
import { ExportsInterface } from '../interfaces/exports-interface';

@Injectable({
  providedIn: 'root'
})
export class ExportsService {
  baseUrl = environment.RaportApiUrl + '/exports-history';

  constructor(private http: HttpClient) {
  }

  fetchExports(page: number, range: number, locationName?: string, startDate?: string, endDate?: string){;
    let url = this.baseUrl+"?page="+page+"&range="+range

    if (locationName) url += "&locationName=" + locationName;
    if (startDate) url += "&startDate=" + startDate;
    if (endDate) url += "&endDate=" + endDate;

    return this.http.get<ExportsInterface>(url)
  }
}
