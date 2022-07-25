import { createStore } from 'vuex'
import { auth } from "./auth-module"
import axios from 'axios';
import createPersistedState from "vuex-persistedstate";
export default createStore({
  state: {
    patients:[],
    questions:[],
    diseases:[],
  },
  getters: {
    patients: state => {
        return state.patients;
    },
    questions: state => {
      return state.questions;
  },
  diseases: state => {
    return state.diseases;
},
  },
  mutations: {
    SET_PATIENTS (state, patients) {
      state.patients = patients
  }, 
  SET_QUESTIONS (state, questions) {
    state.questions = questions
}, 
SET_DISEASES (state, diseases) {
  state.diseases = diseases
}, 
  },
  actions: {
    async loadPatientsForADoctor ({ commit }, user) {
      try {
         const response = await axios({
          method: 'get',
          url: "https://localhost:44333/GetAll/"+user.id, 
          headers:{
              'Authorization':'Bearer '+ user.token
          },
          responseType: 'json'
      });
        
         commit('SET_PATIENTS', response.data)
       }
       catch (error) {
        console.log(error);
      }
    },
    async loadQuestionsForPatient ({ commit }, user) {
      try {
         const response = await axios({
          method: 'get',
          url: "https://localhost:44333/api/Question", 
          headers:{
              'Authorization':'Bearer '+ user.token
          },
          responseType: 'json'
      });
        
         commit('SET_QUESTIONS', response.data)
       }
       catch (error) {
        console.log(error);
      }
    },
    async loadDiseases ({ commit }, user) {
      try {
         const response = await axios({
          method: 'get',
          url: "https://localhost:44333/api/Disease", 
          headers:{
              'Authorization':'Bearer '+ user.token
          },
          responseType: 'json'
      });
        console.log(response.data)
         commit('SET_DISEASES', response.data)
       }
       catch (error) {
        console.log(error);
      }
    },
  },
  modules: {
    auth,
  },
  
})