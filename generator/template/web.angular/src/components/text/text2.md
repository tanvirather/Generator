# 📘 `nc-text` Angular Component

`nc-text` is a **standalone** Angular input component built using **Angular 17+ signal-based APIs**. It provides a flexible text input with support for labels, hints, validation messages, icons, and reactive two-way binding.

***

## 🚀 Features

- ✅ **Signal-based Inputs/Outputs** (Angular 17+ syntax)
- 🔁 **Two-way data binding** using `model()`
- 💡 **Customizable label, hint, and error message**
- 🔒 **Disabled state support**
- 🧹 **Clearable input with optional clear icon**
- 🖼️ **Prefix icon support** (`search`, `user`, `mail`, or custom)
- ⚡ **Events for focus, blur, and value changes**
- 📏 **Optional maxLength constraint**

***

## 🧩 Usage

### 1. Import the Component

Since `nc-text` is standalone, you can directly import it into your component:

```ts
import { Component } from '@angular/core';
import { Text } from './text'; // Adjust path as necessary

@Component({
  selector: 'app-example',
  standalone: true,
  imports: [Text],
  template: `
    <nc-text
      [label]="'Username'"
      [placeholder]="'Enter your name'"
      [required]="true"
      [clearable]="true"
      [prefixIcon]="'user'"
      [(value)]="username"
      (focused)="onFocus()"
      (blurred)="onBlur()"
      (valueChange)="onValueChange($event)"
    ></nc-text>
    <p>Current value: {{ username }}</p>
  `
})
export class ExampleComponent {
  username = '';

  onFocus() {
    console.log('Input focused');
  }

  onBlur() {
    console.log('Input blurred');
  }

  onValueChange(value: string) {
    console.log('Value changed:', value);
  }
}
```


***

## ⚙️ API Reference

### Inputs

| Name | Type | Default | Description |
| :-- | :-- | :-- | :-- |
| `label` | `string` | `'Label'` | Text label for the input field. |
| `placeholder` | `string` | `''` | Placeholder text inside the input. |
| `hint` | `string` | `''` | Displayed helper text below the input. |
| `errorMessage` | `string` | `''` | Shown when validation fails. |
| `disabled` | `boolean` | `false` | Disables user interaction. |
| `clearable` | `boolean` | `true` | Shows a clear button when input has value. |
| `prefixIcon` | `string` | `''` | Optional icon name before input (`'search'`, `'user'`, `'mail'`, etc.). |
| `maxLength` | `number \| null` | `null` | Limits number of characters allowed. |
| `inputId` | `string` | Auto-generated | Unique identifier for the input element. |
| `required` | `boolean` | `false` | Marks input as required. |


***

### Model (Two-way Binding)

| Name | Type | Default | Description |
| :-- | :-- | :-- | :-- |
| `value` | `string` | `''` | The input’s bound value; can be two-way bound via `[(value)]`. |


***

### Outputs

| Name | Type | Description |
| :-- | :-- | :-- |
| `valueChange` | `EventEmitter<string>` | Emits when the user types or clears input. |
| `focused` | `EventEmitter<void>` | Emits when the input gains focus. |
| `blurred` | `EventEmitter<void>` | Emits when the input loses focus. |


***

## 🎨 Template and Styling

- **Template:** `text.html`
- **Styles:** `text.css`

You can customize layout, colors, and spacing via the CSS file. For consistency, align styles with your design system or component library.

***

## 🧠 Internal Logic

- Uses **Angular’s `signal`, `input`, `model`, `output`, and `computed` APIs** for reactive data flow.
- Tracks internal states:
    - `isFocused`: when input is active
    - `hasValue`: dynamically computed based on `value()`

Example logic snippet:

```ts
hasValue = computed(() => !!this.value() && this.value().length > 0);
```


***

## 💡 Example Use Case

Use `nc-text` in forms, search bars, or user input fields where clearable and dynamic behavior is desired.

***



