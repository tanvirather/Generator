<!-------------------------------------------------- script -------------------------------------------------->
<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import Page from '../components/Page.vue';

const router = useRouter()
const model = ref({
  username: 'admin@company.com',
  password: 'P@ssw0rd'
});

/************************* emits *************************/
/************************* computed *************************/
/************************* functions *************************/
async function login() {
  router.push('/')
}
</script>

<!-------------------------------------------------- template -------------------------------------------------->
<template>
  <Page saveText="">
    <Card title="Login" saveText="Login" cancelText="" @onSubmit="login">
      <Input label="Username" v-model="model.username" />
      <Password label="Password" v-model="model.password" />
    </Card>
  </Page>
</template>

<!-------------------------------------------------- style -------------------------------------------------->
<style scoped></style>
