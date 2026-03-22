<script setup>

import { useSignalR } from '@/plugins/signalr';
import { onBeforeUnmount, onMounted, ref } from 'vue';

/************************* Props *************************/
const hub = useSignalR();
const notifications = ref([]);
const isConnected = ref(false);
/************************* emits *************************/
/************************* computed *************************/
/************************* functions *************************/

function handlePush(payload) {
  console.log('Received message from SignalR hub:', payload);
  notifications.value.unshift(payload);
}

function updateState() {
  // 2 means Connected
  isConnected.value = hub.getState() === 2;
}

onMounted(async () => {
  // Subscribe to an event your server hub emits, e.g. "PushNotification"
  hub.on('ReceiveMessage', handlePush);

  updateState();

  if (hub.getState() !== 2) {
    // await hub.start();
    updateState();
  }
});

onBeforeUnmount(() => {
  hub.off('ReceiveMessage', handlePush);
});

</script>

<!-------------------------------------------------- template -------------------------------------------------->

<template>
  <section>
    <header style="display:flex;align-items:center;gap:.5rem;">
      <h2>Live Feed</h2>
      <span
        :style="{
          display:'inline-block',
          width:'8px',
          height:'8px',
          borderRadius:'9999px',
          backgroundColor: isConnected ? '#22c55e' : '#9ca3af'
        }"
        aria-label="connection status"/>
    </header>

    <ul>
      <li v-for="n in notifications" :key="n.id">
        — {{ JSON.stringify(n) }}
      </li>
    </ul>
  </section>
</template>


<!-------------------------------------------------- style -------------------------------------------------->
<style scoped>
</style>
