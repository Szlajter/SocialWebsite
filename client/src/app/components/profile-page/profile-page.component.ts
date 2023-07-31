import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IntlRelativeTimePipeOptions } from 'angular-ecmascript-intl';
import { Member } from 'src/app/models/member';
import { Pagination } from 'src/app/models/pagination';
import { MembersService } from 'src/app/services/members.service';

@Component({
  selector: 'app-profile-page',
  templateUrl: './profile-page.component.html',
  styleUrls: ['./profile-page.component.css']
})
export class ProfilePageComponent implements OnInit {
  member: Member | undefined;
  followers: Member[] | undefined;
  following: Member[] | undefined;
  pageIndex: number = 1;
  pageSize: number = 5;
  pagination: Pagination | undefined;

  constructor(private memberService: MembersService, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.loadMember();
  }

  loadMember() {
    const username = this.route.snapshot.paramMap.get('username');
    if (!username) return;
    this.memberService.getMember(username).subscribe(member => {
      this.member = member;
    })
  }

  loadFollowers() {
    this.memberService.getFollowers(this.pageIndex, this.pageSize).subscribe({
      next: response => {
        this.followers = response.result;
        this.pagination = response.pagination;
      }
    })
  }

  loadFollowing() {
    this.memberService.getFollowing(this.pageIndex, this.pageSize).subscribe({
      next: response => {
        this.following = response.result;
        this.pagination = response.pagination

      }
    })
  }

  followersPageChanged(event: any) {
    if(this.pageIndex !== event.page) {
      this.pageIndex = event.page;
      this.loadFollowers(); 
    }
  }

  followingPageChanged(event: any) {
    if(this.pageIndex !== event.page) {
      this.pageIndex = event.page;
      this.loadFollowing(); 
    }
  }
}
