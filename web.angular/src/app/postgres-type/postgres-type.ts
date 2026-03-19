import { Component, model, OnInit, signal } from '@angular/core';
import { Components } from '../../components';
import { PostgresType as PostgresTypeModel } from '../../models';
import { PostgresTypeStore } from '../../store';
import {JsonPipe} from '@angular/common';

@Component({
  selector: 'app-postgres-type',
  imports: [...Components, JsonPipe],
  templateUrl: './postgres-type.html',
  styleUrl: './postgres-type.css',
  standalone: true,
})
export class PostgresType implements OnInit {
  selectedType = model<string>('varchar');
  store = new PostgresTypeStore();
  postgresTypes = signal<PostgresTypeModel[]>([]);

  columns = [
    { label: 'ID', key: 'id' },
    { label: 'Name', key: 'name' },
  ];

  typeOptions = [
    { label: 'VARCHAR', value: 'varchar' },
    { label: 'INTEGER', value: 'integer' },
    { label: 'BOOLEAN', value: 'boolean' },
    { label: 'DATE', value: 'date' },
    { label: 'TIMESTAMP', value: 'timestamp' },
  ];

  async ngOnInit() {
    this.postgresTypes.set(await this.store.getAll());
  }
}