# HTML
```html
<div class="field-wrapper" [class.focused]="isFocused()" [class.has-value]="hasValue()"
  [class.has-error]="errorMessage()">

  <div class="input-track">
    <span class="icon-slot" *ngIf="prefixIcon()">
      <ng-container [ngSwitch]="prefixIcon()">
        <svg *ngSwitchCase="'search'" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none"
          stroke="currentColor" stroke-width="2">
          <circle cx="11" cy="11" r="8" />
          <path d="m21 21-4.35-4.35" />
        </svg>
        <svg *ngSwitchCase="'user'" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none"
          stroke="currentColor" stroke-width="2">
          <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2" />
          <circle cx="12" cy="7" r="4" />
        </svg>
        <svg *ngSwitchCase="'mail'" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none"
          stroke="currentColor" stroke-width="2">
          <rect width="20" height="16" x="2" y="4" rx="2" />
          <path d="m22 7-8.97 5.7a1.94 1.94 0 0 1-2.06 0L2 7" />
        </svg>
        <svg *ngSwitchDefault xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor"
          stroke-width="2">
          <circle cx="12" cy="12" r="10" />
        </svg>
      </ng-container>
    </span>

    <div class="input-container">
      <label class="floating-label" [attr.for]="inputId()">{{ label() }}</label>
      <input [id]="inputId()" type="text" class="text-input" [(ngModel)]="value"
        [placeholder]="isFocused() ? placeholder() : ''" [disabled]="disabled()" [maxlength]="maxLength() ?? 9999"
        (focus)="onFocus()" (blur)="onBlur()" (input)="onInput($event)" autocomplete="off" />
    </div>

    <button *ngIf="clearable() && hasValue()" class="clear-btn" type="button" (click)="clearValue()"
      aria-label="Clear input">
      <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
        <path d="M18 6 6 18M6 6l12 12" />
      </svg>
    </button>
  </div>

  <div class="input-border">
    <span class="border-fill"></span>
  </div>
  <div class="meta-row">
    <span class="error-text" *ngIf="errorMessage(); else hintTpl">
      <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
        <circle cx="12" cy="12" r="10" />
        <line x1="12" y1="8" x2="12" y2="12" />
        <line x1="12" y1="16" x2="12.01" y2="16" />
      </svg>
      {{ errorMessage() }}
    </span>
    <ng-template #hintTpl>
      <span class="hint-text">{{ hint() }}</span>
    </ng-template>
    <span class="char-count" *ngIf="maxLength()">
      {{ value().length }}/{{ maxLength() }}
    </span>
  </div>
</div>

```

# TS
```ts
import { CommonModule } from '@angular/common';
import {
  Component,
  computed,
  input,
  model,
  output,
  signal
} from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'nc-text',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './text.html',
  styleUrl: './text.css'
})
export class Text {
  // Inputs (Angular 17+ signal-based)
  label = input<string>('Label');
  placeholder = input<string>('');
  hint = input<string>('');
  errorMessage = input<string>('');
  disabled = input<boolean>(false);
  clearable = input<boolean>(true);
  prefixIcon = input<string>('');  // 'search' | 'user' | 'mail' | ''
  maxLength = input<number | null>(null);
  inputId = input<string>('text-input-' + Math.random().toString(36).slice(2, 7));
  required = input<boolean>(false);

  // Two-way binding via model()
  value = model<string>('');

  // Outputs
  valueChange = output<string>();
  focused = output<void>();
  blurred = output<void>();

  // Internal state
  isFocused = signal(false);
  hasValue = computed(() => !!this.value() && this.value().length > 0);

  onFocus() {
    this.isFocused.set(true);
    this.focused.emit();
  }

  onBlur() {
    this.isFocused.set(false);
    this.blurred.emit();
  }

  onInput(event: Event) {
    const val = (event.target as HTMLInputElement).value;
    this.value.set(val);
    this.valueChange.emit(val);
  }

  clearValue() {
    this.value.set('');
    this.valueChange.emit('');
  }
}

```

