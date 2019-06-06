import { Injectable } from '@angular/core';
import { HubConnection } from '@aspnet/signalr-client';
import { HubUrlService } from './hub-url.service';

const SampleHubActions = {
    SayHello: 'sayHello'
};
@Injectable()
export class SampleService {
    hubConnection: HubConnection;

    constructor(private urlService: HubUrlService) {
    }

    init() {
        this.hubConnection = new HubConnection(this.urlService.createUrl('sample'));
        this.hubConnection.on(SampleHubActions.SayHello, (message: string) => {
            console.log(message);
        });
        this.hubConnection.start();
    }
}
