import { TemplateRef } from "@angular/core";
import { ColTemplate } from "./col-template";


describe('ColTemplate', () => {
  it('should create an instance', () => {
    const mockTemplateRef = {} as TemplateRef<any>;
    const directive = new ColTemplate(mockTemplateRef);

    expect(directive).toBeTruthy();
    expect(directive.tpl).toBe(mockTemplateRef);
  });
});
