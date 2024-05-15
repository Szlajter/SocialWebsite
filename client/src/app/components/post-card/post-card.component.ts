import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Post } from 'src/app/models/post';
import { AccountService } from 'src/app/services/account.service';
import { PostsService } from 'src/app/services/posts.service';

@Component({
  selector: 'app-post-card',
  templateUrl: './post-card.component.html',
  styleUrls: ['./post-card.component.css']
})
export class PostCardComponent  {
  @Input() post: Post | undefined;

   constructor(private postService: PostsService, private toastr: ToastrService) {}


  likePost() {
    if (!this.post) return;

    const postId = this.post.id;
  
    this.postService.likePost(postId).subscribe({
      next: () => {
        if (this.post!.hasDisliked) {
          this.post!.dislikedByCount--;
        }
  
        this.post!.hasDisliked = false;
  
        if (this.post!.hasLiked) {
          this.post!.hasLiked = false;
          this.post!.likedByCount--;
        } else {
          this.post!.hasLiked = true;
          this.post!.likedByCount++;
        }
      }
    });
  }

  dislikePost() {
    if (!this.post) return;

    const postId = this.post.id;
  
    this.postService.dislikePost(postId).subscribe({
      next: () => {        
        if (this.post!.hasLiked) {
          this.post!.likedByCount--;
        }
  
        this.post!.hasLiked = false;
  
        if (this.post!.hasDisliked) {
          this.post!.hasDisliked = false;
          this.post!.dislikedByCount--;
        } else {
          this.post!.hasDisliked = true;
          this.post!.dislikedByCount++;
        }
      }
    });
  }

  deletePost() {
    if (!this.post) return;

    const postId = this.post.id;

    this.postService.deletePost(postId).subscribe({
      next: () => {
        this.toastr.success("Post successfully deleted!")
      }
    });
  }
}
