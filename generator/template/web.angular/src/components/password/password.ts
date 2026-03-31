import { Component, input, model, output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'nc-password',
  imports: [FormsModule],
  templateUrl: './password.html',
  styleUrl: './password.css',
})
export class Password {
  label = input<string>('');
  value = model<string>('');
  required = input<boolean>(false);
  change = output<string>();
}
