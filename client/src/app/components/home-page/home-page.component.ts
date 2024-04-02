import { Component, OnInit } from '@angular/core';
import { Pagination } from 'src/app/models/pagination';
import { PaginationParams } from 'src/app/models/paginationParams';
import { Post } from 'src/app/models/post';
import { MembersService } from 'src/app/services/members.service';
import { PostsService } from 'src/app/services/posts.service';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.css']
})
export class HomePageComponent implements OnInit {
  posts: Post[] = [];
  paginationParams: PaginationParams | undefined;
  pagination: Pagination | undefined;
  
  constructor(private postsService: PostsService) {
    this.paginationParams = this.postsService.getPaginationParams();
  }

  ngOnInit(): void {
    this.loadPosts();
  }

  loadPosts() {
    if(this.paginationParams) {
      this.postsService.setPaginationParams(this.paginationParams);

      this.postsService.getPosts(this.paginationParams).subscribe({
        next: response => {
          if(response.result && response.pagination) {
            this.posts = response.result;
            this.pagination = response.pagination;
          }
        }
      })
    }
  }

  pageChanged(event: any) {
    if(this.paginationParams && this.paginationParams?.pageIndex !== event.page) {
      this.paginationParams.pageIndex = event.page;
      this.postsService.setPaginationParams(this.paginationParams);
      this.loadPosts(); 
    }
  }
}
