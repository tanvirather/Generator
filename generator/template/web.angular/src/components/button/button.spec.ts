import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Button } from './button';

describe('Button', () => {
  let component: Button;
  let fixture: ComponentFixture<Button>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Button],
    }).compileComponents();

    fixture = TestBed.createComponent(Button);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have default label "Save"', () => {
    expect(component.label).toBe('Save');
  });

  it('should have default type "primary"', () => {
    expect(component.type).toBe('primary');
  });

  it('should have default disabled state as false', () => {
    expect(component.disabled).toBe(false);
  });

  it('should accept custom label input', () => {
    component.label = 'Submit';
    expect(component.label).toBe('Submit');
  });

  it('should accept custom type input', () => {
    component.type = 'secondary';
    expect(component.type).toBe('secondary');
  });

  it('should accept disabled input', () => {
    component.disabled = true;
    expect(component.disabled).toBe(true);
  });

  it('should accept id input', () => {
    component.id = 'test-button';
    expect(component.id).toBe('test-button');
  });

  // it('should emit onclick event', () => {
  //   spyOn(component.onclick, 'emit');
  //   component.onclick.emit();
  //   expect(component.onclick.emit).toHaveBeenCalled();
  // });
});
