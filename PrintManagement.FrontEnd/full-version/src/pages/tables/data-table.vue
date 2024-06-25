<script setup>
import * as demoCode from "@/views/demos/forms/tables/simple-table/demoCodeSimpleTable";
import ChatLeftSidebarContent from "@/views/apps/chat/ChatLeftSidebarContent.vue";
</script>

<template>
  <v-row>
    <v-col>
      <div class="seach">
        <VNavigationDrawer
          v-model="isLeftSidebarOpen"
          absolute
          touchless
          location="start"
          width="370"
          :temporary="$vuetify.display.smAndDown"
          class="chat-list-sidebar"
          :permanent="$vuetify.display.mdAndUp"
        >
          <ChatLeftSidebarContent
            v-model:isDrawerOpen="isLeftSidebarOpen"
            v-model:search="q"
            @open-chat-of-contact="openChatOfContact"
            @show-user-profile="isUserProfileSidebarOpen = true"
            @close="isLeftSidebarOpen = false"
          />
        </VNavigationDrawer>
      </div>
    </v-col>
    <v-col></v-col>
  </v-row>
  <VRow>
    <VCol cols="12">
      <DemoSimpleTableBasic height="495" />
    </VCol>
  </VRow>
</template>
<script>
export default {};
</script>
