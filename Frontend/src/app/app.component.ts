import {Component, OnInit} from '@angular/core';
import {WebSocketService} from "./WebsocketService";
import {FormControl, Validators} from "@angular/forms";
import {ToastController} from "@ionic/angular";
@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.scss'],
})
export class AppComponent implements OnInit{
  Tlanguage = new FormControl("", [Validators.required]);
  Flanguage  = new FormControl("", [Validators.required]);
  constructor(protected ws: WebSocketService, private toast : ToastController) {
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

  async select() {
    this.ws.toLanguage = this.Tlanguage.value!
    this.ws.fromLanguage = this.Flanguage.value!

    var t = await this.toast.create(
      {
        color: "success",
        duration: 1000,
        message: "Languages successfully selected"
      }
    )
    t.present();
  }
}
