import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'nc-button',
  imports: [CommonModule],
  templateUrl: './button.html',
  styleUrl: './button.css',
})
export class Button {
  @Input() id!: string; // the id of the button
  @Input() label: string = "Save"; // the text of the button
  @Input() type: 'primary' | 'secondary' = 'primary'; // the type of the button (primary, secondary, etc.)
  @Input() disabled: boolean = false; // whether the button is disabled or not
  @Output() onclick = new EventEmitter(); // the event emitted when the button is clicked
}
