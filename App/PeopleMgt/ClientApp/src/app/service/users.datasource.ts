import { DataSource, CollectionViewer } from "@angular/cdk/collections";
import { HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable, of } from "rxjs";
import { PageMetadata } from '../model/PageMetadata';
import { PageParameters } from '../model/PageParameters';
import { User } from "../model/User";
import { UserService } from "./user.service";
import { Output, EventEmitter } from "@angular/core";

export class UsersDataSource implements DataSource<User> {

  private defaultPageSize: number = 5;
  private defaultSortColumnName: string = 'firstname';
  private defaultSortOrder: string = 'asc';

  public userCount: number;

  @Output() serviceError: EventEmitter<any> = new EventEmitter();

  users: User[];
  headers: HttpHeaders;
  pageMetadata: PageMetadata;

  private userChangeTracker: BehaviorSubject<User[]> = new BehaviorSubject<User[]>([]);
  private loadingChangeTracker = new BehaviorSubject<boolean>(false);
  public loading = this.loadingChangeTracker.asObservable();

  constructor(private userService: UserService) {
  }

  private internalFetch(pageParam: PageParameters) {

    this.loadingChangeTracker.next(true);

    this.userService.getUsers(pageParam).subscribe(res => {
      
      // custom paging-Headers in response help in getting paging functionality done on server.
      var pageData = res.headers.get('Page-Headers');
      this.pageMetadata = JSON.parse(pageData);
      var userArray: User[] = res.body;

        //trigger the datasource to be bound on UI table.
      this.userChangeTracker.next(userArray);
      
      this.userCount = userArray.length;

      this.loadingChangeTracker.next(false)
    },
      err => function () {
        this.loading = false;
        this.serviceError.emit(err);
      });
  }

  fetchAll() {

    this.internalFetch(new PageParameters({
      filter: '',
      sortOrder: this.defaultSortOrder,
      sortColumn: this.defaultSortColumnName,
      pageNumber: 1,
      pageSize: this.defaultPageSize
    }));

  }

  fetchSpecificPages(pageParam: PageParameters) {

    this.internalFetch(pageParam);
  }


  connect(collectionViewer: CollectionViewer): Observable<User[]> {

    return this.userChangeTracker.asObservable();
  }

  disconnect(collectionViewer: CollectionViewer): void {

    this.userChangeTracker.complete();
    this.loadingChangeTracker.complete();
  }

}

