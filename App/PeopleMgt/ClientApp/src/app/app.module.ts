import { BrowserModule } from '@angular/platform-browser';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { MatToolbarModule, MatProgressSpinnerModule, MatCheckboxModule, MatTableModule, MatSortModule, MatPaginatorModule, MatFormFieldModule, MatInputModule, MatIconModule, MatBadgeModule, MatButtonModule, MatSnackBarModule, MatDialogModule, MatGridListModule  } from '@angular/material';
import { FormsModule,ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { EditableTableComponent } from './editable-table/editable-table.component';
import { UserService } from "./service/user.service";
import { NotificationService } from './service/notification.service';
import { DialogService } from './service/dialog.service';
import { ConfirmationboxComponent } from './confirmationbox/confirmationbox.component';
import { HeaderComponent } from './header/header.component';
import { CreateuserComponent } from './createuser/createuser.component';

@NgModule({
  declarations: [
    AppComponent,
    EditableTableComponent,
    ConfirmationboxComponent,
    HeaderComponent,
    CreateuserComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    MatToolbarModule,
    MatProgressSpinnerModule,
    MatCheckboxModule,
    MatTableModule,
    MatSortModule,
    MatPaginatorModule,
    MatFormFieldModule,
    MatInputModule,
    MatBadgeModule,
    MatButtonModule,
    MatIconModule,
    MatSnackBarModule,
    MatDialogModule,
    MatGridListModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [UserService, NotificationService, DialogService],
  bootstrap: [AppComponent],
  entryComponents: [EditableTableComponent, ConfirmationboxComponent, HeaderComponent, CreateuserComponent]
})
export class AppModule { }
