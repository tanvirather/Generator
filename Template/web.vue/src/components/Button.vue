<script setup>
import { computed } from "vue";

/************************************************** Props **************************************************/
const props = defineProps({
  id: { type: String, default: "" },
  label: { type: String, default: "" },
  disabled: { type: Boolean, default: false },
});

/************************************************** Emits **************************************************/
defineEmits(["click"]);

/************************************************** Computed **************************************************/
const computedId = computed(() => props.id || props.label.toLowerCase().replace(/\s+/g, "_"));

/************************************************** functions **************************************************/
</script>

<!-------------------------------------------------- template -------------------------------------------------->
<template>
  <div class="ctrl">
    <button :id="computedId" :disabled="disabled" @click="$emit('click', $event)">{{ label }}</button>
  </div>
</template>

<!-------------------------------------------------- style -------------------------------------------------->
<style scoped>
.ctrl {
  display: flex;
  flex-direction: column;
}
</style>
