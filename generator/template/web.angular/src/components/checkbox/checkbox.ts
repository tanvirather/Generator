import { Component, input, model, output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'nc-checkbox',
  imports: [FormsModule],
  templateUrl: './checkbox.html',
  styleUrl: './checkbox.css',
})
export class Checkbox {
  label = input<string>('');
  value = model<boolean>(false);
  required = input<boolean>(false);
  change = output<boolean>();
}
