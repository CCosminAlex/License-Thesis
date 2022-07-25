<template>
  <div>
    <navbar></navbar>
  </div>

  <div v-if="loaded == 0" class="container p-3 my-3 bg-light border" style="float:right">
    <h1>Timeline</h1>
    <div id="chart">
      <apexchart
        type="rangeBar"
        height="900"
        :options="chartOptions"
        :series="JSON.parse(JSON.stringify(series))"
        @dataPointSelection="dataPointSelectionHandler"
      ></apexchart>
    </div>
    
  </div>

  <div v-else-if="loaded == 1" style="margin-top: 150px">
    <h1 style="font-family: Times New Roman">
      Please be patient data is loading
    </h1>
    <div
      class="spinner-border text-secondary"
      style="width: 3rem; height: 3rem; margin-top: 50px"
      role="status"
    >
      <span class="visually-hidden">Loading...</span>
    </div>
  </div>
  <div v-if="loaded == 2" class="container p-3 my-3 bg-light border" style="float:right">
    <h1 style="font-family: Times New Roman">
      Please choose the interval for the timeline
    </h1>
    <div>
      <br />
      <label
        id="intervalStart"
        style="font-weight: bold; font-family: Times New Roman"
        for=""
        >Please choose the start for the interval<br /><input
          type="date"
          v-model="intervalStart"
          required /></label
      ><br /><br />
      <label
        id="intervalEnd"
        style="font-weight: bold; font-family: Times New Roman"
        for=""
        >Please choose the end for the interval<br /><input
          type="date"
          v-model="intervalEnd"
          required /></label
      ><br /><br />
      <button type="button" class="btn btn-primary" @click="loadData()">
        Load Data
      </button>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import apexchart from "vue3-apexcharts";
import navbar from "../components/UserNavigationBar.vue";
import { GChart } from "vue3-googl-chart";
export default {
  components: {
    navbar,
    apexchart,
    GChart,
  },
  data() {
    return {
      index: 0,
      chartData: [],
      googleChartOptions: {
        chart: {
          title: "Activity time",
          subtitle: "",
        },
      },
      userData: [],
      loaded: 2,
      intervalStart: null,
      intervalEnd: null,
      series: [
        {
          data: [],
        },
      ],
      chartOptions: {

        chart: {
          selection: {
    enabled: true,
    type: 'x',
    fill: {
      color: '#24292e',
      opacity: 0.1
    },
          },
          height: 350,
          type: "rangeBar",
        },
        plotOptions: {
          bar: {
            horizontal: true,
          },
        },
        xaxis: {
          type: "datetime",
        },
      },
    };
  },
  computed: {
    currentUser() {
      return this.$store.state.auth.user;
    },
  },
  created() {},
  beforeMount() {},

  methods: {
    assign() {
      var index = 0;
      var myseries = [];
      var activities = this.userData;
      activities.forEach((element) => {
        var dataa = {
          x: "",
          y: [],
        };
        dataa.x = element.activityName;
        dataa.y = [
          new Date(element.startDate).getTime(),
          new Date(element.endDate).getTime(),
        ];
        myseries.push(dataa);
        index++;
      });

      this.series[0].data = myseries;
      console.log(this.series[0]);
    },
    loadData() {
      if (this.intervalEnd == null && this.intervalStart == null) {
        document.getElementById("intervalEnd").classList.add("myRequired");
        document.getElementById("intervalStart").classList.add("myRequired");
        return;
      }
      if (this.intervalStart == null) {
        document.getElementById("intervalStart").classList.add("myRequired");
        return;
      }
      if (this.intervalEnd == null) {
        document.getElementById("intervalEnd").classList.add("myRequired");
        return;
      }
      var data = {
        elderId: this.currentUser.elderId,
        intervalStart: this.intervalStart,
        intervalEnd: this.intervalEnd,
      };
      this.loaded = 1;
      console.log(this.intervalStart, this.intervalEnd);
      axios({
        method: "post",
        url: "https://localhost:44333/MonitoringData",
        data,
        headers: {
          Authorization: "Bearer " + this.currentUser.token,
        },
        responseType: "json",
      })
        .then(
          (response) => (
            (this.userData = response.data),
            (this.loaded = 0),
            this.assign(),
            this.graph()
          )
        )
        .then((error) => {});
    },
    graph(){
      console.log("aici")
      console.log(Math.ceil(Math.abs(new Date(this.intervalEnd)- new Date(this.intervalStart))/ (1000*60*60*24)))
      var toPush = [];
      this.userData.forEach(element => {
        toPush=[]
      });
      this.userData.forEach(element => {
        this.chartData.push(["Date", "Duration"])

        this.googleChartOptions.push([element.activityName, ])
      });



    },
    dataPointSelectionHandler(e, chartContext, config) {

            console.log(this.series[0].data[config.dataPointIndex].x);
            console.log(config.dataPointIndex); 
  }

  },
};
</script>

<style>
.myRequired {
  color: red;
}
</style>
