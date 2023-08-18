import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';
import { Message } from '../models/message';

@Injectable({
  providedIn: 'root'
})
export class MessagesService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

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
}
