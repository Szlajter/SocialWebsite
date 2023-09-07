import { Component, OnInit, ViewChild } from '@angular/core';
import { Message } from 'src/app/models/message';
import { Pagination } from 'src/app/models/pagination';
import { MessagesService } from 'src/app/services/messages.service';
import { ConversationComponent } from '../conversation/conversation.component';
import { ActivatedRoute } from '@angular/router';
import { StatusService } from 'src/app/services/status.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit{
  @ViewChild(ConversationComponent) currentConversation: ConversationComponent | undefined;
  chats: Message[] = [];
  username: string | undefined; 
  chattingWithUsername: string | undefined;

  //pagination variables
  pagination: Pagination | undefined;
  pageNumber = 1;
  pageSize = 5;

  constructor(private messageService: MessagesService, private route: ActivatedRoute,
      public statusService: StatusService) {    
  }

  ngOnInit(): void {
      this.username = JSON.parse(localStorage.getItem('user')!).username;
      this.loadChats();
  }

  pageChanged(event: any) {
    if(this.pageNumber != event.page) {
      this.pageNumber = event.page;
    }
  }
  //when trying to send a message to a new person, that person is not visible on the left side.
  loadChats() {
    this.messageService.getChatList().subscribe(response =>{
      this.chats = response;

      this.route.queryParams.subscribe(params => {
        this.chattingWithUsername = params['user'];
      });
      
      if(this.chattingWithUsername === undefined)
        this.setChattingWithUsername(this.chats[0]);
    })
  }

  setChattingWithUsername(message: Message) {
      this.chattingWithUsername = (message.senderUsername == this.username) ? message.recipientUsername : message.senderUsername;
  }
}
