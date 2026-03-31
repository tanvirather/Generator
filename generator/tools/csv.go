package tools

import (
	"fmt"
	"os"

	"github.com/gocarina/gocsv"
	"zuhid.com/generator/models"
)

// Csv provides methods for reading and processing Csv files.
type Csv struct{}

// ReadTables reads a Csv file and returns a slice of Table.
func (c *Csv) ReadTables(fileName string) ([]models.Table, error) {
	file, err := os.Open(fileName)
	if err != nil {
		return nil, fmt.Errorf("failed to open file: %w", err)
	}
	defer file.Close()

	var entries []models.Table
	if err := gocsv.UnmarshalFile(file, &entries); err != nil {
		return nil, fmt.Errorf("failed to unmarshal csv: %w", err)
	}
	return entries, nil
}

// ReadColumns reads a Csv file and returns a slice of Column.
func (c *Csv) ReadColumns(filePath string) ([]models.Column, error) {
	file, err := os.Open(filePath)
	if err != nil {
		return nil, fmt.Errorf("failed to open file: %w", err)
	}
	defer file.Close()

	var entries []models.Column
	if err := gocsv.UnmarshalFile(file, &entries); err != nil {
		return nil, fmt.Errorf("failed to unmarshal csv: %w", err)
	}

	return entries, nil
}
