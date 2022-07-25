<template>
  <div style="margin-right: 450px">
    <navbar></navbar>
    <notifications :width="600" position="top right" style="font-size: 30px" />
  </div>
  <br />
  <br />
  <div class="container" style="margin-left: 200px">
    <ul class="nav nav-tabs">
      <li class="nav-item">
        <a id="routineId" @click="routineShowFunc()" class="nav-link active"
          >Routine</a
        >
      </li>
      <li class="nav-item">
        <a id="statisticsId" @click="statiscticsShow()" class="nav-link"
          >Activity Statisctics</a
        >
      </li>
      <li class="nav-item">
        <a id="daysInfo" @click="daysInfo()" class="nav-link disabled"
          >Days Information</a
        >
      </li>
    </ul>
  </div>
  <div v-if="this.routineShow == true">
    <div>
      <h1 style="font-family: Times New Roman; margin-left: 200px">
        Patient daily routine
      </h1>
    </div>
    <div v-if="this.spinner == true">
      <div
        class="container p-3 my-3 bg-light border border-3 shadow-sm p-5 mb-5 bg-body rounded"
        style="float: right"
      >
        <div class="text-center">
          <div
            class="spinner-border text-primary"
            style="width: 4rem; height: 4rem"
            role="status"
          ></div>
          <div>
            <h3 style="font-family: Times New Roman">Please be patient</h3>
          </div>
          <div>
            <h2 style="font-family: Times New Roman">
              The routine is beeing generated.
            </h2>
          </div>
        </div>
      </div>
    </div>

    <div
      v-if="this.loaded"
      class="container p-3 my-3 bg-light border"
      style="float: right"
    >
      <GChart
        type="ColumnChart"
        :data="this.chartData"
        :options="chartOptions"
      />
      <br />
      <p>Select a day to check if deviated:</p>
      <p id="errorMessage" hidden style="color: red">
        Date field cannot be empty.
      </p>
      <input v-model="this.inputDate" type="date" />
    </div>
    <br />

    <div style="margin-left: 200px">
      <button
        id="loadData"
        @click="loadData()"
        type="button"
        class="btn btn-success"
        style="margin-right: 10px"
      >
        Load Data
      </button>
      <button
        id="deviated"
        @click="detectDeviation()"
        type="button"
        class="btn btn-warning disabled"
      >
        Check if deviated
      </button>
    </div>
  </div>

  <div v-else-if="this.activityStatsShow == true">
    <div
      class="container p-3 my-4 bg-light border"
      style="height: 125px; width: 30%"
    >
      <div class="input-group mb-2" style="">
        <span class="input-group-text" id="basic-addon1">Number of days</span>
        <input
          type="text"
          class="form-control"
          aria-label="dayNumber"
          aria-describedby="basic-addon1"
          v-model="this.daysInput"
        /><br />
      </div>
      <button @click="getStatistics()" class="btn btn-primary">
        Get statistics
      </button>
    </div>
  </div>
  <div v-else-if="this.daysInfoShow == true">
    <br />
    <br />
    <div class="row g-1">
      <div class="col-sm-6 col-md-2"></div>
      <div class="col-6 col-md-9">
        <table
          class="table table-striped table-bordered table-hover"
          id="myTable"
        >
          <thead>
            <tr>
              <th scope="col">Date</th>
              <th scope="col">Numbers of slept hours</th>
              <th scope="col">Medication status</th>
              <th scope="col">Last Questionnaire Score</th>
              <th scope="col">Deviated</th>
              <th scope="col">Deviation type</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(day, index) in this.tableRecomandation" :key="index">
              <td>{{ day.date }}</td>
              <td>{{ day.sleepHours }}</td>
              <td v-if="day.takeMedication">yes</td>
              <td v-else>no</td>
              <td>{{ day.questionnaireScore }}</td>
              <td v-if="day.isDeviated">yes</td>
              <td v-else>no</td>
              <td v-if="day.isDeviated == false">-</td>
              <td v-else>{{ day.deviation }}</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>

  <div v-if="this.renderTables" style="float: right">
    <div class="container p-3 my-4 bg-light border">
      <GChart
        type="ColumnChart"
        :data="this.chartDataNightNightActivities"
        :options="chartOptions2"
      />
    </div>
    <div class="container p-3 my-4 bg-light border">
      <GChart
        type="ColumnChart"
        :data="this.chartDataNightMorningActivities"
        :options="chartOptions2"
      />
    </div>
    <div class="container p-3 my-4 bg-light border">
      <GChart
        type="ColumnChart"
        :data="this.chartDataMorningActivities"
        :options="chartOptions2"
      />
    </div>
    <div class="container p-3 my-4 bg-light border">
      <GChart
        type="ColumnChart"
        :data="this.chartDataEveningActivities"
        :options="chartOptions2"
      />
    </div>
    <div class="container p-3 my-4 bg-light border">
      <GChart
        type="ColumnChart"
        :data="this.chartDataNoonActivities"
        :options="chartOptions2"
      />
    </div>
  </div>
