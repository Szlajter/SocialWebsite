import { Component, OnInit } from '@angular/core';
import { Observable, take } from 'rxjs';
import { Member } from 'src/app/models/member';
import { Pagination } from 'src/app/models/pagination';
import { User } from 'src/app/models/user';
import { UserParams } from 'src/app/models/userParams';
import { MembersService } from 'src/app/services/members.service';

@Component({
  selector: 'app-members-page',
  templateUrl: './members-page.component.html',
  styleUrls: ['./members-page.component.css']
})
export class MembersPageComponent implements OnInit { 
  //members$: Observable<Member[]> | undefined;
  members: Member[] = [];
  user: User | undefined;
  userParams: UserParams | undefined;
  pagination: Pagination | undefined;

  constructor(private membersService: MembersService) {
    this.userParams = this.membersService.getUsersParams();
  }

  ngOnInit(): void {
    this.loadMembers();
  }

  loadMembers() {
    if(this.userParams) {
      this.membersService.setUserParams(this.userParams);

      this.membersService.getMembers(this.userParams).subscribe({
        next: response => {
          if(response.result && response.pagination) {
            this.members = response.result;
            this.pagination = response.pagination;
          }
        }
      })
    }
  }

  pageChanged(event: any) {
    if(this.userParams && this.userParams?.pageIndex !== event.page) {
      this.userParams.pageIndex = event.page;
      this.membersService.setUserParams(this.userParams);
      this.loadMembers(); 
    }
  }
}
