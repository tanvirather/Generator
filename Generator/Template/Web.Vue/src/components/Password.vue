<script setup>
import { computed } from "vue";

/************************************************** Props **************************************************/
const model = defineModel({ type: [String, Number], default: "" });
const props = defineProps({
  id: { type: String, default: "" },
  label: { type: String, default: "" },
  disabled: { type: Boolean, default: false },
});

/************************************************** Emits **************************************************/
defineEmits(["blur"]);

/************************************************** Computed **************************************************/
const computedId = computed(() => props.id || props.label.toLowerCase().replace(/\s+/g, "_"));

/************************************************** functions **************************************************/
</script>

<!-------------------------------------------------- template -------------------------------------------------->
<template>
  <div class="ctrl">
    <label v-if="label" :for="computedId">{{ label }}</label>
    <input :id="computedId" type="password" :placeholder="label" :disabled="disabled" v-model="model" @blur="$emit('blur', $event)" />
  </div>
</template>

<!-------------------------------------------------- style -------------------------------------------------->
<style scoped>
.ctrl {
  display: flex;
  flex-direction: column;
  gap: 6px;

  > label {
    font-weight: 600;
    font-size: 0.92rem;
  }

  > input {
    padding: 8px 10px;
    border: 1px solid #d0d0d0;
    border-radius: 6px;
    font-size: 1rem;
  }

  > input:focus {
    outline: none;
    border-color: #6aa0ff;
    box-shadow: 0 0 0 3px rgba(106, 160, 255, 0.12);
  }
}
</style>
