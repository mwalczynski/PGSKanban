import { trigger, state, style, transition, animate } from '@angular/core';

export const slideInOutAnimation = trigger('slideInOutAnimation', [
    transition(':enter', [
        style({ transform: 'translate(-300px, 0)', 'margin-right': -300 }),
        animate('0.3s')
    ]),
    transition(':leave', [
        style({ transform: 'translate(0, 0)', 'margin-right': 0 }),
        animate('0.1s', style({ transform: 'translate(-300px, 0)', 'margin-right': -300 }))
    ])
]);
