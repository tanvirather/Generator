package models

import "strings"

// Column represents a row in a column CSV file.
type Column struct {
	Label     string `csv:"label"`
	Control   string `csv:"control"`
	Column    string `csv:"column"`
	Datatype  string `csv:"datatype"`
	Required  string `csv:"required"`
	Default   string `csv:"default"`
	Length    string `csv:"length"`
	Precision string `csv:"precision"`
	Unique    string `csv:"unique"`
	FkSchema  string `csv:"fk_schema"`
	FkTable   string `csv:"fk_table"`
	FkColumn  string `csv:"fk_column"`
}

func (c Column) ColumnLower() string {
	return strings.ToLower(c.Column)
}
func (c Column) FkSchemaLower() string {
	return strings.ToLower(c.FkSchema)
}
func (c Column) FkTableLower() string {
	return strings.ToLower(c.FkTable)
}
func (c Column) FkColumnLower() string {
	return strings.ToLower(c.FkColumn)
}
func (c Column) ColumnCsharp() string {
	return strings.ReplaceAll(c.Column, "_", "")
}
