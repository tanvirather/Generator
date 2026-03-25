<!-------------------------------------------------- script -------------------------------------------------->
<script setup>
import { inject, onMounted, ref } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { NumericTypeStore } from '../store';

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
  await new NumericTypeStore(apiClient).put(model.value);
  router.push("/numericType");
}
const onReset = async () => {
  await loadData();
}

async function loadData() {
  var result =  await new NumericTypeStore(apiClient).get(route.params.id);
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
  <Card title="NumericType" @onSubmit="onSubmit" @onCancel="onReset">
    <Number label="Id" type="number" v-model="model.id" />
    <Number label="UpdatedById" type="number" v-model="model.updatedById" />
    <Number label="Updated" type="number" v-model="model.updated" />
    <Number label="Small Int Type" type="number" v-model="model.smallintType" />
    <Number label="Integer Type" type="number" v-model="model.integerType" />
    <Number label="Big Int_Type" type="number" v-model="model.bigintType" />
  </Card>
</template>

<!-------------------------------------------------- style -------------------------------------------------->
<style scoped></style>
