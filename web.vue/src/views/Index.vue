<!-------------------------------------------------- script -------------------------------------------------->
<script setup>
</script>

<!-------------------------------------------------- template -------------------------------------------------->
<template>
  <nav>
    <router-link to="/dashboard">Dashboard</router-link>
    <router-link to="/numericType">NumericType</router-link>
  </nav>
   <router-view />
</template>

<!-------------------------------------------------- style -------------------------------------------------->
<style scoped>
/* nav {
  background-color: var(--surface-color);
  padding: var(--spacing-md);
  border-bottom: 1px solid var(--border-color);
  display: flex;
  gap: var(--spacing-md);
  margin-bottom: var(--spacing-lg);
}

nav a {
  font-weight: 500;
  padding: 4px 8px;
  border-radius: var(--radius-sm);
}

nav a.router-link-active {
  background-color: var(--primary-light);
  color: var(--primary-color);
} */
</style>
