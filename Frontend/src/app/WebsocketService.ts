import {Message} from "./models/Message.model";
import {Router} from "@angular/router";
import {ToastController} from "@ionic/angular";
import {BaseDto, ServerRespondsToUser} from "../assets/BaseDto";
import {Injectable} from "@angular/core";

@Injectable({providedIn: 'root'})
export class WebSocketService {
  messages: Array<Message> = [];
  socket: WebSocket = new WebSocket("ws://localhost:8181");

  constructor(public router: Router, public toast: ToastController) {
    this.socket.onmessage = message => {
      const messageFromServer = JSON.parse(message.data) as BaseDto<any>;
      // @ts-ignore
      this[messageFromServer.eventType].call(this, messageFromServer);
    }
  }

  ServerRespondsToUser(dot: ServerRespondsToUser)
  {
    this.messages.push(dot as Message);
  }
}
