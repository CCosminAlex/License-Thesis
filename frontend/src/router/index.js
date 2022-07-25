import { createRouter, createWebHashHistory } from 'vue-router'
import Login from '../views/Login.vue'
import Register from '../views/Register.vue'
import DoctorView from '../views/DoctorView.vue'
import Patients from '../views/Patients.vue'
import AddPatient from '../views/AddPatient.vue'
import Questionnaire from '../views/Questionnaire.vue'
import PatientInfo from '../views/PatientInfo.vue'
import AddDiseaseToPatient from '../views/AddDiseaseToPatient'
import Diseases from '../views/Diseases'
import PatientQuestionnaireInfo from '../views/PatientQuestionnaireInfo'
import PermissionDenied from '../views/PermissionDenied'
import UserView from '../views/UserView'
import Timeline from "../views/Timeline"
import AdminView from "../views/AdminView"
import Routine from "../views/Routine"
import Recommendation from "../views/Recommendation"

function isAuthenticated(to, from, next) {
  const user = JSON.parse(localStorage.getItem("user"));
  console.log(user)
  if (user) {
        next();
    } else {
        next('/');
    }
}

function isDoctor(to, from, next) {
  const user = JSON.parse(localStorage.getItem("user"));
  if (user.role == "Doctor") {
      next();
  }else{
    next("/permissionDenied")
  }
}

function isUser(to, from, next) {
  const user = JSON.parse(localStorage.getItem("user"));
  if (user.role == "User") {
      next();
  }else{
    next("/permissionDenied")
  }
}

function isAdmin(to, from, next) {
  const user = JSON.parse(localStorage.getItem("user"));
  if (user.role == "Admin") {
      next();
  }else{
    next("/permissionDenied")
  }
}


const routes = [
  {
    path: '/',
    name: 'Login',
    component: Login
  },
  {
    path: '/register',
    name: 'Register',
    component: Register
  },
  {
    path: '/doctorView',
    name: 'DoctorView',
    component: DoctorView,
   beforeEnter:[isAuthenticated,isDoctor]
  },
  {
    path: '/patients',
    name: 'Patients',
    component: Patients,
  },
  {
    path: '/patientAdd',
    name: 'AddPatient',
    component: AddPatient
  },
  {
    path: '/questionnaire',
    name: 'Questionnaire',
    component: Questionnaire,
    beforeEnter:[isAuthenticated,isUser]
  },
  {
    path: '/patientFile/:patientId',
    name: 'PatientInfo',
    component: PatientInfo,
    props:true,
  },
  {
    path: '/addDiseaseToPatient',
    name: 'AddDiseaseToPatient',
    component: AddDiseaseToPatient
  },
  {
    path: '/diseases',
    name: 'Diseases',
    component: Diseases,
    beforeEnter:[isAuthenticated,isDoctor]
  },
  {
    path: '/questionnaireInfo/:patientId',
    name: 'PatientQuestionnaireInfo',
    component: PatientQuestionnaireInfo,
    props:true,
  },
  {
    path: '/permissionDenied',
    name: 'PermissionDenied',
    component: PermissionDenied
  },
  {
    path: '/timeline',
    name: 'Timeline',
    component: Timeline
  },
  {
    path: '/recommendation',
    name: 'Recommendation',
    component:Recommendation
  },
  {
    path: '/userView',
    name: 'UserView',
    component: UserView,
    beforeEnter:[isAuthenticated,isUser]
  },
  {
    path: '/adminView',
    name: 'AdminView',
    component: AdminView,
    beforeEnter:[isAuthenticated,isAdmin]
  },
  {
    path: '/routine/:patientId',
    name: 'Routine',
    component: Routine,
    props:true,
  },
]

const router = createRouter({
  history: createWebHashHistory(),
  routes
})

export default router
