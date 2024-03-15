import {Component, OnInit} from '@angular/core';
import {WebSocketService} from "./WebsocketService";
import {FormControl, Validators} from "@angular/forms";
@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.scss'],
})
export class AppComponent implements OnInit{
  Tlanguage = new FormControl("", [Validators.required]);
  Flanguage  = new FormControl("", [Validators.required]);
  constructor(protected ws: WebSocketService) {
  }


  ngOnInit() {
    setTimeout(() => {
      this.ClientWantsToGetLangueges()

    }, 2000)
  }

  ClientWantsToGetLangueges()
  {
    const obj =
      {
        eventType: "ClientWantsToGetLangueges",
        isUser: true,
      }
    this.ws.socket.send(JSON.stringify(obj))
  }

  select() {
    this.ws.toLanguage = this.Tlanguage.value!
    this.ws.fromLanguage = this.Flanguage.value!
  }
}
