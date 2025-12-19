<script setup lang="ts">
import { useRouter } from "vue-router";
import { useAuthStore } from "@/stores/authStore";
import Sidebar from "@/components/Sidebar.vue";

const router = useRouter();
const store = useAuthStore();

const logout = () => {
  store.logout();
  router.push({ name: 'login' });
};

const goDashboard = () => router.push({ name: 'dashboard' });
const goAbout = () => router.push("/about");
</script>

<template>
  <v-app>
    <v-app-bar color="primary" dark app>
      <v-toolbar-title>Gestão Logística</v-toolbar-title>
      <v-spacer></v-spacer>
      <v-btn text @click="goDashboard">Dashboard</v-btn>
      <v-btn text @click="goAbout" disabled>Sobre Nós</v-btn>
      <v-btn text color="error" @click="logout">Sair</v-btn>
    </v-app-bar>

    <v-main>
      <v-container class="pa-4">
        <!-- Sidebar -->
        <Sidebar />
        
        <slot></slot>
      </v-container>
    </v-main>
  </v-app>
</template>