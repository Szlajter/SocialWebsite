import { Component, OnInit, ViewChild } from '@angular/core';
import { Message } from 'src/app/models/message';
import { Pagination } from 'src/app/models/pagination';
import { UserParams } from 'src/app/models/userParams';
import { MessagesService } from 'src/app/services/messages.service';
import { ConversationComponent } from '../conversation/conversation.component';

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
  pagination: Pagination | undefined;
  pageNumber = 1;
  pageSize = 5;

  constructor(private messageService: MessagesService) {    
  }

  ngOnInit(): void {
      this.loadChats();
      this.getUsername();
  }

  pageChanged(event: any) {
    if(this.pageNumber != event.page) {
      this.pageNumber = event.page;
    }
  }

  loadChats() {
    this.messageService.getChatList().subscribe(response =>{
      this.chats = response;      
    })
  }

  getUsername() {
    this.username = JSON.parse(localStorage.getItem('user')!).username;
  }

  //todo: make it less ugly
  setChattingWithUsername(message: Message) {
      this.chattingWithUsername = (message.senderUsername == this.username) ? message.recipientUsername : message.senderUsername;
  }
}
