import { Component, input, model } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'nc-select',
  imports: [FormsModule],
  templateUrl: './select.html',
  styleUrl: './select.css',
})
export class Select {
  label = input<string>('');
  value = model<string | number>('');
  options = input<{ label: string; value: string | number }[]>([]);
}
