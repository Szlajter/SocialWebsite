import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';
import { Message } from '../models/message';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { User } from '../models/user';
import { BehaviorSubject } from 'rxjs';

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

  sendMessage(username: string, content: string) {
    return this.http.post<Message>(this.baseUrl + 'messages', {recipientUsername: username, content});
  }

  DeleteMessage(id: number) {
    return this.http.delete(this.baseUrl + 'messages/' + id);
  }
}
