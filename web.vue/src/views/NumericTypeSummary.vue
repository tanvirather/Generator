<!-------------------------------------------------- script -------------------------------------------------->
<script setup>
import { inject, onMounted, ref } from 'vue';
import { useRouter } from 'vue-router';
import { NumericTypeStore } from '../store';

/************************* properties *************************/
const apiClient = inject('apiClient');
const router = useRouter()
const model = ref([]);
const headers = [
  { text: 'Id', value: 'id' },
  { text: 'UpdatedById', value: 'updatedById' },
  { text: 'Updated', value: 'updated' },
  { text: 'Small Int Type', value: 'smallintType' },
  { text: 'Integer Type', value: 'integerType' },
  { text: 'Big Int_Type', value: 'bigintType' },
];
/************************* functions *************************/

onMounted(async () => {
  await loadData();
})

async function loadData() {
  model.value = await new NumericTypeStore(apiClient).get();
}

async function onAdd() {
  router.push(`/numericType/${crypto.randomUUID()}`);
}

async function onEdit(item) {
  router.push(`/numericType/${item.id}`);
}

async function onDelete(item) {
  await new NumericTypeStore(apiClient).delete(item.id);
}
</script>

<!-------------------------------------------------- template -------------------------------------------------->
<template>
  <div>
    <h1>NumericType</h1>
    <Table :headers="headers" :items="model" :showEdit="true" :showDelete="true" :showAdd="true" @edit="onEdit" @delete="onDelete" @add="onAdd" />
  </div>
</template>

<!-------------------------------------------------- style -------------------------------------------------->
<style scoped></style>
