import { Injectable } from '@angular/core';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  constructor(private snackBar: MatSnackBar) { }

  config: MatSnackBarConfig = {
    duration: 3000,
    horizontalPosition: 'center',
    verticalPosition: 'top'
  }

  notify(msg, responseType: Response = Response.Info) {

    if (responseType == Response.Info) {
      this.config['panelClass'] = ['notification', 'info'];
    }
    else if (responseType == Response.Warning) {
      this.config['panelClass'] = ['notification', 'warn'];
    }
    else {
      this.config['panelClass'] = ['notification', 'error'];
    }

    this.snackBar.open(msg, '', this.config);
  }
  
}

export enum Response {
  Info,
  Warning,
  Error
}