# CSS
```css
:host {
  display: block;
  font-family: 'DM Sans', 'Segoe UI', sans-serif;
}

.field-wrapper {
  --clr-bg: #f8f7f4;
  --clr-border: #d4cfc8;
  --clr-border-focus: #1a1a1a;
  --clr-label: #9a9490;
  --clr-label-active: #1a1a1a;
  --clr-text: #1a1a1a;
  --clr-hint: #b5b0aa;
  --clr-error: #c0392b;
  --clr-icon: #b5b0aa;
  --clr-icon-focus: #1a1a1a;
  --transition: 0.22s cubic-bezier(0.4, 0, 0.2, 1);
  position: relative;
}

.input-track {
  display: flex;
  align-items: center;
  background: var(--clr-bg);
  border-radius: 10px;
  padding: 0 14px;
  gap: 10px;
  min-height: 58px;
}

.icon-slot {
  display: flex;
  align-items: center;
  flex-shrink: 0;
  color: var(--clr-icon);
  transition: color var(--transition);
}

.icon-slot svg {
  width: 18px;
  height: 18px;
}

.focused .icon-slot {
  color: var(--clr-icon-focus);
}

.input-container {
  flex: 1;
  position: relative;
  padding-top: 18px;
  padding-bottom: 6px;
}

.floating-label {
  position: absolute;
  top: 50%;
  left: 0;
  transform: translateY(-50%);
  font-size: 15px;
  color: var(--clr-label);
  pointer-events: none;
  transition:
    top var(--transition),
    font-size var(--transition),
    color var(--transition),
    transform var(--transition);
  white-space: nowrap;
  line-height: 1;
}

.focused .floating-label,
.has-value .floating-label {
  top: 10px;
  transform: none;
  font-size: 11px;
  letter-spacing: 0.04em;
  text-transform: uppercase;
  color: var(--clr-label-active);
  font-weight: 600;
}

.focused .floating-label {
  color: #1a1a1a;
}

.has-error .floating-label {
  color: var(--clr-error) !important;
}

.text-input {
  width: 100%;
  border: none;
  background: transparent;
  outline: none;
  font-size: 15px;
  color: var(--clr-text);
  font-family: inherit;
  padding: 0;
  line-height: 1.4;
  caret-color: #1a1a1a;
}

.text-input::placeholder {
  color: var(--clr-hint);
}

.text-input:disabled {
  opacity: 0.45;
  cursor: not-allowed;
}

.clear-btn {
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  background: #e8e4de;
  border: none;
  border-radius: 50%;
  width: 22px;
  height: 22px;
  cursor: pointer;
  padding: 0;
  color: #6b6560;
  transition: background var(--transition), transform 0.1s;
}

.clear-btn svg {
  width: 12px;
  height: 12px;
}

.clear-btn:hover {
  background: #ddd8d1;
  transform: scale(1.1);
}

/* Animated bottom border */
.input-border {
  position: relative;
  height: 2px;
  background: var(--clr-border);
  border-radius: 0 0 10px 10px;
  overflow: hidden;
}

.border-fill {
  position: absolute;
  inset: 0;
  background: var(--clr-border-focus);
  transform: scaleX(0);
  transform-origin: left;
  transition: transform 0.28s cubic-bezier(0.4, 0, 0.2, 1);
}

.focused .border-fill {
  transform: scaleX(1);
}

.has-error .border-fill {
  background: var(--clr-error);
  transform: scaleX(1);
}

.meta-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 5px 2px 0;
  min-height: 20px;
}

.hint-text {
  font-size: 12px;
  color: var(--clr-hint);
}

.error-text {
  display: flex;
  align-items: center;
  gap: 4px;
  font-size: 12px;
  color: var(--clr-error);
  font-weight: 500;
}

.error-text svg {
  width: 13px;
  height: 13px;
  flex-shrink: 0;
}

.char-count {
  font-size: 12px;
  color: var(--clr-hint);
  font-variant-numeric: tabular-nums;
  margin-left: auto;
}

```

