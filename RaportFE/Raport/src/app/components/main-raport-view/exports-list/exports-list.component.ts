import { RaportContextService } from './../raport-context.service';
import { ExportsService,} from './../../../services/exports.service';
import {AfterViewInit, Component, OnInit, ViewChild, effect} from '@angular/core';
import {MatPaginator, MatPaginatorModule} from '@angular/material/paginator';
import {MatTableDataSource, MatTableModule} from '@angular/material/table';
import { ActivatedRoute, Router } from '@angular/router';
import { ExportsForListInterface } from 'src/app/interfaces/export-for-list-interface';
import { ExportsInterface } from 'src/app/interfaces/exports-interface';
import { NotificationService } from 'src/app/shared/notification/services/notification.service';
import { Notification } from 'src/app/shared/notification/models/notification.model';

@Component({
  selector: 'app-exports-list',
  templateUrl: './exports-list.component.html',
  styleUrls: ['./exports-list.component.scss']
})
export class ExportsListComponent implements OnInit {
  exports?: ExportsInterface;
  isExportsLoaded?: boolean;
  isNoContent?: boolean;
  isContentActive?: boolean;
  lengthOfExportsList: number = 0;
  pageSize: number = 10;
  pageNumberPag: number = 0;
  pageNumber: number = 1;
  pageSizeOptions: number[] = [10, 15, 20, 30, 50];
  locationName?: string;
  startDate?: string;
  endDate?: string;

  displayedColumns: string[] = ['Name', 'Date', 'Time', 'UserName', 'LocationName'];
  dataSource = new MatTableDataSource<ExportsForListInterface>();

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private exportsService: ExportsService,
    private notificationService: NotificationService,
    private raportContextService: RaportContextService,
  ){
    effect(() => {
      this.locationName = this.raportContextService.selectedLocation();
      this.startDate = this.raportContextService.startDate();
      this.endDate = this.raportContextService.endDate();
      if(this.raportContextService.fetchData()){
        this.fetchData()
      }
    })
  }

  ngOnInit(): void {
    this.route.params.subscribe( params => {
      this.pageNumber = params['page'];
      this.pageSize = params['range'];
      this.pageNumberPag = this.pageNumber - 1;
    });
    this.fetchData();
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
  }

  fetchData(){
    this.isExportsLoaded = false;
    this.exportsService.fetchExports(this.pageNumber, this.pageSize, this.locationName, this.startDate, this.endDate).subscribe(
      res => {
        this.exports = res;
        this.dataSource = new MatTableDataSource<ExportsForListInterface>(this.exports.Exports);
        this.lengthOfExportsList = this.exports.NumberOfItems;
        this.isExportsLoaded = true;
        this.raportContextService.fetchData.set(false)
      },
      err => {
        this.isExportsLoaded = true
        this.isNoContent = true;
        let notification = new Notification({
          message: "Raport data fetch failed!",
          type: "error"
         });
        this.notificationService.errorSignal.set(notification)
        this.raportContextService.fetchData.set(false)
      }
    )
  }

  public loadNextPage(e: any) {
    this.pageNumber = e.pageIndex + 1;
    this.pageSize = e.pageSize;
    this.naviagte();
    this.fetchData()
  }

  naviagte(){
    return this.router.navigate(['raport/'+this.pageNumber+"/"+this.pageSize]);
  }

}


