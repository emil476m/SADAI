import {Component, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {ActivatedRoute} from "@angular/router";
import {IonContent} from "@ionic/angular";
import {FormControl, Validators} from "@angular/forms";
import {Message} from "../models/Message.model";

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

        <ion-card id="textCard" *ngFor="let message of messages"
                  [ngClass]="{'left-card': !message.isUser, 'right-card': message.isUser}">
          <ion-tab-bar [ngStyle]="{ 'background-color': message.isUser ? '#001087' : '#3A3B3C' }">
            <ion-title style="color: White">{{ message.message }}</ion-title>
          </ion-tab-bar>
        </ion-card>
      </ion-content>
    </ion-content>
    <ion-item>
      <ion-input placeholder="  text...  " [formControl]="message" id="messageInput"></ion-input>
      <ion-button (click)="sendMessage()" id="button" slot="end">
        <ion-icon name="send-outline"></ion-icon>
        <p>&#160; send message</p>
      </ion-button>
    </ion-item>
  `,
  styleUrls: ['AiChat.page.scss'],
})
export class AiChatPage implements OnInit {
  constructor(private http: HttpClient) {
  }

  botName : string = "";

  message: FormControl<string | null> = new FormControl("", [Validators.required, Validators.minLength(3), Validators.maxLength(50)]);

  messages: Message[] = [];

  ngOnInit() {
    this.botName = "Gemini";


    let text1: Message = {
      message: "Hi I am " + this.botName + "\n I am a AI chat bot",
      isUser: false,
    }
    let text2: Message = {
      message: "How may I help you?",
      isUser: false,
    }


    this.messages = [
      text1,
      text2
    ];

    this.getConnection();

  }

  async sendMessage() {
    if (this.message.value != null) {
      let text: Message = {
        message: this.message.value,
        isUser: true,
      }

      this.messages.push(text)

      //TODO - Send Message
    }

  }


  async getConnection() {
    //ToDo esablish socket
  }

}
