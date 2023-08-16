import { Component, Input, OnInit } from '@angular/core';
import { Message } from 'src/app/models/message';
import { MessagesService } from 'src/app/services/messages.service';

@Component({
  selector: 'app-conversation',
  templateUrl: './conversation.component.html',
  styleUrls: ['./conversation.component.css']
})
export class ConversationComponent {
  @Input() username?: string;
  @Input() chattingWithUsername?: string;
  messages: Message[] = []; 

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
}
