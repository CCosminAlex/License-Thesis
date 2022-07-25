import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'
import $ from 'jquery'
import "bootstrap/dist/css/bootstrap.css"
import "bootstrap/dist/js/bootstrap"
import { library } from '@fortawesome/fontawesome-svg-core'
import { faHatWizard } from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'
import VueGoogleCharts from 'vue3-googl-chart'
import Notifications from '@kyvg/vue3-notification'
import VCalendar from 'v-calendar';

library.add(faHatWizard);

createApp(App).use(store).use(router).use(VueGoogleCharts).use(VCalendar,{componentPrefix:'vc'}).use(Notifications).use(FontAwesomeIcon).use(library).use($).mount('#app')
