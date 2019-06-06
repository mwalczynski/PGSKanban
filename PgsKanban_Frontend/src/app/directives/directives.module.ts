import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EscKeyDirective } from './esc-key.directive';
import { AutofocusDirective } from './autofocus.directive';
import { ClickStopPropagationDirective } from './click-stop-propagation.directive';

@NgModule({
    imports: [
        CommonModule
    ],
    declarations: [EscKeyDirective, AutofocusDirective, ClickStopPropagationDirective],
    exports: [EscKeyDirective, AutofocusDirective, ClickStopPropagationDirective]
})
export class DirectivesModule { }
