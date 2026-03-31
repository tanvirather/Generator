import { Component, input, model, output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'nc-text',
  imports: [FormsModule],
  templateUrl: './text.html',
  styleUrl: './text.css'
})
export class Text {
  label = input<string>('');
  value = model<string>('');
  required = input<boolean>(false);
  change = output<string>();
}
