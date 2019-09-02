import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { URI, Comments } from '../discussion/model';

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html'
})

export class CommentsComponent {
  public comments: Comment[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {

    http.get<Comment[]>(baseUrl + URI.comments).subscribe(result => {
      this.comments = result;
    }, error => console.error(error));
  }
}
