import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';

import {ApplicationPaths} from '../shared/app.constants';
import {ChatComponent} from './chat.component';

const routes: Routes = [
  {path: ApplicationPaths.Chat, component: ChatComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ChatRoutingModule {
}
