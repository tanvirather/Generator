package main

import (
	"zuhid.com/generator/generate"
	"zuhid.com/generator/tools"
)

func main() {
	csv := tools.Csv{}
	_ = generate.Postgres{}
	company := "Zuhid"
	product := "Product"
	inputPath := "input/" + company
	outputPath := "../"
	entries, _ := csv.ReadTables(inputPath + "/Table.csv")

	// 	postgres := generate.Postgres{InputPath: "input", OutputPath: "../db.postgres"}
	// 	postgres.Generate(entries)

	csharp := generate.Csharp{Company: company, Product: product, InputPath: inputPath, OutputPath: outputPath}
	csharp.Generate(entries)

	// vue := generate.Vue{Company: "Zuhid", Product: "Product", InputPath: "input", OutputPath: "../"}
	// vue.Generate(entries)

	// angular := generate.Angular{Company: "Zuhid", Product: "Product", InputPath: "input", OutputPath: "../"}
	// angular.Generate(entries)
}
