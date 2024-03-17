import { Component, Input, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload'
import { take } from 'rxjs';
import { Member } from 'src/app/models/member';
import { Photo } from 'src/app/models/photo';
import { User } from 'src/app/models/user';
import { AccountService } from 'src/app/services/account.service';
import { BusyService } from 'src/app/services/busy.service';
import { environment } from 'src/environments/environment.development';



@Component({
  selector: 'app-avatar-uploader',
  templateUrl: './avatar-uploader.component.html',
  styleUrls: ['./avatar-uploader.component.css']
})
export class AvatarUploaderComponent implements OnInit {
  @Input() member: Member | undefined
  uploader!: FileUploader;
  hasBaseDropZoneOver = false;
  baseUrl = environment.apiUrl;
  user: User | undefined;

  //had to use busyService because this request just doesn't go throught interceptors
  constructor(private accountService: AccountService, private busyService: BusyService) { 
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => {
        if(user) this.user = user;
      }
    });
  }

  ngOnInit(): void {
    this.initializeUploader();
  }

  fileOverBase(e: any) {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'users/update-profile-picture',
      authToken: 'Bearer ' + this.user?.token,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: true,
      maxFileSize: 10 * 1024 * 1024
    });

    //works but looks ugly, maybe send GET request instead
    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response && this.member && this.user) {
        const photo: Photo = JSON.parse(response);
        this.member.photoUrl = photo.url;
        
        this.user.photoUrl = photo.url;
        this.accountService.setCurrentUser(this.user);
        this.busyService.idle();
      }
    }

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
      this.busyService.busy();
    };
  }
}
