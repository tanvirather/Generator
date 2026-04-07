namespace Zuhid.Generator.Models;

public record ColumnModel(
    string Label,
    string Control,
    string Column,
    string Datatype,
    bool Required,
    string? Default,
    int? Length,
    int? Precision,
    int? Unique,
    string FkSchema,
    string FkTable,
    string FkColumn
);
