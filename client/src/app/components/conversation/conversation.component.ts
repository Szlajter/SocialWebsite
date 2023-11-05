import { ChangeDetectionStrategy, Component, Input, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { take } from 'rxjs';
import { User } from 'src/app/models/user';
import { AccountService } from 'src/app/services/account.service';
import { MessagesService } from 'src/app/services/messages.service';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
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
    this.messageService.sendMessage(this.chattingWithUsername, this.messageContent).then(() => {
      this.messageForm?.reset();
    })
  }

  deleteMessage(id: number) {
    this.messageService.DeleteMessage(id);
  }
}