</template>

<script>
import navbar from "../components/NavigationBar.vue";
import axios from "axios";
import apexchart from "vue3-apexcharts";
import { notify } from "@kyvg/vue3-notification";
export default {
  components: {
    navbar,
    apexchart,
    notify,
  },
  data() {
    return {
      spinner: false,
      userData: {},
      daysInput: "",
      loaded: false,
      inputDate: "",
      routineShow: true,
      activityStatsShow: false,
      daysInfoShow: false,
      statistics: {},
      chartsNumber: 5,
      chartDataNightNightActivities: [],
      chartDataNightMorningActivities: [],
      chartDataMorningActivities: [],
      chartDataNoonActivities: [],
      chartDataEveningActivities: [],
      renderTables: false,
      recomandation: {},
      tableRecomandation: {},
      renderTableData: {},

      chartData: [],

      chartOptions: {
        bar: { groupWidth: "1000%" },
        height: 500,
        vAxis: {
          baselineColor: "#fff",
          gridlineColor: "#fff",
          textPosition: "none",
        },
        chart: {
          title: "Score Evolution",
          subtitle: "",
          legend: { position: "top", maxLines: 3 },
          isStacked: true,
        },
      },
      chartOptions2: {
        width: 1300,
        chart: {
          title: "Activities",
          subtitle: "",
          legend: { position: "top", maxLines: 3 },
          isStacked: true,
        },
      },
    };
  },
  props: ["patientId"],
  computed: {
    currentUser() {
      return this.$store.state.auth.user;
    },
  },
  methods: {
    async loadData() {
      this.spinner = true;
      let AuthUser = this.$store.state.auth.user;

      axios({
        method: "get",
        url: "https://localhost:44333/api/RoutineDetection/" + this.patientId,
        headers: {
          Authorization: "Bearer " + AuthUser.token,
        },
        responseType: "json",
      })
        .then(
          (response) => (
            (this.userData = response.data),
            this.assign(),
            (this.loaded = true),
            document.getElementById("loadData").classList.add("disabled"),
            document.getElementById("deviated").classList.remove("disabled"),
            (this.spinner = false)
          )
        )
        .then((error) => {});
    },
    assign() {
      var act = [];
      var numbers = [];
      var dataa = [];
      var activities = this.userData.activities;
      numbers.push("Activity");
      act.push("Activity");
      activities.forEach((element, index) => {
        act.push(element.activity);

        numbers.push(30);
      });
      act.push({ role: "annotation" });
      numbers.push("");
      dataa.push(act);
      dataa.push(numbers);
      console.log(dataa);
      this.chartData = dataa;
    },
    detectDeviation() {
      if (this.inputDate == "") {
        document.getElementById("errorMessage").removeAttribute("hidden");
      } else {
        let AuthUser = this.$store.state.auth.user;
        var send = this.userData;
        axios({
          method: "post",
          url:
            "https://localhost:44333/api/RoutineDetection/detection/" +
            this.inputDate +
            "/" +
            this.patientId,
          data: send,

          headers: {
            Authorization: "Bearer " + AuthUser.token,
            "Content-Type": "application/json",
          },
          responseType: "json",
        })
          .then(
            (response) => (
              (this.recomandation = response), this.processRecomandation()
            )
          )
          .then((error) => {});
      }
    },
    routineShowFunc() {
      document.getElementById("routineId").classList.add("active");
      document.getElementById("statisticsId").classList.remove("active");
      document.getElementById("daysInfo").classList.remove("active");
      this.routineShow = true;
      this.activityStatsShow = false;
      this.renderTables = false;
      this.daysInfoShow = false;
    },
    statiscticsShow() {
      document.getElementById("statisticsId").classList.add("active");
      document.getElementById("routineId").classList.remove("active");
      document.getElementById("daysInfo").classList.remove("active");
      this.activityStatsShow = true;
      this.routineShow = false;
      this.daysInfoShow = false;
    },
    daysInfo() {
      document.getElementById("daysInfo").classList.add("active");
      document.getElementById("routineId").classList.remove("active");
      document.getElementById("statisticsId").classList.remove("active");
      this.daysInfoShow = true;
      this.routineShow = false;
      this.activityStatsShow = false;
      this.renderTables = false;

      let AuthUser = this.$store.state.auth.user;

      axios({
        method: "get",
        url:
          "https://localhost:44333/api/RoutineDetection/allRecomendation/" +
          this.patientId,
        headers: {
          Authorization: "Bearer " + AuthUser.token,
        },
        responseType: "json",
      })
        .then(
          (response) => (
            (this.tableRecomandation = response.data), this.renderDaysTable()
          )
        )
        .then((error) => {});
    },
    getStatistics() {
      let AuthUser = this.$store.state.auth.user;

      axios({
        method: "get",
        url:
          "https://localhost:44333/api/RoutineDetection/" +
          this.patientId +
          "/" +
          this.daysInput,
        headers: {
          Authorization: "Bearer " + AuthUser.token,
        },
        responseType: "json",
      })
        .then(
          (response) => (
            (this.statistics = response.data), this.processStatistics()
          )
        )
        .then((error) => {});
    },

    renderDaysTable() {
      this.tableRecomandation.forEach((element) => {
        if (element.medicationStatus == true) {
          element.medicationStatus = "yes";
          this.renderTableData = element;
        } else {
          element.medicationStatus = "no";
          this.renderTableData = element;
        }
      });
    },

    processStatistics() {
      var activitiesNightNight = [];
      var activitiesNightMorning = [];
      var activitiesMorning = [];
      var activitiesEvening = [];
      var activitiesNoon = [];

      var numbersNightNight = [];
      var numbersNightMorning = [];
      var numbersMorning = [];
      var numbersEvening = [];
      var numbersNoon = [];

      activitiesNightNight.push("Activities_Night_Night");
      activitiesNightMorning.push("Activities_Night_Morning");
      activitiesMorning.push("Activities_Morning");
      activitiesEvening.push("Activities_Evening");
      activitiesNoon.push("Activities_Noon");

      numbersNightNight.push("Activities_Night_Night");
      numbersNightMorning.push("Activities_Night_Morning");
      numbersMorning.push("Activities_Morning");
      numbersEvening.push("Activities_Evening");
      numbersNoon.push("Activities_Noon");

      this.statistics.forEach((element) => {
        if (element.partOfTheDay == "Night_Night") {
          activitiesNightNight.push(element.activityName);
          numbersNightNight.push(element.duration / 3600);
        }
        if (element.partOfTheDay == "Night_Morning") {
          activitiesNightMorning.push(element.activityName);
          numbersNightMorning.push(element.duration / 3600);
        }
        if (element.partOfTheDay == "Morning") {
          activitiesMorning.push(element.activityName);
          numbersMorning.push(element.duration / 3600);
        }
        if (element.partOfTheDay == "Evening") {
          activitiesEvening.push(element.activityName);
          numbersEvening.push(element.duration / 3600);
        }
        if (element.partOfTheDay == "Noon") {
          activitiesNoon.push(element.activityName);
          numbersNoon.push(element.duration / 3600);
        }
      });
      activitiesNightNight.push({ role: "annotation" });
      activitiesNightMorning.push({ role: "annotation" });
      activitiesMorning.push({ role: "annotation" });
      activitiesEvening.push({ role: "annotation" });
      activitiesNoon.push({ role: "annotation" });

      numbersNightNight.push("");
      numbersNightMorning.push("");
      numbersMorning.push("");
      numbersEvening.push("");
      numbersNoon.push("");

      this.chartDataNightNightActivities.push(activitiesNightNight);
      this.chartDataNightMorningActivities.push(activitiesNightMorning);
      this.chartDataMorningActivities.push(activitiesMorning);
      this.chartDataEveningActivities.push(activitiesEvening);
      this.chartDataNoonActivities.push(activitiesNoon);

      this.chartDataNightNightActivities.push(numbersNightNight);
      this.chartDataNightMorningActivities.push(numbersNightMorning);
      this.chartDataMorningActivities.push(numbersMorning);
      this.chartDataEveningActivities.push(numbersEvening);
      this.chartDataNoonActivities.push(numbersNoon);

      this.renderTables = true;
      console.log(this.chartDataNoonActivities);
    },
    processRecomandation() {
      notify({
        title: "Info",
        text: "Days information is now available",
        duration: 3000,
        closeOnClick: true,
      });
      document.getElementById("daysInfo").classList.remove("disabled");
    },
  },
};
</script>

<style>
#routine {
  font-family: "Times New Roman", Times, serif;
  font-size: large;
  text-align: left;
}
</style>
