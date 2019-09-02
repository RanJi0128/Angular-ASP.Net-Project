import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { URI, User } from '../discussion/model';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html'
})

export class UsersComponent {
  public users: User[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {

    http.get<User[]>(baseUrl + URI.users).subscribe(result => {
      this.users = result;
    }, error => console.error(error));
  }
}
