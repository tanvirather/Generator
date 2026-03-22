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
defineEmits(["select", "blur"]);

/************************************************** Computed **************************************************/
const computedId = computed(() => props.id || props.label.toLowerCase().replace(/\s+/g, "_"));

/************************************************** State & Helpers **************************************************/
const isOpen = ref(false);
const highlighted = ref(-1);
const inputRef = ref(null);

const itemLabel = (it) => (it && typeof it === "object" ? it.label ?? it.value ?? String(it) : String(it));

const filtered = computed(() => {
  const q = String(model ?? "").trim();
  if (!q || q.length < props.minChars) return [];
  const lower = q.toLowerCase();
  return props.items.filter((it) => itemLabel(it).toLowerCase().includes(lower));
});

function openIfNeeded() {
  isOpen.value = filtered.value.length > 0;
}

function selectItem(it) {
  model.value = itemLabel(it);
  isOpen.value = false;
  highlighted.value = -1;
  emit("select", it);
}

function onInput() {
  openIfNeeded();
}

function onBlur(e) {
  // slight delay so clicks on suggestions register
  setTimeout(() => {
    isOpen.value = false;
    highlighted.value = -1;
    emit("blur", e);
  }, 150);
}

function onKeydown(e) {
  if (!isOpen.value && (e.key === "ArrowDown" || e.key === "ArrowUp")) {
    openIfNeeded();
  }
  if (!isOpen.value) return;

  if (e.key === "ArrowDown") {
    e.preventDefault();
    highlighted.value = Math.min(highlighted.value + 1, filtered.value.length - 1);
  } else if (e.key === "ArrowUp") {
    e.preventDefault();
    highlighted.value = Math.max(highlighted.value - 1, 0);
  } else if (e.key === "Enter") {
    e.preventDefault();
    if (highlighted.value >= 0 && highlighted.value < filtered.value.length) {
      selectItem(filtered.value[highlighted.value]);
    }
  } else if (e.key === "Escape") {
    isOpen.value = false;
    highlighted.value = -1;
  }
}
</script>

<!-------------------------------------------------- template -------------------------------------------------->

<template>
  <div class="ctrl" :class="{ open: isOpen }">
    <label v-if="label" :for="computedId">{{ label }}</label>
    <div class="wrap">
      <input ref="inputRef" :id="computedId" :placeholder="label" :disabled="disabled" v-model="model" @input="onInput"
        @blur="onBlur" @keydown="onKeydown" autocomplete="off" />

      <ul v-if="isOpen" class="suggestions">
        <li v-for="(it, idx) in filtered" :key="idx" :class="{ highlighted: idx === highlighted }"
          @mousedown.prevent="selectItem(it)">
          {{ itemLabel(it) }}
        </li>
        <li v-if="filtered.length === 0" class="empty">No results</li>
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

  .wrap {
    position: relative;
  }

  input {
    padding: 8px 10px;
    border: 1px solid #d0d0d0;
    border-radius: 6px;
    font-size: 1rem;
    width: 100%;
  }

  input:focus {
    outline: none;
    border-color: #6aa0ff;
    box-shadow: 0 0 0 3px rgba(106, 160, 255, 0.12);
  }

  .suggestions {
    position: absolute;
    left: 0;
    right: 0;
    margin: 6px 0 0 0;
    padding: 0;
    list-style: none;
    border: 1px solid #d0d0d0;
    border-radius: 6px;
    background: white;
    max-height: 220px;
    overflow: auto;
    z-index: 10;
  }

  .suggestions>li {
    padding: 8px 10px;
    cursor: pointer;
  }

  .suggestions>li.highlighted {
    background: #f0f8ff;
  }

  .suggestions>li.empty {
    color: #888;
    cursor: default;
  }
}
</style>
