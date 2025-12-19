<template>
  <v-app>
    <v-main>
      <v-container>
        <v-card class="pa-4">
          <h2>Home</h2>
          <v-text-field v-model="name" label="Seu nome" />
          <v-btn @click="sayHi">Diz oi</v-btn>
        </v-card>
      </v-container>
    </v-main>
  </v-app>
</template>

<script setup>
import { ref } from 'vue'
const name = ref('')
function sayHi() {
  alert(`E aí ${name.value || 'usuário'}!`)
}
</script>
