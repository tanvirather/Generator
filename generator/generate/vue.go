package generate

import (
	"fmt"
	"os"
	"path/filepath"
	"strings"

	"zuhid.com/generator/models"
	"zuhid.com/generator/tools"
)

// Vue represents a Vue code generator.
type Vue struct {
	Company    string
	Product    string
	InputPath  string
	OutputPath string
}

// Generate generates Vue code based on the provided table data.
func (vue *Vue) Generate(tables []models.Table) error {
	template := Template{
		Company:    vue.Company,
		Product:    vue.Product,
		OutputPath: vue.OutputPath,
		Folders:    []string{"web.vue"},
	}
	template.Generate()

	for _, table := range tables {
		filePath := fmt.Sprintf("%s/%s/%s.csv", vue.InputPath, table.Schema, table.Table)
		csv := tools.Csv{}
		columns, err := csv.ReadColumns(filePath)
		if err != nil {
			return err
		}

		vue.generateSummary(table, columns)
		vue.generateDetail(table, columns)
	}
	vue.generateRouter(tables)
	vue.generateNavigation(tables)

	return nil
}

func (vue *Vue) generateSummary(table models.Table, columns []models.Column) {
	headers := ""
	for _, column := range columns {
		label := column.Label
		if label == "" {
			label = vue.TableJs(column.Column)
		}
		headers += fmt.Sprintf("  { text: '%s', value: '%s' },\n", label, vue.ColumnJs(column.Column))
	}

	content := fmt.Sprintf(`<!-------------------------------------------------- script -------------------------------------------------->
<script setup>
import { inject, onMounted, ref } from 'vue';
import { useRouter } from 'vue-router';
import { %[2]sStore } from '../store';

/************************* properties *************************/
const apiClient = inject('apiClient');
const router = useRouter()
const model = ref([]);
const headers = [
%[4]s];
/************************* functions *************************/

onMounted(async () => {
  await loadData();
})

async function loadData() {
  model.value = await new %[2]sStore(apiClient).get();
}

async function onAdd() {
  router.push(%[1]s/%[3]s/${crypto.randomUUID()}%[1]s);
}

async function onEdit(item) {
  router.push(%[1]s/%[3]s/${item.id}%[1]s);
}

async function onDelete(item) {
  await new %[2]sStore(apiClient).delete(item.id);
}
</script>

<!-------------------------------------------------- template -------------------------------------------------->
<template>
  <div>
    <h1>%[2]s</h1>
    <Table :headers="headers" :items="model" :showEdit="true" :showDelete="true" :showAdd="true" @edit="onEdit" @delete="onDelete" @add="onAdd" />
  </div>
</template>

<!-------------------------------------------------- style -------------------------------------------------->
<style scoped></style>
`,
		"`",
		vue.TableJs(table.Table),
		vue.TableJsLower(table.Table),
		headers,
	)

	filePath := filepath.Join(vue.OutputPath, "web.vue", "src", "views", fmt.Sprintf("%sSummary.vue", vue.TableJs(table.Table)))
	_ = os.WriteFile(filePath, []byte(content), 0644)
}

