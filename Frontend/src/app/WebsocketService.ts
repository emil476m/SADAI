import {Message} from "./models/Message.model";
import {Router} from "@angular/router";
import {ToastController} from "@ionic/angular";
import {
  BaseDto,
  ServerRespondsToUser,
  ServerReturnsListOfLanguageNames,
  ServerSendsErrorMessageToClient
} from "../assets/BaseDto";
import {Injectable} from "@angular/core";

@Injectable({providedIn: 'root'})
export class WebSocketService {
  messages: Array<Message> = [];
  languages : Array<string> = [];
  toLanguage: string = "";
  fromLanguage: string = "";
  socket: WebSocket = new WebSocket("ws://localhost:8181");

  constructor(public router: Router, public toast: ToastController) {
    this.socket.onmessage = message => {
      const messageFromServer = JSON.parse(message.data) as BaseDto<any>;
      // @ts-ignore
      this[messageFromServer.eventType].call(this, messageFromServer);
    }
  }

  ServerRespondsToUser(dto: ServerRespondsToUser)
  {
    this.messages.push(dto as Message);
  }

  ServerReturnsListOfLanguageNames(dto: ServerReturnsListOfLanguageNames)
  {
    var l = dto.names as Array<string>;
    this.languages.push(...l);
  }

  async ServerSendsErrorMessageToClient(dto: ServerSendsErrorMessageToClient)
  {
    var t = await this.toast.create(
      {
        color: "warning",
        duration: 2000,
        message: dto.recivedMessage
      }
    )
    t.present();
  }
}
