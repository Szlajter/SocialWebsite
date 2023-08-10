import { Component, Input } from '@angular/core';
import { Message } from 'src/app/models/message';

@Component({
  selector: 'app-message-card',
  templateUrl: './message-card.component.html',
  styleUrls: ['./message-card.component.css']
})
export class MessageCardComponent {
  @Input() message: Message | undefined;
}
