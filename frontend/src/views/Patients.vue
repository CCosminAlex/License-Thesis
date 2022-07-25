<template>
  <head>
    <link
      href="https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css"
      rel="stylesheet"
    />
  </head>

  <navbar></navbar>
  <div>
    <docbar></docbar>
  </div>

  <div><h1>Patients</h1></div>
  <br />
  <div class="row g-0">
    <div class="col-sm-6 col-md-10">
      <button
        @click="redirect('/patientAdd')"
        class="btn btn-outline-primary btn-lg"
        style="float: right"
      >
        <i class="fa fa-user-plus"></i>
      </button>
    </div>
  </div>
  <div class="row g-1">
    <div class="col-sm-6 col-md-2"></div>
    <div class="col-6 col-md-8">
      <table
        class="table table-striped table-bordered table-hover"
        id="myTable"
      >
        <thead>
          <tr>
            <th scope="col">First Name</th>
            <th scope="col">Middle Name</th>
            <th scope="col">Last Name</th>
            <th scope="col">CNP</th>
            <th scope="col"></th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="patient in patients" :key="patient.id">
            <td>{{ patient.firstName }}</td>
            <td>{{ patient.middleName }}</td>
            <td>{{ patient.lastName }}</td>
            <td>{{ patient.cnp }}</td>
            <td>
              <button
                @click="patientInfo(patient)"
                class="btn btn-secondary"
                style="margin: 5px"
              >
                <i class="fa fa-info-circle"></i>
              </button>
              <button
                @click="questionnaires(patient)"
                class="btn btn-info"
                style="margin: 5px"
              >
                <i class="fa fa-file-text-o" aria-hidden="true"></i>
              </button>
              <button
                style="margin: 5px"
                type="button"
                class="btn btn-danger"
                @click="routine(patient)"
              >
                <i class="fa fa-line-chart" ></i>
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script>
import docbar from "../components/DoctorBar.vue";
import navbar from "../components/NavigationBar.vue";
export default {
  data() {
    return {};
  },
  components: {
    navbar,
    docbar,
  },
  computed: {
    currentUser() {
      return this.$store.state.auth.user;
    },
    patients() {
      return this.$store.getters.patients;
    },
  },
  created() {
    this.$store.dispatch("loadPatientsForADoctor", this.currentUser);
    console.log(this.currentUser);
  },
  mounted() {
    if (!this.currentUser) {
      this.$router.push("/");
    }
    if (this.$store.state.auth.user.role != "Doctor") {
      this.$router.back();
    }
  },
  methods: {
    redirect(path) {
      this.$router.push({ path: path, replace: true });
    },
    patientInfo(patient) {
      
      this.$router.push({ path: "/patientFile/" + patient.id });
    },
    questionnaires(patient){
      this.$router.push({ path: "/questionnaireInfo/" + patient.id});
    },
    routine(patient){
      this.$router.push({ path: "/routine/" + patient.id});
    },
  },
};
</script>

<style>
#myTable th {
  background-color: #ffffff;
  color: rgb(0, 0, 0);
}

#myTable tbody {
  height: 10px;
  overflow: auto;
}
#ul1 {
  list-style-type: none;
  margin: 0;
  padding: 0;
  overflow: hidden;
  background-color: rgb(255, 255, 255);
}

li {
  float: left;
}

li a {
  display: block;
  color: white;
  text-align: center;
  padding: 14px 16px;
  text-decoration: none;
}
</style>
