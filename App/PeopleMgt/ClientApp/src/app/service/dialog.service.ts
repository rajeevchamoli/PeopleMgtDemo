import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material';
import { ConfirmationboxComponent } from '../confirmationbox/confirmationbox.component'

@Injectable({
  providedIn: 'root'
})
export class DialogService {

  constructor(private msgBox: MatDialog) { }

  launchConfirmationBox(msg, title:string = '') {

    return this.msgBox.open(ConfirmationboxComponent, {
      width: '350px',
      panelClass: 'confirmation-box-container',
      disableClose: true,
      position: { top: "5px" },
      data: {
        message: msg,
        title: title
      }
    });
  }



}
