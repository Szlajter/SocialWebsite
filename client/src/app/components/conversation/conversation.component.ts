import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { take } from 'rxjs';
import { Message } from 'src/app/models/message';
import { User } from 'src/app/models/user';
import { AccountService } from 'src/app/services/account.service';
import { MessagesService } from 'src/app/services/messages.service';

@Component({
  selector: 'app-conversation',
  templateUrl: './conversation.component.html',
  styleUrls: ['./conversation.component.css']
})
export class ConversationComponent {
  @ViewChild('messageForm') messageForm?: NgForm;
  @Input() chattingWithUsername?: string;
  user?: User;
  messageContent = '';

  constructor(public messageService: MessagesService, private accountService: AccountService) { 
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => {
        if (user) this.user = user;
      }
    })
  }

  ngOnChanges(): void {
    this.loadMessages();
  }

  ngOnDestroy(): void {
      this.messageService.stopHubConnection();
  }

  loadMessages() {  
      this.messageService.stopHubConnection();

    if (this.chattingWithUsername && this.user) {
      this.messageService.createHubConnection(this.user, this.chattingWithUsername)
    }
  }

  sendMessage() {
    if(!this.chattingWithUsername) return;
    this.messageService.sendMessage(this.chattingWithUsername, this.messageContent).subscribe({
      next: message => {
          //this.messages.push(message);
          //this.messageForm?.reset();
      }
    })
  }

  deleteMessage(id: number) {
    this.messageService.DeleteMessage(id).subscribe({
     // next: () => this.messages.splice(this.messages.findIndex(m => m.id === id), 1)
    })
  }
}
