import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Member } from 'src/app/models/member';
import { Pagination } from 'src/app/models/pagination';
import { MembersService } from 'src/app/services/members.service';

@Component({
  selector: 'app-members-page',
  templateUrl: './members-page.component.html',
  styleUrls: ['./members-page.component.css']
})
export class MembersPageComponent implements OnInit { 
  //members$: Observable<Member[]> | undefined;
  members: Member[] = [];
  pagination: Pagination | undefined;
  pageIndex = 1;
  pageSize = 5;

  constructor(private membersService: MembersService) {}

  ngOnInit(): void {
    //this.members$ = this.membersService.getMembers();
    this.loadMembers();
  }

  loadMembers() {
    this.membersService.getMembers(this.pageIndex, this.pageSize).subscribe({
      next: response => {
        if(response.result && response.pagination) {
          this.members = response.result;
          this.pagination = response.pagination;
        }
      }
    })
  }

  pageChanged(event: any) {
    if(this.pageIndex !== event.page) {
      this.pageIndex = event.page;
      this.loadMembers();
    }
  }
}
