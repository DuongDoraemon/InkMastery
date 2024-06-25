/* eslint-disable import/order */
import "@/@fake-db/db";
import "@/@iconify/icons-bundle";
import App from "@/App.vue";
import ability from "@/plugins/casl/ability";
import i18n from "@/plugins/i18n";
import vuetify from "@/plugins/vuetify";
import { loadFonts } from "@/plugins/webfontloader";
import router from "@/router";
import { abilitiesPlugin } from "@casl/vue";
import "@core/scss/template/index.scss";
import "@styles/styles.scss";
import {
  Button,
  DatePicker,
  Divider,
  Flex,
  Grid,
  Layout,
  Segmented,
  Select,
  Skeleton,
  Spin,
  Statistic,
  Table,
  Tabs,
  Upload,
} from "ant-design-vue";
import { createPinia } from "pinia";
import { createApp, h } from "vue";

import * as echarts from "echarts";
import { plugin } from "echarts-for-vue";
loadFonts();

// Create vue app
const app = createApp(App);

// Use plugins
app.use(vuetify);
app.use(createPinia());
app.use(plugin, { echarts, h });
app.use(router);
app.use(DatePicker);
app.use(Button);
app.use(Layout);
app.use(Divider);
app.use(Spin);
app.use(Flex);
app.use(Upload);
app.use(Skeleton);
app.use(Grid);
app.use(Segmented);
app.use(Statistic);
app.use(Table);
app.use(Tabs);
app.use(Select);
app.use(i18n);
app.use(abilitiesPlugin, ability, {
  useGlobalProperties: true,
});

// Mount vue app
app.mount("#app");
