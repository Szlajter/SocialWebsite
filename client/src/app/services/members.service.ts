import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { Member } from '../models/member';
import { map, of, take } from 'rxjs';
import { PaginatedResult } from '../models/pagination';
import { UserParams } from '../models/userParams';
import { AccountService } from './account.service';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;
  members: Member[] = [];
  memberCache = new Map;
  user: User | undefined;
  userParams: UserParams | undefined;
  
  constructor(private http: HttpClient, private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({  
      next: user => {
        if(user){
          //I'll probably expand userParams contructor so it will take user
          this.userParams = new UserParams();
          this.user = user;
        }
      }
    })
  }

  getUsersParams() {
    return this.userParams;
  }

  setUserParams(params: UserParams) {
    this.userParams = params;
  }

  getMembers(userParams: UserParams) {
    const response = this.memberCache.get(Object.values(userParams).join('-'));
    if (response)
      return of(response);

    let params = this.getPaginationHeaders(userParams.pageIndex, userParams.pageSize);

    return this.getPaginatedResult<Member[]>(this.baseUrl + 'users', params).pipe(
      map(response => {
        this.memberCache.set(Object.values(userParams).join('-'), response);
        return response;  
      }) 
    )
  }

  getMember(username: string) {
    //change it later so users won't be repeated in map (use set maybe?)
    const member = [...this.memberCache.values()]
      .reduce((arr, elem) => arr.concat(elem.result), [])
      .find((member: Member) => member.userName === username);

    if(member)
      return of(member);    
    return this.http.get<Member>(this.baseUrl + 'users/' + username);
  }

  updateMember(member: Member) {
    //pipe to update loaded  members array aswell
    return this.http.put(this.baseUrl + 'users', member).pipe(
      map(() => {
        const index = this.members.indexOf(member);
        this.members[index] = {...this.members[index], ...member}
      })
    )
  }

  deletePhoto(photoId: number) {
    return this.http.delete(this.baseUrl + 'users/delete-photo/' + photoId);
  }

  addFollow(username: string) {
    return this.http.post(this.baseUrl + 'follows/follow/' + username, {});
  }

  getFollowers(pageNumber: number, pageSize: number) {
    let params = this.getPaginationHeaders(pageNumber, pageSize);

    return this.getPaginatedResult<Member[]>(this.baseUrl + 'follows/followers', params);
  }

  getFollowing(pageNumber: number, pageSize: number) {
    let params = this.getPaginationHeaders(pageNumber, pageSize);

    return this.getPaginatedResult<Member[]>(this.baseUrl + 'follows/following', params);
  }

  deleteFollow(username: string) {
    return this.http.delete(this.baseUrl + 'follows/unfollow/' + username, {});
  }

  private getPaginatedResult<T>(url: string, params: HttpParams) {
    const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>;
    
    return this.http.get<T>(url, { observe: 'response', params }).pipe(
      map(response => {
        if (response.body) {
          paginatedResult.result = response.body;
        }
        const pagination = response.headers.get('Pagination');
        if (pagination) {
          paginatedResult.pagination = JSON.parse(pagination);  
        }
        return paginatedResult;
      })
    );
  }
  
  private getPaginationHeaders(pageIndex: number, pageSize: number) {
    let params = new HttpParams();

    params = params.append("pageIndex", pageIndex);
    params = params.append("pageSize", pageSize);
    return params;
  }

}
