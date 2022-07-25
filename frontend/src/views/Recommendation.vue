<template>
  <div><navbar></navbar></div>
  <div class="container p-3 my-3 bg-light border" style="height:500px;float:right;">
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
              <th scope="col">Recommendation</th>
              
            </tr>
          </thead>
          <tbody>
            <tr v-for="(day, index) in this.reccoms.data" :key="index">
              <td>{{ day.date }}</td>
              <td>{{ day.sleepHours }}</td>
              <td v-if="day.takeMedication">yes</td><td v-else>no</td>
                <td>{{ day.text }}</td>
              
            </tr>
          </tbody>
        </table>
      </div>
    </div>

  
  </div>
  
</template>

<script>
import navbar from "../components/UserNavigationBar.vue"
import axios from 'axios'
export default {
    components:{
        navbar,
    },
  data() {
    return {
      reccoms:{},
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
  methods:{
    loadData(){
        let AuthUser = this.$store.state.auth.user;
     
        axios({
          method: "get",
          url:
            "https://localhost:44333/api/RoutineDetection/recommendation/" + AuthUser.elderId,

          headers: {
            Authorization: "Bearer " + AuthUser.token,
            "Content-Type": "application/json",
          },
          responseType: "json",
        })
          .then(
            (response) => (
              (this.reccoms = response)
            )
          )
          .then((error) => {});
    },
  }  
}

</script>
