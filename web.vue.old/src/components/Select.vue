<script setup>
import { computed } from "vue";

/************************************************** Props **************************************************/
const model = defineModel({ type: [String, Number], default: "" });
const props = defineProps({
  id: { type: String, default: "" },
  label: { type: String, default: "" },
  disabled: { type: Boolean, default: false },
  options: { type: Array, default: [] }, // [{id: "1", text: "Colorado"}],
});

/************************************************** Emits **************************************************/
defineEmits(["change"]);

/************************************************** Computed **************************************************/
const computedId = computed(() => props.id || props.label.toLowerCase().replace(/\s+/g, "_"));

/************************************************** functions **************************************************/
</script>

<template>
  <div class="ctrl">
    <label v-if="label" :for="computedId">{{ label }}</label>
    <select :id="computedId" :disabled="disabled" v-model="model" @change="$emit('change', $event)">
      <option></option>
      <option v-for="opt in options" :value="opt.id">{{ opt.text }}</option>
    </select>
    {{ model }}
  </div>
</template>

<style scoped>
.ctrl {
  display: flex;
  flex-direction: column;
  gap: 6px;

  >label {
    font-weight: 600;
    font-size: 0.92rem;
  }

  >select {
    padding: 8px 10px;
    border: 1px solid #d0d0d0;
    border-radius: 6px;
    font-size: 1rem;
  }

  >select:focus {
    outline: none;
    border-color: #6aa0ff;
    box-shadow: 0 0 0 3px rgba(106, 160, 255, 0.12);
  }
}
</style>
