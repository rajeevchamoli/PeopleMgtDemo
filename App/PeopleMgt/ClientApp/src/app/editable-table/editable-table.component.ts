import {  AfterViewInit, Component, OnInit, ViewChild, ElementRef, ChangeDetectorRef } from '@angular/core';
import { MatPaginator, MatSort, MatDialog, MatDialogConfig} from "@angular/material";
import { fromEvent, merge } from 'rxjs';
import { debounceTime, distinctUntilChanged, tap } from 'rxjs/operators';
import { SelectionModel } from '@angular/cdk/collections';
import { PageMetadata } from '../model/PageMetadata';
import { PageParameters } from '../model/PageParameters';
import { User } from '../model/User';
import { UserService } from '../service/user.service';
import { UsersDataSource } from '../service/users.datasource';
import { NotificationService, Response } from '../service/notification.service';
import { DialogService } from '../service/dialog.service';
import { CreateuserComponent } from '../createuser/createuser.component'


const initialSelection = [];
const allowMultiSelect = false;

export enum RowOperation {
  New,
  Update,
  None
}

@Component({
  selector: 'app-editable-table',
  templateUrl: './editable-table.component.html',
  styleUrls: ['./editable-table.component.css']
})
export class EditableTableComponent implements OnInit, AfterViewInit {

  displayedColumns: string[] = ['firstname', 'lastname', 'email', 'address', 'interests', 'age', 'picture', 'actionsColumn'];
  dataSource: UsersDataSource;
  selection = new SelectionModel<User>(allowMultiSelect, initialSelection);
  pageMetadata: PageMetadata;
 

  currentRowBeingEdited: any = { Operation: RowOperation.None, Row: null };

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild('input') input: ElementRef;

  //initialise Users WebApi service before DOM in ngOnInit
  constructor(private _userService: UserService, private notificationService: NotificationService, private dialogService: DialogService, private dialog: MatDialog, private changeDetectorRefs: ChangeDetectorRef,) { }

  ngOnInit() {

    this.dataSource = new UsersDataSource(this._userService);

    this.dataSource.serviceError.subscribe(err => function () {
      this.notificationService.notify("Unexpected Service Error, refreshing the page.", Response.Error);
      this.paginator.pageIndex = 0;
      this.dataSource.fetchAll(); 
    });
    // on loading of of view get the data to be bound .
    this.dataSource.fetchAll();
  }

  ngAfterViewInit() {

    this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);

    fromEvent(this.input.nativeElement, 'keyup')
      .pipe(
        debounceTime(250),
        distinctUntilChanged(),
        tap(() => {
          this.paginator.pageIndex = 0;
          this.dataSource.fetchSpecificPages(this.getCurrentPageParam());          
        })
      ).subscribe();

