import { Component, Input } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Member } from 'src/app/models/member';
import { MembersService } from 'src/app/services/members.service';
import { StatusService } from 'src/app/services/status.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent {
  @Input() member: Member | undefined;

  constructor(private memberService: MembersService, private toastr: ToastrService,
      public statusService: StatusService) {}

  addFollow(member: Member) {
    console.log(member);
    this.memberService.addFollow(member.userName).subscribe({
      next: () => this.toastr.success('You have followed ' + member.nickName)
    })
  }

  deleteFollow(member: Member) {
    this.memberService.deleteFollow(member.userName).subscribe({
      next: () => this.toastr.success('You have unfollowed ' + member.nickName)
    })
  }
}
