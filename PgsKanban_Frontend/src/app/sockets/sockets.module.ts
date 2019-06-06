import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SampleService } from './sample.service';
import { HubUrlService } from './hub-url.service';
import { CardDetailsSocketsService } from './card-details.sockets.service';

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [],
    providers: [
        SampleService,
        CardDetailsSocketsService,
        HubUrlService
    ]
})
export class SocketsModule { }
