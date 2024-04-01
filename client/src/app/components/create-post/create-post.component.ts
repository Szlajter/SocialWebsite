import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { PostsService } from 'src/app/services/posts.service';

@Component({
  selector: 'app-create-post',
  templateUrl: './create-post.component.html',
  styleUrls: ['./create-post.component.css']
})
export class CreatePostComponent implements OnInit {
  username: string | undefined;
  profilePictureUrl: string | undefined;
  model: any = { };
  
  constructor(public postsService: PostsService, private toastr: ToastrService) {}

  ngOnInit(): void {
    this.username = JSON.parse(localStorage.getItem('user')!).username;
    this.profilePictureUrl = JSON.parse(localStorage.getItem('user')!).photoUrl;
  }

  createPost() {
    this.postsService.createPost(this.model).subscribe({
      next: () => this.toastr.success("Succesfully created a new post!")
    });
  }
}
