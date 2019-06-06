import { Component } from '@angular/core';
import { SampleService } from '../sockets/sample.service';
import { ACTIVATE_SOCKETS } from '../shared/constants';

@Component({
    templateUrl: './main.component.html',
    styleUrls: ['./main.component.scss']
})
export class MainComponent {
    constructor(private sampleService: SampleService) {
        if (ACTIVATE_SOCKETS) {
            sampleService.init();
        }
    }
}
