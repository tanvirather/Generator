package tools

import (
	"fmt"
	"os"
)

func CreateFile(directory string, filePath string, content string) {
	os.MkdirAll(directory, os.ModePerm)
	f, e := os.Create(directory + "/" + filePath)
	if e != nil {
		fmt.Println(e)
	}
	e = os.Chmod(directory+"/"+filePath, 0766)
	if e != nil {
		fmt.Println(e)
	}
	f.WriteString(content)
	f.Close()
}
