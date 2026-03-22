<script setup>

import { UserStore } from '@/store';
import { inject, onMounted, ref, toRaw } from 'vue';

const apiClient = inject('apiClient');

/************************* Props *************************/

const columns = [
  // { key: 'id', title: 'Id', type: 'hidden' },
  { key: 'email', title: 'Email', type: 'email' },
]

let users = ref([])

/************************* emits *************************/
/************************* computed *************************/
/************************* functions *************************/
onMounted(async () => {
  users.value = await new UserStore(apiClient).get();
}); 
function add() {
  users.value.push({email: 'test@example.com'});
}
async function save() {
  await new UserStore(apiClient).save(toRaw(users.value));
}

</script>

<!-------------------------------------------------- template -------------------------------------------------->
<template>
  <input type="button" value="Add" @click="add" />
  <Table :columns="columns" :rows="users"/>
  <input type="button" value="Save" @click="save" />
</template>

<!-------------------------------------------------- style -------------------------------------------------->
<style scoped>
td {
  border: 1px solid #d0d0d0;
}
</style>