    merge(this.sort.sortChange, this.paginator.page)
      .pipe(
      tap(() => function () {
          this.dataSource.fetchSpecificPages(this.getCurrentPageParam())
      })).subscribe();
  }

  private getCurrentPageParam(): PageParameters {

    return new PageParameters({
      filter: this.input.nativeElement.value,
      sortOrder: this.sort.direction ? this.sort.direction : 'asc',
      sortColumn: this.sort.active ? this.sort.active : 'firstname',
      // paginator starts from 0th index
      pageNumber: this.paginator.pageIndex + 1, 
      pageSize: this.paginator.pageSize
    });

  }

  pageEvent(event) {
    //it will run everytime you change paginator
    this.dataSource.fetchSpecificPages(this.getCurrentPageParam());
  }


  //since the editing has started change icon from "Edit" to "Check" and "Cancel"
  startEdit(event, row) {

    // cancel any current row being edited/new.
    this.cancelCurrentOperation();
    // set the new row in edit model and mark it as currentrow
    this.markCurrentRowWithOperation(RowOperation.Update, row);
    this.takeSnapshot(row);
    row.editing = true;
  }


  delete(event, row) {

    this.dialogService.launchConfirmationBox("Are you sure ?", "Delete Record")
      .afterClosed().subscribe(res => {
        if (res === 'true') {

          this._userService.deleteUser(row.id).subscribe(
            data => {

              this.dataSource.fetchSpecificPages(this.getCurrentPageParam());          
              this.notificationService.notify("Deleted user records");

            },
            err => function () {

              this.notificationService.notify("Unable to delete the user record, Hence refreshing the page", Response.Error);
              this._userService.FetchAll()
            });
        }
      });
  }


  cancel(event, row) {

    //since the editing has finished change icon from "Check" and "Cancel" to "Edit"
    row.editing = false;
    this.reversetoSnapshot(row);
    this.markCurrentRowWithOperation(RowOperation.None, null);

  }

  cancelCurrentOperation() {

    if (this.currentRowBeingEdited.Operation === RowOperation.None) {
      return;
    }

     if (this.currentRowBeingEdited.Operation === RowOperation.Update) {
      // remove the last row from the ata being pushed during operation
      this.cancel(null, this.currentRowBeingEdited.Row);
    }

    this.markCurrentRowWithOperation(RowOperation.None, null);
  }


  saveEdit(event, row) {

    // overwrite the data with new from the user inputs
    if (this.isEditSameAsOldValues(row)) {
      this.notificationService.notify('Values have not changed ', Response.Warning);
      return;
    }

    if (this._userService.userForm.valid) {

      let currentRecord = this._userService.userForm.value;
      
      currentRecord.picture = row.picture

      // create new resource
      if (!row.id) {

        this._userService.insertUser(currentRecord).subscribe(
          res => {
            this.notificationService.notify('User record added successfully');
            // update the properties back after reciving te update (except id , rest are not need but doing incase)
            row.id = res.id;
            //since the editing has finished change icon from "Check" and "Cancel" to "Edit"
            row.editing = false;
            this.clearLastEditedDirtyState(row);
            this.markCurrentRowWithOperation(RowOperation.None, null);
            
          },
          err => {
            this.notificationService.notify('User record creation failed : ' + err.statusText + this.formatErrors(err.error));
          }
        );
      }
      else { // update existing resource.

        // Id is not bound to form group.
        currentRecord.id = row.id;
        this._userService.updateUser(currentRecord).subscribe(
          res => {
            this.notificationService.notify('User record updated successfully');
            //since the editing has finished change icon from "Check" and "Cancel" to "Edit"
            row.editing = false;
            this.clearLastEditedDirtyState(row);
            this.markCurrentRowWithOperation(RowOperation.None, null);
            
          },
          err => {
            this.notificationService.notify('User record updation failed : ' + err.statusText + this.formatErrors(err.error));
          }
        );
      }
    }
  }

  /// Launches the modal dialog to create new user.
  createNew() {

    this.cancelCurrentOperation();

    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = "60%";
    dialogConfig.height = "60%";

    this.dialog.open(CreateuserComponent, dialogConfig).afterClosed().subscribe(
      res => { this.dataSource.fetchSpecificPages(this.getCurrentPageParam()); }
    );
  }


  clearSearch() {

    this.input.nativeElement.value = '';
    this.paginator.pageIndex = 0;
    this.dataSource.fetchSpecificPages(this.getCurrentPageParam());
  }


  /// Formats the list of server reside errors
  formatErrors(errors: any) {

    if ((!errors))
      return '';
    else if (Array.isArray(errors))
      return " -> [  " + errors.join(", ") + " ]";
    else
      return " -> " + errors;
  }


  /// is edited values different from actual ones in edited row.
  isEditSameAsOldValues(obj1) {

    return (obj1.firstname === obj1.editedfirstname &&
      obj1.email === obj1.editedemail &&
      obj1.lastname === obj1.editedlastname &&
      obj1.address === obj1.editedaddress &&
      obj1.interests === obj1.editedinterests &&
      obj1.age === obj1.editedage &&
      obj1.picture === obj1.editedpicture);
  }

  /// clear the backup as row changes has been commited.
  clearLastEditedDirtyState(row) {

    // Clear the temporary variables
    row.editedfirstname = "";
    row.editedlastname = "";
    row.editedemail = "";
    row.editedaddress = "";
    row.editedinterests = "";
    row.editedage = "";
    row.editedpicture = "";
  }

  /// create the snapshot of current values of row being edited.
  takeSnapshot(row) {

    row.editedfirstname = row.firstname;
    row.editedemail = row.email;
    row.editedlastname = row.lastname;
    row.editedaddress = row.address;
    row.editedinterests = row.interests;
    row.editedage = row.age;
    row.editedpicture = row.picture;
    row.fakeFilePath = 'c:\\fakeFilePath.jpg';
    
  }

  /// clear the edited values and reverse to old snapshot values.
  reversetoSnapshot(row) {
    row.firstname = row.editedfirstname;
    row.email = row.editedemail;
    row.lastname = row.editedlastname;
    row.address = row.editedaddress;
    row.interests = row.editedinterests;
    row.age = row.editedage;
    row.picture = row.editedpicture;

    this.clearLastEditedDirtyState(row);
  }

  /// handles the uploading of picture.
  acceptImageFile(imageInput: any) {

    const file: File = imageInput.files[0];
    const reader = new FileReader();

    reader.addEventListener('load', (event: any) => {

      if (this.currentRowBeingEdited.Row) {

        this.currentRowBeingEdited.Row.picture = event.target.result;
      }
      else {

        this.notificationService.notify('View has stale data , hence refreshing.'),
        // refresh the view due to state records
        this.paginator.pageIndex = 0;
        this.dataSource.fetchAll();
      }
    });
    reader.readAsDataURL(file);
  }

  /// mark the current row for operation being done. 
  markCurrentRowWithOperation(op: RowOperation, row: any) {

    this.currentRowBeingEdited.Operation = op;
    this.currentRowBeingEdited.Row = row;
  }


  
}
