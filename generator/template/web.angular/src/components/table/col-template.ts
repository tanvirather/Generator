import { Directive, Input, TemplateRef } from '@angular/core';

/**
 * Directive to define a custom cell (and optionally header) template for a column.
 *
 * Usage:
 *   <ng-template columnDef="status" let-value let-row="row">
 *     <span class="badge">{{ value }}</span>
 *   </ng-template>
 *
 * The implicit context variable (`let-value`) receives the raw cell value.
 * The `row` context variable receives the full row object.
 * An optional `headerTpl` sibling can override the header cell.
 */
@Directive({
  selector: '[colTemplate]',
  standalone: true,
})
export class ColTemplate {
  /** Matches the `key` of a TableColumn */
  @Input('colTemplate') key!: string;

  /** Optional: override just the header for this column */
  @Input() headerTpl?: TemplateRef<any>;

  constructor(public readonly tpl: TemplateRef<{ $implicit: any; row: any }>) { }
}
