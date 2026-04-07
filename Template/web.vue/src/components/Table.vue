<script setup>
import { computed } from "vue";

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
.table-container {
  overflow-x: auto;
}

.simple-table {
  width: 100%;
  border-collapse: collapse;
}

.simple-table th,
.simple-table td {
  padding: 8px;
  border: 1px solid #d0d0d0;
  text-align: left;
}

.simple-table th {
  background-color: #f5f5f5;
  font-weight: bold;
}

.simple-table tr:hover {
  background-color: #f9f9f9;
}

.action-btn {
  padding: 4px 8px;
  cursor: pointer;
}

.table-actions {
  margin-bottom: 10px;
  display: flex;
  justify-content: flex-end;
}

.add-btn {
  padding: 6px 12px;
  background-color: #4caf50;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

.add-btn:hover {
  background-color: #45a049;
}
</style>
