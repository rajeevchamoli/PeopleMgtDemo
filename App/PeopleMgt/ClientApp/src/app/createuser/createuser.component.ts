import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material';
import { UserService } from '../service/user.service';
import { NotificationService } from '../service/notification.service';

@Component({
  selector: 'app-createuser',
  templateUrl: './createuser.component.html',
  styleUrls: ['./createuser.component.css']
})
export class CreateuserComponent implements OnInit {

  private currentPicture: string = "";

  constructor(private userService: UserService,
    private notificationService: NotificationService,
    public dialogRef: MatDialogRef<CreateuserComponent>) {

    // create new data row .
    this.userService.userForm.setValue({
      id: -1,
      firstname: '',
      lastname: '',
      email: '',
      age: '',
      address: '',
      interests: '',
      picture: ''
    });
  }

  ngOnInit() {
  }

  acceptImageFile(imageInput: any) {

    const file: File = imageInput.files[0];
    const reader = new FileReader();

    reader.addEventListener('load', (event: any) => {
      this.currentPicture = event.target.result;

    });
    reader.readAsDataURL(file);
  }

  onClear() {

    this.userService.userForm.reset();
  }

  onSubmit() {

    if (this.userService.userForm.valid) {

      let currentRecord = this.userService.userForm.value;
      currentRecord.id = 0;
      currentRecord.picture = this.currentPicture;
      
      this.userService.insertUser(currentRecord).subscribe(
          res => {
            this.notificationService.notify('User record added successfully');
            this.onClose();
          },
          err => {
            this.notificationService.notify('User record creation failed : ' + err.statusText + this.formatErrors(err.error));
          }
        );
    }
  }

  /// formats the server side errors.
  formatErrors(errors: any) {

    if ((!errors))
      return '';
    else if (Array.isArray(errors))
      return " -> [  " + errors.join(", ") + " ]";
    else
      return " -> " + errors;
  }


  onClose() {

    this.userService.userForm.reset();
    this.dialogRef.close();
  }
}
