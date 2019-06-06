import { Injectable } from '@angular/core';

@Injectable()
export class TooltipService {

    shouldHideTooltip(inputType: string, length: number)
    {
        return inputType.length <= length
    }
}
