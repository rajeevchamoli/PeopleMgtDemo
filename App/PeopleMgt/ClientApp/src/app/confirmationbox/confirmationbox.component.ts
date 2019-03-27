import { Component, OnInit, Inject} from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

@Component({
  selector: 'app-confirmationbox',
  templateUrl: './confirmationbox.component.html',
  styleUrls: ['./confirmationbox.component.css']
})
export class ConfirmationboxComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data,
    public diagRef: MatDialogRef<ConfirmationboxComponent>) { }

  ngOnInit() {
  }

  close() {
    this.diagRef.close(false);
  }

}
