import { Component, Input, OnInit } from '@angular/core';
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

   constructor(private postService: PostsService) {}


  likePost() {
    this.postService.likePost(this.post!.id).subscribe({
      next: () => {
        if (this.post?.hasLiked) {
          this.post.hasLiked = false;
          this.post.likedByCount--;
        } else if (this.post?.hasDisliked) {
          this.post.hasDisliked = false;
          this.post.dislikedByCount--;
          this.post.likedByCount++;
          this.post.hasLiked = true;
        } else {
          this.post!.hasLiked = true;
          this.post!.likedByCount++;
        }
      }
    });
  }

  dislikePost() {
    this.postService.dislikePost(this.post!.id).subscribe({
      next: () => {
        if (this.post?.hasDisliked) {
          this.post.hasDisliked = false;
          this.post.dislikedByCount--;
        } else if (this.post?.hasLiked) {
          this.post.hasLiked = false;
          this.post.likedByCount--;
          this.post.dislikedByCount++;
          this.post.hasDisliked = true;
        }
        else {
          this.post!.hasDisliked = true;
          this.post!.dislikedByCount++;
        }
      }
    });
  }
}
