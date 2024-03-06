import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AiChatPage } from './AiChat.page';

const routes: Routes = [
  {
    path: '',
    component: AiChatPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class FolderPageRoutingModule {}
