import { Directive, EventEmitter, HostListener, Input, Output } from '@angular/core';

@Directive({
    selector: '[escKeyPressed]'
})
export class EscKeyDirective {
    @Output('escKeyPressed') appEscKey = new EventEmitter();
    @HostListener('keydown.escape')
    handleKeyboardEvent() {
        this.appEscKey.emit();
    }
}