func (vue *Vue) generateDetail(table models.Table, columns []models.Column) {
	fields := ""
	for _, column := range columns {
		label := column.Label
		if label == "" {
			label = vue.TableJs(column.Column)
		}
		fields += fmt.Sprintf("    <Number label=\"%s\" type=\"number\" v-model=\"model.%s\" />\n", label, vue.ColumnJs(column.Column))
	}

	content := fmt.Sprintf(`<!-------------------------------------------------- script -------------------------------------------------->
<script setup>
import { inject, onMounted, ref } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { %[1]sStore } from '../store';

/************************* properties *************************/
const apiClient = inject('apiClient');
const route = useRoute()
const router = useRouter()

const model = ref({
});
/************************* functions *************************/

onMounted(async () => {
  await loadData();
})

const onSubmit = async () => {
  await new %[1]sStore(apiClient).put(model.value);
  router.push("/%[2]s");
}
const onReset = async () => {
  await loadData();
}

async function loadData() {
  var result =  await new %[1]sStore(apiClient).get(route.params.id);
  if(result.length > 0){
    model.value = result[0];
  }
  else{
    model.value = {}
  }
}

</script>

<!-------------------------------------------------- template -------------------------------------------------->
<template>
  <Card title="%[1]s" @onSubmit="onSubmit" @onCancel="onReset">
%[3]s  </Card>
</template>

<!-------------------------------------------------- style -------------------------------------------------->
<style scoped></style>
`,
		vue.TableJs(table.Table),
		vue.TableJsLower(table.Table),
		fields)

	filePath := filepath.Join(vue.OutputPath, "web.vue", "src", "views", fmt.Sprintf("%sDetail.vue", vue.TableJs(table.Table)))
	_ = os.WriteFile(filePath, []byte(content), 0644)
}

func (vue *Vue) generateRouter(tables []models.Table) {
	routes := ""
	for _, table := range tables {
		routes += fmt.Sprintf("        { path: '%s', component: () => import('../views/%sSummary.vue') },\n", vue.TableJsLower(table.Table), vue.TableJs(table.Table))
		routes += fmt.Sprintf("        { path: '%s/:id', component: () => import('../views/%sDetail.vue') },\n", vue.TableJsLower(table.Table), vue.TableJs(table.Table))
	}

	content := fmt.Sprintf(`import { createRouter, createWebHistory } from 'vue-router';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    { path: '/login', component: () => import('../views/Login.vue') },
    {
      path: '/', component: () => import('../views/Index.vue'), meta: { requiresAuth: true },
      children: [
        { path: 'dashboard', component: () => import('../views/Dashboard.vue') },
%s        { path: '', redirect: 'dashboard' }
      ]
    },
    { path: '/:pathMatch(.*)*', redirect: '/' } // Catch-all route (must be LAST)
  ],
})

router.beforeEach((to, from, next) => {
  // const isAuthenticated = !!localStorage.getItem('authToken')
  const isAuthenticated = true
  const requiresAuth = to.meta.requiresAuth
  if (requiresAuth && !isAuthenticated) {
    next('/login')
  } else {
    next()
  }
})

export default router
`, routes)

	directory := filepath.Join(vue.OutputPath, "web.vue", "src", "router")
	tools.CreateFile(directory, "index.js", content)
}

func (vue *Vue) generateNavigation(tables []models.Table) {
	navigation := ""
	for _, table := range tables {
		navigation += fmt.Sprintf("    <router-link to=\"/%s\">%s</router-link>\n", vue.TableJsLower(table.Table), vue.TableJs(table.Table))
	}

	content := fmt.Sprintf(`<!-------------------------------------------------- script -------------------------------------------------->
<script setup>
</script>

<!-------------------------------------------------- template -------------------------------------------------->
<template>
  <nav>
    <router-link to="/dashboard">Dashboard</router-link>
%s  </nav>
   <router-view />
</template>

<!-------------------------------------------------- style -------------------------------------------------->
<style scoped></style>
`, navigation)

	filePath := filepath.Join(vue.OutputPath, "web.vue", "src", "views", "Index.vue")
	_ = os.WriteFile(filePath, []byte(content), 0644)
}

func (vue *Vue) TableJs(table string) string {
	parts := strings.Split(table, "_")
	for i, part := range parts {
		parts[i] = strings.Title(part)
	}
	return strings.Join(parts, "")
}

func (vue *Vue) TableJsLower(table string) string {
	res := vue.TableJs(table)
	if len(res) > 0 {
		return strings.ToLower(res[:1]) + res[1:]
	}
	return res
}

func (vue *Vue) ColumnJs(column string) string {
	parts := strings.Split(column, "_")
	for i, part := range parts {
		if i > 0 {
			parts[i] = strings.Title(part)
		} else {
			parts[i] = strings.ToLower(part)
		}
	}
	return strings.Join(parts, "")
}
