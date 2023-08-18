import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Message } from 'src/app/models/message';
import { MessagesService } from 'src/app/services/messages.service';

@Component({
  selector: 'app-conversation',
  templateUrl: './conversation.component.html',
  styleUrls: ['./conversation.component.css']
})
export class ConversationComponent {
  @ViewChild('messageForm') messageForm?: NgForm;
  @Input() username?: string;
  @Input() chattingWithUsername?: string;
  messages: Message[] = []; 
  messageContent = '';

  constructor(private messageService: MessagesService) { }
  
  ngOnChanges(): void {
    this.loadMessages();
  }

  loadMessages() {  
    if (this.chattingWithUsername) {
      this.messageService.getConversation(this.chattingWithUsername).subscribe({
        next: messages => this.messages = messages
      })
    }
  }

  sendMessage() {
    if(!this.chattingWithUsername) return;
    this.messageService.sendMessage(this.chattingWithUsername, this.messageContent).subscribe({
      next: message => {
          this.messages.push(message);
          this.messageForm?.reset();
      }
    })
  }
}
