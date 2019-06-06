import {Component, Input} from '@angular/core';

@Component({
  selector: 'app-markdown-info',
  templateUrl: './markdown-info.component.html',
  styleUrls: ['./markdown-info.component.scss']
})
export class MarkdownInfoComponent {
    @Input() isDisplayed: boolean;
}
