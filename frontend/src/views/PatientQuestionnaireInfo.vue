<template>
  <head>
    <link
      href="https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css"
      rel="stylesheet"
    />
  </head>
  <div><navbar></navbar></div>
  <div><docbar></docbar></div>

<div style="float:right;width:86%" >
  <div class="container p-3 my-3 bg-light border">
 <table
        class="table table-striped table-bordered table-hover"
        id="myTable"
      >
        <thead>
          <tr>
            <th>Questionnaire</th>
            <th>Score</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(quest, index) in this.questionnairesFromDb.slice(this.lowerBound,this.upperBound)" :key="index">
           
            <td>Questionnaire {{this.lowerBound +index +1  }}</td>
            <td>{{ quest.totalScore }}</td>
            <td><button
        class="btn btn-success btn"
        data-bs-toggle="modal"
        data-bs-target="#exampleModal"
        @click="getDataForModal(quest)"
        
      >
        <i class="fa fa-eye" aria-hidden="true"></i>
      </button></td>
          </tr>
        </tbody>
      </table>
      <nav aria-label="Page navigation example">
  <ul class="pagination disabled">
    <li id="prevId" class="page-item disabled"><a class="page-link" @click="previousPage()" >Previous</a></li>
    <li id="nextId" class="page-item"><a class="page-link" @click="nextPage()" >Next</a></li>
  </ul>
</nav>

  </div>
  <div class="container p-3 my-3 bg-light border">
     <GChart type="LineChart" :data="chartData" :options="chartOptions" />
  </div>
</div>

  <div >

<!-- Modal -->
<div class="modal fade" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="exampleModalLabel">Questions</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
      </div>
      <div class="modal-body">
        <div
      v-for="(s, index) in this.modalData.details"
      :key="index"
      style="font-family: Times New Roman; text-align: left; font-size: 30px"
    >
       Question: {{ s.questionStatment }}<br />Answer: {{ s.answer }}<br />
    </div>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
      </div>
    </div>
  </div>
</div>
  </div>
</template>

<script>
import axios from "axios";
import navbar from "../components/NavigationBar.vue";
import docbar from "../components/DoctorBar.vue";
import { GChart } from "vue3-googl-chart";
export default {
  components: {
    navbar,
    docbar,
    GChart,
  },
  props: ["patientId"],
  data() {
    return {
      lowerBound:0,
      upperBound:5,
      chartData: [],
      modalData:{},
      modal :false,
      chartOptions: {
        width:1300,
        chart: {
          title: "Score Evolution",
          subtitle: "",
        },
      },
      questions: {},
      show: {},
      questionnairesFromDb: {},
    };
  },
  computed: {
    currentUser() {
      return this.$store.state.auth.user;
    },
  },
  beforeCreate() {
    let AuthUser = this.$store.state.auth.user;
    axios({
      method: "get",
      url: "https://localhost:44333/api/Questionnaire/"+this.patientId,
      headers: {
        Authorization: "Bearer " + AuthUser.token,
      },
      responseType: "json",
    })
      .then((response) => this.computeChart(response.data))
      .then((error) => {});
  },
  methods: {
    navbarFunc(index) {
      console.log(index);
      this.show = this.questionnairesFromDb[index]["details"];
      console.log(this.show);
      for (var i = 0; i < Object.keys(this.questionnairesFromDb).length; ++i) {
        document.getElementById("navbar" + i).classList.remove("active");
      }
      document.getElementById("navbar" + index).classList.add("active");
    },
    computeChart(data) {
      var scores = [];
     
      this.questionnairesFromDb = data;

      this.questionnairesFromDb.forEach((element) => {
        
        scores.push([element.dateTaken, element.totalScore]);
      });
      
      this.chartData.push(["QuestionnaireId", "Score"]);
      scores.forEach((element) => {
        this.chartData.push(element);
      });
      
    },
    getDataForModal(quest){
      console.log(quest)
        this.questionnairesFromDb.forEach(element => {
          if(element.questionnaireId == quest.questionnaireId)
          this.modalData = element
        });
    },
    previousPage(){
      
      if(this.lowerBound - 5  == 0){
        this.lowerBound-=5
        this.upperBound -=5;
        document.getElementById("prevId").classList.add("disabled");
        
      } else{
        this.lowerBound -= 5;
        this.upperBound -=5;
        document.getElementById("prevId").classList.remove("disabled");
        document.getElementById("nextId").classList.remove("disabled");
      }
    },

    nextPage(){
      var objLength = 0;
      this.questionnairesFromDb.forEach(element => {
        objLength++;
      });
      this.upperBound+=5;
      this.lowerBound+=5;
      document.getElementById("prevId").classList.remove("disabled");
      if(this.upperBound >= objLength){
        document.getElementById("nextId").classList.add("disabled");
      } else{
         document.getElementById("nextId").classList.remove("disabled");
      }
    },
  },
};
</script>

<style></style>
