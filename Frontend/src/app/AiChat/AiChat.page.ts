import {Component, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {ActivatedRoute} from "@angular/router";
import {IonContent} from "@ionic/angular";
import {FormControl, Validators} from "@angular/forms";
import {Message} from "../models/Message.model";
import {BaseDto, ClientWantsToTextServeDto} from "../../assets/BaseDto";
import {WebSocketService} from "../WebsocketService";

@Component({
  selector: 'app-AiChat',
  template: `
    <ion-header [translucent]="true">
      <ion-toolbar>
        <ion-title>
          {{botName}}
        </ion-title>
      </ion-toolbar>
    </ion-header>

    <ion-content [fullscreen]="true">
      <ion-header collapse="condense">
        <ion-toolbar>
          <ion-title size="large">Blank</ion-title>
        </ion-toolbar>
      </ion-header>

      <ion-content #textWindow id="Textcontainer" [scrollEvents]="true">

        <ion-card [ngStyle]="{ 'background-color': message.isUser ? '#001087' : '#3A3B3C' }" id="textCard" *ngFor="let message of this.ws.messages"
                  [ngClass]="{'left-card': !message.isUser, 'right-card': message.isUser}">

            <ion-card-content style="color: White">{{ message.message }}</ion-card-content>

        </ion-card>
      </ion-content>
    </ion-content>
    <ion-item>
      <ion-input placeholder="  text...  " [formControl]="message" id="messageInput"></ion-input>
      <ion-button (click)="chooseMessageType()" id="button" slot="end">
        <ion-icon name="send-outline"></ion-icon>
        <p>&#160; send message</p>
      </ion-button>
    </ion-item>
  `,
  styleUrls: ['AiChat.page.scss'],
})
export class AiChatPage implements OnInit {


  botName : string = "";

  message: FormControl<string | null> = new FormControl("", [Validators.required, Validators.minLength(3), Validators.maxLength(50)]);


  constructor(private http: HttpClient, protected ws: WebSocketService) {

  }

  serverRespondsMessage(dto: ClientWantsToTextServeDto) {
    let text: Message = {
      message: dto.message,
      isUser: dto.isUser,
    }
    this.ws.messages.push(text)
  }


  ngOnInit() {
    this.botName = "Sally";

    let text1: Message = {
      message: "Hi I am " + this.botName + "\n. I am an AI, all translations may not be accurate, so use at your own risk. Remember to hit select, after selecting the languages for the translate feature.",
      isUser: false,
    }


    this.ws.messages = [
      text1
    ];
  }

  async sendMessage() {
    if (this.message.value != null) {
      let text: Message = {
        message: this.message.value,
        isUser: true,
      }

      this.ws.messages.push(text)

      var object = {
        eventType: "ClientWantsToTranslateText",
        text: text.message,
        toLan: this.ws.toLanguage,
        fromLan: this.ws.fromLanguage
      }

      this.ws.socket.send(JSON.stringify(object));
      this.message.reset();
    }
  }

  async sendMessageToAI() {
    if (this.message.value != null) {
      let text: Message = {
        message: this.message.value,
        isUser: true,
      }

      this.ws.messages.push(text)

      var object = {
        eventType: "ClientWantsAIResponse",
        message: text.message,
        isUser: true
      }

      this.ws.socket.send(JSON.stringify(object));
      this.message.reset();
    }
  }


  async chooseMessageType(){
    if (this.ws.openAiToggle)
      this.sendMessageToAI();
    else
      this.sendMessage();
  }

}
