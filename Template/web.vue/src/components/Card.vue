<script setup>
/************************************************** Props **************************************************/
const props = defineProps({
  title: { type: String, default: "" },
  saveText: { type: String, default: 'Save' },
  cancelText: { type: String, default: 'Cancel' }
});

/************************************************** Emits **************************************************/
defineEmits(["onSubmit", "onCancel"]);

/************************************************** Computed **************************************************/

/************************************************** functions **************************************************/
</script>

<!-------------------------------------------------- template -------------------------------------------------->
<template>
  <div class="card">
    <header>{{ title }}</header>
    <main class="card-content"><slot /></main>
    <footer class="card-actions">
      <slot name="footer">
        <button id="save-btn" v-if="saveText.length > 0" @click="$emit('onSubmit')">{{ saveText }}</button>
        <button id="cancel-btn" v-if="cancelText.length > 0" @click="$emit('onCancel')">{{ cancelText }}</button>
      </slot>
    </footer>
  </div>
</template>

<!-------------------------------------------------- style -------------------------------------------------->
<style scoped>
.card {
  display: flex;
  flex-direction: column;
  gap: 6px;
  border: 1px solid #d0d0d0;
  border-radius: 6px;
  padding: 16px;
}

.card header {
  font-weight: bold;
}

.card main {
  margin-top: 8px;
}

.card footer {
  margin-top: auto;
}
</style>
