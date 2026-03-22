<!-------------------------------------------------- script -------------------------------------------------->
<script setup>
import { inject, ref } from 'vue';
import { useRouter } from 'vue-router';

const apiClient = inject('apiClient');
const router = useRouter()

const model = ref({
  username: 'admin@company.com',
  password: 'P@ssw0rd'
});

/************************* emits *************************/
/************************* computed *************************/
/************************* functions *************************/
async function login() {
  // await new LoginStore(apiClient).login(model.value);
  // console.log("Login button clicked" + JSON.stringify(model.value));
  router.push('/Product/PostgresType')
}
</script>

<!-------------------------------------------------- template -------------------------------------------------->
<template>
  <div>
    <Card title="Login" saveText="Login" cancelText="" @onSubmit="login">
      <Input label="Username" v-model="model.username" />
      <Password label="Password" v-model="model.password" />
    </Card>
  </div>
</template>

<!-------------------------------------------------- style -------------------------------------------------->
<style scoped></style>
