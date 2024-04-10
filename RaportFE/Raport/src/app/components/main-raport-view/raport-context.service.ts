import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class RaportContextService {

  startDate = signal<string | undefined>(undefined);
  endDate = signal<string | undefined>(undefined);
  selectedLocation = signal<string | undefined>(undefined);
  fetchData = signal<boolean>(false)

constructor() { }

}
