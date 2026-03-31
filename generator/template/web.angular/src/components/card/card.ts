import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Button } from '../button/button';

@Component({
  selector: 'nc-card',
  imports: [CommonModule, Button],
  templateUrl: './card.html',
  styleUrl: './card.css',
})
export class Card {
  @Input() header: string = "";
  @Input() reset: boolean = false;
  @Input() help: boolean = false;
  @Input() history: boolean = false;
  @Input() submit: string = "Save";
  @Output() resetClick = new EventEmitter();
  @Output() helpClick = new EventEmitter();
  @Output() historyClick = new EventEmitter();
  @Output() submitClick = new EventEmitter();
  cardId = () => "card-" + this.header.replace(" ", "-").toLowerCase();
  showFooter() {
    return this.submit.trim() !== "";
  }
}
