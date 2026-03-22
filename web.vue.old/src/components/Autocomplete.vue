<script setup>
import { ref, computed } from "vue";

/************************************************** Props **************************************************/
const model = defineModel({ type: [String, Number], default: "" });
const props = defineProps({
  id: { type: String, default: "" },
  label: { type: String, default: "" },
  disabled: { type: Boolean, default: false },
  items: { type: Array, default: () => [] },
  minChars: { type: Number, default: 1 },
});

/************************************************** Emits **************************************************/

/************************************************** Computed **************************************************/
const computedId = computed(() => props.id || props.label.toLowerCase().replace(/\s+/g, "_"));
const filtered = computed(() => props.items.filter(n => n.text.toLowerCase().includes(model.value.toLowerCase())));
/************************************************** fucntions **************************************************/

function selectItem(item) {
  model.value = item.text
}
</script>

<!-------------------------------------------------- template -------------------------------------------------->

<template>
  <div class="ctrl">
    <label v-if="label" :for="computedId">{{ label }}</label>
    <input :id="computedId" :placeholder="label" :disabled="disabled" v-model="model" autocomplete="off" />
    <div class="suggestions">
      <ul v-if="filtered.length > 0">
        <li v-for="(item) in filtered" :key="item.id" @click="selectItem(item)">
          {{ item.text }}
        </li>
      </ul>
    </div>
  </div>
</template>

<!-------------------------------------------------- style -------------------------------------------------->

<style scoped>
.ctrl {
  display: flex;
  flex-direction: column;
  gap: 6px;

  >label {
    font-weight: 600;
    font-size: 0.92rem;
  }

  >input {
    padding: 8px 10px;
    border: 1px solid #d0d0d0;
    border-radius: 6px;
    font-size: 1rem;
  }

  >input:focus {
    outline: none;
    border-color: #6aa0ff;
    box-shadow: 0 0 0 3px rgba(106, 160, 255, 0.12);
  }

  >.suggestions {
    position: relative;

    >ul {
      list-style: none;
      position: absolute;
      top: -20px;
      left: 0px;
      right: 0px;
      background-color: #6aa0ff;
      border-radius: 6px;
      max-height: 220px;
      overflow: auto;
      padding: 5px;
      text-align: left;

      >li {
        cursor: pointer;
      }
    }
  }

}
</style>
