import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';
import { Message } from '../models/message';
import { HubConnection, HubConnectionBuilder } from '../../../node_modules/@microsoft/signalr';
import { User } from '../models/user';
import { BehaviorSubject, take } from 'rxjs';
import { Group } from '../models/group';

@Injectable({
  providedIn: 'root'
})
export class MessagesService {
  baseUrl = environment.apiUrl;
  hubUrl = environment.hubUrl;
  private hubConnection?: HubConnection;
  private conversationSource = new BehaviorSubject<Message[]>([]);
  conversation$ = this.conversationSource.asObservable();

  constructor(private http: HttpClient) { }

  createHubConnection(user: User, otherUsername: string) {
  this.hubConnection = new HubConnectionBuilder()
    .withUrl(this.hubUrl + 'message?user=' + otherUsername, {
      accessTokenFactory: () => user.token
    })
    .withAutomaticReconnect()
    .build();

    this.hubConnection.start().catch(error => console.log(error));

    this.hubConnection.on('ReceiveConversation', messages => {
      this.conversationSource.next(messages);
    })

    this.hubConnection.on('NewMessage', message => {
      this.conversation$.pipe(take(1)).subscribe({
        next: messages => {
          this.conversationSource.next([...messages, message])
        }
      })
    })

    this.hubConnection.on('MessageDeleted', message => {
      this.conversation$.pipe(take(1)).subscribe({
        next: messages => {
          const indexToDelete = messages.findIndex(msg => msg.id === message.id);
          
          if (indexToDelete !== -1) {
            messages.splice(indexToDelete, 1);
            this.conversationSource.next([...messages]);
          }
        }
      })
    })

    this.hubConnection.on('UpdatedGroup', (group: Group) => {
      if (group.connections.some(x => x.username === otherUsername)) {
        this.conversation$.pipe(take(1)).subscribe({
          next: messages => {
            messages.forEach(message => {
              if (!message.dateRead) {
                message.dateRead = new Date(Date.now());
              }
            })
            this.conversationSource.next([...messages]);
          }
        })
      }
    })
}

  stopHubConnection() {
    if(this.hubConnection) {
      this.hubConnection?.stop();
    }
  }

  getMessages(pageNumber: number, pageSize: number) {
    let params = getPaginationHeaders(pageNumber, pageSize);
    return getPaginatedResult<Message[]>(this.baseUrl + 'messages', params, this.http);
  }

  getConversation(username: string) {
    return this.http.get<Message[]>(this.baseUrl + 'messages/conversation/' + username);
  }

  getChatList() {
    return this.http.get<Message[]>(this.baseUrl + 'messages/conversations'); 
  }

  async sendMessage(username: string, content: string) {
    return this.hubConnection?.invoke('SendMessage', {recipientUsername: username, content})
      .catch(error => console.log(error));
  }

  DeleteMessage(id: number) {
    return this.hubConnection?.invoke('DeleteMessage', id)
      .catch(error => console.log(error));
  }
}
