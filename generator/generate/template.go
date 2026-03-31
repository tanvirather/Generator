package generate

import (
	"log"
	"os"
	"path/filepath"
	"strings"

	"zuhid.com/generator/tools"
)

// Template represents a template-based code generator.
type Template struct {
	Company    string
	Product    string
	OutputPath string
	Folders    []string
}

// Generate generates code based on the templates in the specified folders.
func (t *Template) Generate() {
	for _, template := range t.Folders {
		Path := "template/" + template

		err := filepath.Walk(Path, func(path string, info os.FileInfo, err error) error {
			if err != nil {
				return err
			}

			// Create relative path from template directory
			relPath, err := filepath.Rel("template", path)
			if err != nil {
				return err
			}

			// Replace [Product] in directory/file names
			targetRelPath := strings.ReplaceAll(relPath, "[Product]", t.Product)
			targetRelPath = strings.ReplaceAll(targetRelPath, "[sln]", "sln")
			targetRelPath = strings.ReplaceAll(targetRelPath, "[csproj]", "csproj")
			// log.Printf("targetRelPath: %s", targetRelPath)

			targetPath := filepath.Join(t.OutputPath, targetRelPath)

			// delete targetRelPath
			os.RemoveAll(targetPath)

			// If it's a directory, create it and return
			if info.IsDir() {
				return os.MkdirAll(targetPath, os.ModePerm)
			}

			// Read file content
			content, err := os.ReadFile(path)
			if err != nil {
				return err
			}

			// Replace [Company] and [Product] placeholders in content
			newContent := strings.ReplaceAll(string(content), "[Company]", t.Company)
			newContent = strings.ReplaceAll(newContent, "[Product]", t.Product)
			newContent = strings.ReplaceAll(newContent, "[product]", tools.LowerFirst(t.Product))
			newContent = strings.ReplaceAll(newContent, "[sln]", "sln")
			newContent = strings.ReplaceAll(newContent, "[csproj]", "csproj")

			// Create directory for the target file if it doesn't exist
			err = os.MkdirAll(filepath.Dir(targetPath), os.ModePerm)
			if err != nil {
				return err
			}

			// Write content to target path
			return os.WriteFile(targetPath, []byte(newContent), info.Mode())
		})

		if err != nil {
			log.Printf("Error generating %s: %v", template, err)
		}
	}
}
