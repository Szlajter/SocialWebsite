import { Component, Input, OnInit } from '@angular/core';
import { Message } from 'src/app/models/message';
import { MessagesService } from 'src/app/services/messages.service';

@Component({
  selector: 'app-conversation',
  templateUrl: './conversation.component.html',
  styleUrls: ['./conversation.component.css']
})
export class ConversationComponent implements OnInit {
  @Input() username?: string;
  messages: Message[] = []; 

  constructor(private messageService: MessagesService) { }

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages() {
    if (this.username) {
      this.messageService.getConversation(this.username).subscribe({
        next: messages => this.messages = messages
      })
    }
  }
}
