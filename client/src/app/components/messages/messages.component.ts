import { Component, OnInit } from '@angular/core';
import { Message } from 'src/app/models/message';
import { Pagination } from 'src/app/models/pagination';
import { MessagesService } from 'src/app/services/messages.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit{
  chats: Message[] | undefined
  messages: Message[] | undefined;
  pagination: Pagination | undefined;
  pageNumber = 1;
  pageSize = 5;

  constructor(private messageService: MessagesService) {    
  }

  ngOnInit(): void {
      this.loadChats();
  }

  loadMessages() {
    this.messageService.getMessages(this.pageNumber, this.pageSize).subscribe({
      next: response => {
        this.messages = response.result;
        this.pagination = response.pagination;
      }
    })
  }

  pageChanged(event: any) {
    if(this.pageNumber != event.page) {
      this.pageNumber = event.page;
      this.loadMessages();
    }
  }

  loadChats() {
    this.messageService.getChatList().subscribe(response =>{
      this.chats = response;
    })
  }
}
