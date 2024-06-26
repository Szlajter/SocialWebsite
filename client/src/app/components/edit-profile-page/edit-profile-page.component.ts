import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { FileUploader } from 'ng2-file-upload';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { Member } from 'src/app/models/member';
import { User } from 'src/app/models/user';
import { AccountService } from 'src/app/services/account.service';
import { MembersService } from 'src/app/services/members.service';

@Component({
  selector: 'app-edit-profile-page',
  templateUrl: './edit-profile-page.component.html',
  styleUrls: ['./edit-profile-page.component.css']
})
export class EditProfilePageComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm | undefined;
  //warns about leaving/refreshing dirty form
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any) {
    if(this.editForm?.dirty) {
      $event.returnValue = true;
    }
  }
  member: Member | undefined;
  user: User | null = null;
  
  constructor(private accountService: AccountService, private memberService: MembersService, private toastr: ToastrService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user)  
  }

  ngOnInit(): void {
    this.loadMember();
  }

  loadMember() {
    if (!this.user) return;
    this.memberService.getMember(this.user.username).subscribe(member => {
      this.member = member;
    })
  }
  
  updateMember() {
    this.memberService.updateMember(this.editForm?.value).subscribe({
      next: () => {
        if(this.member && this.user) {
          this.user.nickname = this.member.nickName;
          this.accountService.setCurrentUser(this.user);
        }
        this.toastr.success("Profile updated");
        this.editForm?.reset(this.member);
      }
    })
  }

  // deletePhoto(photoId: number) {
  //   this.memberService.deletePhoto(photoId).subscribe({
  //     next: () => {
  //       if(this.member) {
  //         this.member.photos = this.member.photos.filter(x => x.id != photoId);
  //       }
  //     }
  //   })
  // }
}
