import { Component, OnInit } from '@angular/core';
import { Observable, take } from 'rxjs';
import { Member } from 'src/app/models/member';
import { Pagination } from 'src/app/models/pagination';
import { User } from 'src/app/models/user';
import { PaginationParams } from 'src/app/models/paginationParams';
import { MembersService } from 'src/app/services/members.service';

@Component({
  selector: 'app-members-page',
  templateUrl: './members-page.component.html',
  styleUrls: ['./members-page.component.css']
})
export class MembersPageComponent implements OnInit { 
  members: Member[] = [];
  paginationParams: PaginationParams | undefined;
  pagination: Pagination | undefined;

  constructor(private membersService: MembersService) {
    this.paginationParams = this.membersService.getPaginationParams();
  }

  ngOnInit(): void {
    this.loadMembers();
  }

  loadMembers() {
    if(this.paginationParams) {
      this.membersService.setPaginationParams(this.paginationParams);

      this.membersService.getMembers(this.paginationParams).subscribe({
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
    if(this.paginationParams && this.paginationParams?.pageIndex !== event.page) {
      this.paginationParams.pageIndex = event.page;
      this.membersService.setPaginationParams(this.paginationParams);
      this.loadMembers(); 
    }
  }
}
