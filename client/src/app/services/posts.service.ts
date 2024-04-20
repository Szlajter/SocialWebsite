import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { Post } from '../models/post';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';
import { PaginationParams } from '../models/paginationParams';

@Injectable({
  providedIn: 'root'
})
export class PostsService {
  baseUrl = environment.apiUrl;
  paginationParams: PaginationParams | undefined;
  
  constructor(private http: HttpClient) {
    this.paginationParams = new PaginationParams();
  }

  getPaginationParams() {
    return this.paginationParams;
  }

  setPaginationParams(paginationParams: PaginationParams) {
    this.paginationParams = paginationParams;
  }

  createPost(model: any) {
    return this.http.post(this.baseUrl + 'posts', model);
  }

  getPost(id: string) {
    return this.http.get<Post>(this.baseUrl + 'posts/' + id);
  }

  getPosts(paginationParams: PaginationParams) {
    let params = getPaginationHeaders(paginationParams.pageIndex, paginationParams.pageSize);

    return getPaginatedResult<Post[]>(this.baseUrl + 'posts', params, this.http);
  }

  likePost(id: number) {
    return this.http.post<number>(this.baseUrl + 'posts/' + id + '/like', {});
  }

  dislikePost(id: number) {
    return this.http.post<number>(this.baseUrl +  'posts/' + id + '/dislike', {});
  }
}
