import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { environment } from '../../environments/environment';
import { PageParameters } from '../model/PageParameters';
import { User } from '../model/User';


@Injectable({
  providedIn: 'root'
})
export class UserService {

  baseUrl: string = environment.ApiBaseUrl;
  userApiPath: string = environment.UserApiPath;
  requestContentType: string = 'application/json';

  constructor(private http: HttpClient) { }

  // from group directive
  userForm: FormGroup = new FormGroup({
    id: new FormControl(),
    email: new FormControl('', [Validators.required, Validators.email, Validators.maxLength(50)]),
    firstname: new FormControl('', [Validators.required, Validators.maxLength(25)]),
    lastname: new FormControl('', [Validators.required, Validators.maxLength(25)]),
    age: new FormControl(0, [Validators.min(1), Validators.max(120)]),
    address: new FormControl('', Validators.maxLength(100)),
    interests: new FormControl('', Validators.maxLength(100)),
    picture: new FormControl(null, Validators.maxLength(200000)) // 200KB ref: http://extraconversion.com/data-storage/kilobytes/kilobytes-to-characters.html
  });

  getUsers(nav: PageParameters) {

    return this.http.get<User[]>(this.baseUrl + this.userApiPath, {
      observe: 'response',
      params: new HttpParams()
        .set('filter', nav.filter)
        .set('sortOrder', nav.sortOrder)
        .set('sortColumn', nav.sortColumn)
        .set('pageNumber', nav.pageNumber.toString())
        .set('pageSize', nav.pageSize.toString())
    });
  }

  insertUser(user: User) {

    const headers = new HttpHeaders().set('content-type', this.requestContentType);

    var body = {
      email: user.email,
      firstname: user.firstname,
      lastname: user.lastname,
      age: user.age,
      address: user.address,
      interests: user.interests,
      picture: user.picture
    };

    return this.http.post<User>(this.baseUrl + this.userApiPath, body, { headers });

  }

  updateUser(user: User) {

    const headers = new HttpHeaders().set('content-type', this.requestContentType);

    return this.http.put<User>(this.baseUrl + this.userApiPath + '/' + user.id, user, { headers });

  }

  getUserById(id: number) {

    return this.http.get<User>(this.baseUrl + this.userApiPath + '/' + id);
  }

  deleteUser(id: number) {

    return this.http.delete<User>(this.baseUrl + this.userApiPath + '/' + id);
  }

}
