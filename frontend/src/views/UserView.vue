<template>
  <div><navbar></navbar></div>
  <div>
    <h1 style="font-family: Times New Roman; font-size: 300%">
      Hello, {{ this.user.lastName }} {{ this.user.firstName }}
    </h1>
    <notifications :width="600" position="top right" style="font-size: 30px" />

    <div
      class="container p-3 my-3 bg-light border card"
      style="width: 35rem; float: right; margin-right: 100px"
    >
      <img
        src="https://miro.medium.com/max/1016/0*FUXg3GXQhW5r5g5i.png"
        class="card-img-top"
        height="500"
      />
      <div class="card-body">
        <p
          style="font-family: Times New Roman; font-size: x-large"
          class="card-text"
        >
          If you have any of the symptoms mentioned above please take a
          questionnaire.
        </p>
      </div>
    </div>

    <div
      class="container p-3 my-3 bg-light border card"
      style="width: 30rem; float: right; margin-right: 50px"
    >
      <div class="card-body">
        <p style="font-family: Times New Roman; font-size: x-large">
          When to see a doctor
        </p>
        See a doctor if you or a loved one has memory problems or other dementia
        symptoms. Some treatable medical conditions can cause dementia symptoms,
        so it's important to determine the cause.
      </div>
      <div class="card-body">
        <p style="font-family: Times New Roman; font-size: x-large">
          Cognitive changes
        </p>

        <li>Memory loss, which is usually noticed by someone else</li>
        <br />
        <li>Difficulty communicating or finding words</li>
        <br />
        <li>Difficulty reasoning or problem-solving</li>
        <br />
        <li>Difficulty handling complex tasks</li>
        <br />
        <li>Difficulty with planning and organizing</li>
        <br />
        <li>Difficulty with coordination and motor functions</li>
        <br />
        <li>Confusion and disorientation</li>
        <br />
      </div>
      <div class="card-body">
        <p style="font-family: Times New Roman; font-size: x-large">
          Psychological changes
        </p>

        <li>Personality changes</li>
        <br />
        <li>Depression</li>
        <br />
        <li>Anxiety</li>
        <br />
        <li>Inappropriate behavior</li>
        <br />
        <li>Paranoia</li>
        <br />
        <li>Agitation</li>
        <br />
        <li>Hallucinations</li>
        <br />
      </div>
      <button class="btn btn-warning" @click="notification()">
        Get latest recommendation
      </button>
    </div>
  </div>
</template>

<script>
import navbar from "../components/UserNavigationBar.vue";
import axios from "axios";

import { notify } from "@kyvg/vue3-notification";
export default {
  components: {
    navbar,
    notify,
  },
  data() {
    return {
      reccomText: "",
      user: {},
    };
  },
  computed: {
    currentUser() {
      return this.$store.state.auth.user;
    },
  },
  mounted() {
    this.loadData();
  },
  methods: {
    loadData() {
      let AuthUser = this.$store.state.auth.user;
      this.user = AuthUser;
      axios({
        method: "get",
        url:
          "https://localhost:44333/api/RoutineDetection/lastrecommendation/" +
          AuthUser.elderId,

        headers: {
          Authorization: "Bearer " + AuthUser.token,
          "Content-Type": "application/json",
        },
        responseType: "json",
      })
        .then((response) => (this.reccomText = response.data))
        .then((error) => {});
    },
    notification() {
      notify({
        title: "Last available recommendation",
        text: this.reccomText,
        duration: 3000,

        closeOnClick: true,
      });
    },
  },
};
</script>

<style>
ul.b {
  list-style-type: square;
}
</style>
