<template>
  <div><navbar></navbar></div>
<notifications :width="600" position="top right" style="font-size: 30px" />

  <div v-if="started == 1">
    <div
      class="container p-3 my-3 bg-light border border-3 shadow-sm p-3 mb-5 bg-body rounded" style="float:right"
    >
      <div class="progress" style="height: 20px">
        <div
          class="progress-bar progress-bar-striped progress-bar-animated"
          role="progressbar"
          :style="{ width: progressBarStatus + '%' }"
          :aria-valuenow="progressBarStatus"
          aria-valuemin="0"
          aria-valuemax="100"
        >
          Completed
        </div>
      </div>
      <br />
      <transition-group name="fadeLeft" tag="div">
        <div v-for="(question, index) in questions" :key="index">
          <div v-if="index === questionIndex">
            <div
              style="font-family: Times New Roman; font-size: xx-large"
              class="shadow p-3 mb-5 bg-body rounded"
            >
              {{ question.statement }}
            </div>
            <div
              v-for="(answer, index) in question.answers"
              :key="index"
              style="display: inline-block"
            >
              <div class="form-check form-check-inline">
                <input
                  class="form-check-input"
                  type="radio"
                  name="inlineRadioOptions"
                  :id="'inlineradio' + answer.text"
                  v-model="picked"
                  :value="answer"
                />
                <label
                  style="font-family: Times New Roman; font-size: x-large"
                  class="form-check-label"
                  :for="'inlineradio' + answer.text"
                  >{{ answer.text }}</label
                >
              </div>
            </div>
          </div>
        </div>
      </transition-group>

      <br />

      <div class="row g-0">
        <div class="col-sm-6 col-md-1"></div>
        <div class="col-6 col-md-11" style="text-align: right">
          <button
            v-show="buttonStatusNext"
            id="but"
            @click="next()"
            class="btn btn-primary"
            type="submit"
          >
            Next
          </button>
          <button
            v-show="buttonStatusSave"
            id="but"
            @click="save()"
            class="btn btn-success"
            type="submit"
          >
            Save
          </button>
        </div>
      </div>
    </div>
  </div>

  <div v-else-if="started == 0">
    <div
      class="container p-3 my-3 bg-light border border-3 shadow-sm p-3 mb-5 bg-body rounded" style="float:right"
    >
      <h2 style="font-family: Times New Roman">Hello</h2>
      <h4 style="font-family: Times New Roman">
        Do you want to take a questionnaire about your physical and mental
        health?
      </h4>
      <br />
      <button class="btn btn-primary" @click="start()" type="button">
        START QUESTIONNAIRE
      </button>
    </div>
  </div>
  <div v-else-if="started == 2">
    <div
      class="container p-3 my-3 bg-light border border-3 shadow-sm p-3 mb-5 bg-body rounded" style="float:right"
    >
      <div class="text-center">
        <div
          class="spinner-border text-secondary"
          style="width: 4rem; height: 4rem"
          role="status"
        ></div>
        <div>
          <h3 style="font-family: Times New Roman">Please be patient</h3>
        </div>
        <div>
          <h2 style="font-family: Times New Roman">
            Your answers are being saved.
          </h2>
        </div>
      </div>
    </div>
  </div>
  <div v-else-if="started == 3">
    <div
      class="container p-3 my-3 bg-light border border-3 shadow-sm p-3 mb-5 bg-body rounded" style="float:right"
    >
      <div>
        <h3 style="font-family: Times New Roman">
          Thank you for your response, your answers have been saved.
        </h3>
      </div>
      <br />
      <button @click="home()" type="button" class="btn btn-primary btn-lg">
        Home
      </button>
    </div>
  </div>
</template>

<script>
import navbar from "../components/UserNavigationBar.vue";
import axios from "axios";
import { notify } from "@kyvg/vue3-notification";
export default {
  components:{
  },
  data() {
    return {
      questionIndex: 0,
      buttonStatusNext: true,
      buttonStatusSave: false,
      progressBarStatus: 0,
      started: 0,
      choosenAnswer: [],
      picked: {},
      patientAnswer: [],
      questionnaireResponse: "",
      patientIdClass: {
        patientId: "",
      },
    };
  },
  computed: {
    currentUser() {
      return this.$store.state.auth.user;
    },
    questions() {
      return this.$store.getters.questions;
    },
  },
  mounted() {
    this.$store.dispatch("loadQuestionsForPatient", this.currentUser);
  },
  methods: {
    next() {
      console.log(this.picked);
      if (Object.keys(this.picked).length == 0) {
        
      notify({
        title: "Warning",
        text: "You must select an answer.",
        duration: 3000,
        type: "error",
        closeOnClick: true,
      });
        return;
      }
      this.patientAnswer.push(this.picked);

      this.progressBarStatus += 100 / this.questions.length;
      if (this.questions.length - 2 == this.questionIndex) {
        this.buttonStatusNext = false;
        this.buttonStatusSave = 1;
      }
      this.questionIndex++;
      this.picked = {};
    },
    save() {
      if (Object.keys(this.picked).length == 0) {
           
      notify({
        title: "Warning",
        text: "You must select an answer.",
        duration: 3000,
        type: "error",
        closeOnClick: true,
      });
      return}
      this.patientAnswer.push(this.picked);
      let AuthUser = this.$store.state.auth.user;

      this.createQuestionnaire().then((data) => {
        this.patientAnswer.forEach((element) => {
          element["questionnaireId"] = data;
          delete element.text;
        });
        axios
          .post(
            "https://localhost:44333/api/PatientAnswer",
            { patientAnswerDtos: this.patientAnswer },
            {
              headers: { Authorization: "Bearer " + AuthUser.token },
              "Content-Type": "application/json",
            }
          )
          .then((response) => this.wait());
      });
    },
    start() {
      this.progressBarStatus += 100 / this.questions.length;
      this.started = 1;
    },
    selected(answer) {
      
      this.patientAnswer.push(answer);
    },
    async createQuestionnaire() {
      let AuthUser = this.$store.state.auth.user;
      this.patientIdClass.patientId = AuthUser.id;
      try {
        let res = await axios.post(
          "https://localhost:44333/api/Questionnaire",
          this.patientIdClass,
          {
            headers: { Authorization: "Bearer " + AuthUser.token },
            "Content-Type": "application/json",
          }
        );
        return res.data;
      } catch {
        
      }
    },
    wait() {
      this.started = 2;
      setTimeout(() => {
        this.started = 3;
      }, 3000);
    },
    home() {
      this.$router.push("/userView");
    },
  },
  components: {
    navbar,
    notify,
  },
};
</script>

<style>
/* .slide-fade-enter-active {
  transition: all 0.3s ease;
}
.slide-fade-leave-active {
  transition: all 0.8s cubic-bezier(1, 0.5, 0.8, 1);
}
.slide-fade-enter, .slide-fade-leave-to
.slide-fade-leave-active for <2.1.8 {
  transform: translateX(10px);
  opacity: 0;
  overflow: hidden;
} */
</style>
