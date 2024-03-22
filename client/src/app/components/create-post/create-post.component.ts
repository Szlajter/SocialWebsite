import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-create-post',
  templateUrl: './create-post.component.html',
  styleUrls: ['./create-post.component.css']
})
export class CreatePostComponent implements OnInit {
  username: string | undefined;
  profilePictureUrl: string | undefined;
  
  constructor() {

  }

  ngOnInit(): void {
    this.username = JSON.parse(localStorage.getItem('user')!).username;
    this.profilePictureUrl = JSON.parse(localStorage.getItem('user')!).photoUrl;
  }

  
}
