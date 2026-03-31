<script setup>

/************************************************** Props **************************************************/
const props = defineProps({
  headers: { type: Array, default: () => [] },
  items: { type: Array, default: () => [] },
  showEdit: { type: Boolean, default: false },
  showDelete: { type: Boolean, default: false },
  showAdd: { type: Boolean, default: false },
});

/************************************************** Emits **************************************************/
const emit = defineEmits(["edit", "delete", "add"]);

/************************************************** Computed **************************************************/

/************************************************** functions **************************************************/
function onEdit(item) {
  emit("edit", item);
}

function onDelete(item) {
  emit("delete", item);
}

function onAdd() {
  emit("add");
}
</script>

<!-------------------------------------------------- template -------------------------------------------------->
<template>
  <div class="table-container">
    <div v-if="showAdd" class="table-actions">
      <button class="add-btn" @click="onAdd">Add</button>
    </div>
    <table class="simple-table">
      <thead>
        <tr>
          <th v-for="header in headers" :key="header.value">
            {{ header.text }}
          </th>
          <th v-if="showEdit">Edit</th>
          <th v-if="showDelete">Delete</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="(item, index) in items" :id="item.id" :key="item.id">
          <td v-for="header in headers" :key="header.value">
            <slot :name="`item-${header.value}`" :item="item">
              {{ item[header.value] }}
            </slot>
          </td>
          <td v-if="showEdit">
            <button class="action-btn" @click="onEdit(item)">Edit</button>
          </td>
          <td v-if="showDelete">
            <button class="action-btn" @click="onDelete(item)">Delete</button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<!-------------------------------------------------- style -------------------------------------------------->
<style scoped>
/* .table-container {
  overflow-x: auto;
  border-radius: var(--radius-lg);
  border: 1px solid var(--border-color);
  background-color: var(--surface-color);
  box-shadow: var(--shadow-sm);
}

.table-actions {
  padding: var(--spacing-md);
  display: flex;
  justify-content: flex-start;
  border-bottom: 1px solid var(--border-color);
}

.add-btn {
  background-color: var(--success-color);
  color: white;
}

.add-btn:hover {
  background-color: var(--success-hover);
}

.action-btn {
  padding: 4px 12px;
  background-color: var(--surface-color);
  border: 1px solid var(--border-color);
  color: var(--text-secondary);
}

.action-btn:hover {
  background-color: var(--primary-light);
  border-color: var(--primary-color);
  color: var(--primary-color);
} */
</style>
