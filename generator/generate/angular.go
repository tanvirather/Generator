package generate

import (
	// "fmt"
	// "os"
	// "path/filepath"
	// "strings"

	"zuhid.com/generator/models"
	// "zuhid.com/generator/tools"
)

// Angular represents a Angular code generator.
type Angular struct {
	Company    string
	Product    string
	InputPath  string
	OutputPath string
}

// Generate generates Angular code based on the provided table data.
func (angular *Angular) Generate(tables []models.Table){
	template := Template{
		Company:    angular.Company,
		Product:    angular.Product,
		OutputPath: angular.OutputPath,
		Folders:    []string{"web.angular"},
	}
	template.Generate()

	// for _, table := range tables {
	// 	filePath := fmt.Sprintf("%s/%s/%s.csv", angular.InputPath, table.Schema, table.Table)
	// 	csv := tools.Csv{}
	// 	columns, _ := csv.ReadColumns(filePath)
	// }
}
