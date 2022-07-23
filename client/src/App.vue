<script setup>
import House from './components/House.vue';
import User from './components/User.vue';
import Chat from './components/Chat.vue';
</script>

<template>
  <div class="main-container">
    <div class="left">
      <User />
    </div>
    <div class="center">
      <House />
    </div>
    <div class="right">
      <Chat />
    </div>
  </div>

</template>
<style scoped>
.main-container {
  display: flex;
  width: 960px;
}

.left {
  flex: 1
}

.center {
  flex: 2;
}

.right {
  flex: 1
}
</style>