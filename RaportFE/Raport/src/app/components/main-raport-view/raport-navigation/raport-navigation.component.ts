import { RaportContextService } from './../raport-context.service';
import { Component } from '@angular/core';
import {MatDatepickerControl, MatDatepickerInputEvent, MatDatepickerModule, MatDatepickerPanel} from '@angular/material/datepicker';
import {MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import {provideNativeDateAdapter} from '@angular/material/core';

@Component({
  selector: 'app-raport-navigation',
  templateUrl: './raport-navigation.component.html',
  styleUrls: ['./raport-navigation.component.scss']
})
export class RaportNavigationComponent {


locations: string[] = [
  '',
  'Restaurant Le Monde',
  'Hotel Grand',
  'Café Roma',
  'Pizzeria Bella Napoli',
  'Sunny Side Café'
];
selectedLocation?: string;

  constructor(
    private raportContextService: RaportContextService
  ){

  }

  onConfirmClick(){
    this.raportContextService.fetchData.set(true)
  }

  onStartDateChange(e: MatDatepickerInputEvent<Date>){
    const selectedDate: Date | null = e.value;
    if (selectedDate !== null) {
      const year: number = selectedDate.getFullYear();
      const month: number = selectedDate.getMonth() + 1; // Dodajemy 1, ponieważ numeracja miesięcy zaczyna się od 0
      const day: number = selectedDate.getDate();

      const dateString: string = `${year}-${month.toString().padStart(2, '0')}-${day.toString().padStart(2, '0')}`;
      this.raportContextService.startDate.set(dateString)
    }
  }

  onEndDateChange(e: MatDatepickerInputEvent<Date>){
    const selectedDate: Date | null = e.value;
    if (selectedDate !== null) {
      const year: number = selectedDate.getFullYear();
      const month: number = selectedDate.getMonth() + 1; // Dodajemy 1, ponieważ numeracja miesięcy zaczyna się od 0
      const day: number = selectedDate.getDate();

      const dateString: string = `${year}-${month.toString().padStart(2, '0')}-${day.toString().padStart(2, '0')}`;
      this.raportContextService.endDate.set(dateString)
    }
  }

  onLocationChange(){
    this.raportContextService.selectedLocation.set(this.selectedLocation)
  }


}


