import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { Post } from '../models/post';

@Injectable({
  providedIn: 'root'
})
export class PostsService {
  baseUrl = environment.apiUrl;
  
  constructor(private http: HttpClient) { }

  createPost(model: any) {
    return this.http.post<Post>(this.baseUrl + 'posts', model).subscribe();
  }
}
