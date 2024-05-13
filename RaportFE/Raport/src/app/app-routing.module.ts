import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainRaportViewComponent } from './components/main-raport-view/main-raport-view.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';

const routes: Routes = [
  { path: '', redirectTo: '/raport/1/10', pathMatch: 'full' , canActivate: [authGuard] }, // Przekierowanie pustej ścieżki do raportu
  { path: 'raport/:page/:range', component: MainRaportViewComponent },
  { path: '**', component: PageNotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
