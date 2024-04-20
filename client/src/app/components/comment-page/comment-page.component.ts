import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Post } from 'src/app/models/post';
import { PostsService } from 'src/app/services/posts.service';

@Component({
  selector: 'app-comment-page',
  templateUrl: './comment-page.component.html',
  styleUrls: ['./comment-page.component.css']
})
export class CommentPageComponent implements OnInit {
  post: Post | undefined;

  constructor(private postsService: PostsService, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.route.params.subscribe(() => {
      this.loadPost();
    })
  }

  // todo: pagination
  loadPost() {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) return;
    
    this.postsService.getPost(id).subscribe({
      next: post => {
          this.post = post;
      }
    })
  }
}
