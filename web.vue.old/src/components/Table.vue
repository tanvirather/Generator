<script setup>
/************************* Props *************************/
const props = defineProps({
  columns: { type: Array, default: [] },  
  rows: { type: Array, default: [] },  
});

/************************* emits *************************/
/************************* computed *************************/
/************************* functions *************************/
</script>

<!-------------------------------------------------- template -------------------------------------------------->
<template>
  <table>
    <thead>
      <tr>
        <th v-for="col in columns" :key="col.key">{{ col.title }}</th>
      </tr>
    </thead>
    <tbody>
      <tr v-for="row in rows" :id="row.id" :key="row.id">
        <td v-for="col in columns" :key="col.key">
          <input v-if="col.type === 'text'" type="text" v-model="row[col.key]" />
          <input v-else-if="col.type === 'hidden'" type="hidden" v-model="row[col.key]" />
          <input v-else-if="col.type === 'number'" type="number" v-model="row[col.key]" />
          <input v-else-if="col.type === 'email'" type="email" v-model="row[col.key]" />
          <input v-else-if="col.type === 'date'" type="date" v-model="row[col.key]" />
          <input v-else-if="col.type === 'datetime-local'" type="datetime-local" v-model="row[col.key]" />
          <input v-else-if="col.type === 'time'" type="time" v-model="row[col.key]" />
          <span v-else>{{ row[col.key] }}</span>
        </td>
      </tr>
    </tbody>
  </table>
</template>

<!-------------------------------------------------- style -------------------------------------------------->
<style scoped>
</style>
