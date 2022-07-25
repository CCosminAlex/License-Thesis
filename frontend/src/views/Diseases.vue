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
  <!-- Button trigger modal -->

  <div>
    <div><h1>Diseases</h1></div>
    <br />
    <div class="row g-0">
      <div class="col-sm-6 col-md-10">
        <button
          class="btn btn-primary btn"
          data-bs-toggle="modal"
          data-bs-target="#AddModal"
          style="float: right"
        >
          <i class="fa fa-plus-square"></i>
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
              <th scope="col">Name</th>
              <th scope="col">Description</th>
              <th style="width: 10%" scope="col"></th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="disease in diseases" :key="disease.diseaseId">
              <td>{{ disease.name }}</td>
              <td>{{ disease.description }}</td>
              <td>
                <button
                  type="button"
                  class="btn btn-success"
                  data-bs-toggle="modal"
                  data-bs-target="#exampleModal"
                  style="margin:5px;"
                  @click="takeInfo(disease)"
                >
                  <i class="fa fa-pencil-square-o"></i>
                </button>
                <button
                  @click="deleteDisease(disease.diseaseId)"
                  class="btn btn-danger"
                >
                  <i class="fa fa-trash"></i>
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
  <!-- Button trigger modal -->

  <!-- Modal -->
  <div
    class="modal fade"
    id="exampleModal"
    tabindex="-1"
    aria-labelledby="exampleModalLabel"
    aria-hidden="true"
  >
    <div class="modal-dialog modal-xl">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="exampleModalLabel">Edit Disease</h5>
          <button
            type="button"
            class="btn-close"
            data-bs-dismiss="modal"
            aria-label="Close"
            @click="reset()"
          ></button>
        </div>
        <form class="row g-2 needs-validation" novalidate>
          <div class="col-md-6">
            <br />
            <label for="validationCustom03" class="form-label">Name</label>
            <input
              v-model="diseaseName"
              type="textarea"
              class="form-control"
              id="validationCustom03"
              placeholder="Name"
              required
            />
            <div class="invalid-feedback">Please write the Name.</div>
          </div>
          <div class="mb-3">
            <label for="validationTextarea" class="form-label"
              >Description</label
            >
            <textarea
              v-model="diseaseDescription"
              class="form-control"
              id="validationTextarea"
              placeholder="Description"
              required
            ></textarea>
            <div class="invalid-feedback">Please enter a description.</div>
          </div>
          <div class="modal-footer">
            <button
              type="button"
              class="btn btn-secondary"
              data-bs-dismiss="modal"
              @click="reset()"
            >
              Close
            </button>
            <button id="save" @click="edit()" class="btn btn-primary">
              Save changes
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>

  <!-- Add Modal -->
   <div
    class="modal fade"
    id="AddModal"
    tabindex="-1"
    aria-labelledby="AddModalLabel"
    aria-hidden="true"
  >
    <div class="modal-dialog modal-xl">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="AddModalLabel">Add Disease</h5>
          <button
            type="button"
            class="btn-close"
            data-bs-dismiss="modal"
            aria-label="Close"
            @click="reset()"
          ></button>
        </div>
        <form class="row g-2 needs-validation" novalidate>
          <div class="col-md-6">
            <br />
            <label for="validationCustom03" class="form-label">Name</label>
            <input
              v-model="diseaseName"
              type="textarea"
              class="form-control"
              id="validationCustom03"
              placeholder="Name"
              required
            />
            <div class="invalid-feedback">Please write the Name.</div>
          </div>
          <div class="mb-3">
            <label for="validationTextarea" class="form-label"
              >Description</label
            >
            <textarea
              v-model="diseaseDescription"
              class="form-control"
              id="validationTextarea"
              placeholder="Description"
              required
            ></textarea>
            <div class="invalid-feedback">Please enter a description.</div>
          </div>
          <div class="modal-footer">
            <button
              type="button"
              class="btn btn-secondary"
              data-bs-dismiss="modal"
              @click="reset()"
            >
              Close
            </button>
            <button id="save" @click="save()" class="btn btn-primary">
              Save changes
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
import docbar from "../components/DoctorBar.vue";
import navbar from "../components/NavigationBar";
import axios from "axios";
export default {
  data() {
    return {
      diseaseToEditId: "",
      diseaseName: "",
      diseaseDescription: "",
    };
  },
  components: {
    navbar,
    docbar,
  },
  computed: {
    currentUser() {
      console.log(this.$store.state.auth.user);
      return this.$store.state.auth.user;
    },
    diseases() {
      return this.$store.getters.diseases;
    },
  },
  created() {
    this.$store.dispatch("loadDiseases", this.currentUser);
  },
  mounted() {
  
  },
  methods: {
    redirect(path) {
      this.$router.push({ path: path, replace: true });
    },
    edit() {
    
      var disease = {
        diseaseId: this.diseaseToEditId,
        name: this.diseaseName,
        description: this.diseaseDescription,
      };
      console.log(disease);
      let AuthUser = this.$store.state.auth.user;
      var forms = document.querySelectorAll(".needs-validation");
      Array.prototype.slice.call(forms).forEach(function (form) {
        form.addEventListener(
          "submit",
          function (event) {
            if (!form.checkValidity()) {
              event.preventDefault();
              event.stopPropagation();
            } else {
              axios
                .put(
                  "https://localhost:44333/api/Disease/" + disease.diseaseId,
                  disease,
                  {
                    headers: { Authorization: "Bearer " + AuthUser.token },
                  }
                )
                .then((response) => window.location.reload());
            }
            form.classList.add("was-validated");
          },
          false
        );
      });
      
    },
    reset() {
      document.getElementById("validationTextarea").value = "";
      document.getElementById("validationCustom03").value = "";
    },
    deleteDisease(diseaseId) {
      let AuthUser = this.$store.state.auth.user;
      axios
        .delete(
          "https://localhost:44333/api/Disease/" + diseaseId,
          this.deviceToEdit,
          {
            headers: { Authorization: "Bearer " + AuthUser.token },
          }
        )
        .then((response) => window.location.reload());
    },
    takeInfo(disease) {
      this.diseaseToEditId = disease.diseaseId;
      
      this.diseaseName=disease.name;
      this.diseaseDescription = disease.description;
      
    },
    save() {
      var disease = {
        name: this.diseaseName,
        description: this.diseaseDescription,
      };
      console.log(disease);
      let AuthUser = this.$store.state.auth.user;
      var forms = document.querySelectorAll(".needs-validation");
      Array.prototype.slice.call(forms).forEach(function (form) {
        form.addEventListener(
          "submit",
          function (event) {
            if (!form.checkValidity()) {
              event.preventDefault();
              event.stopPropagation();
            } else {
              axios
                .post(
                  "https://localhost:44333/api/Disease", disease,
                  {
                    headers: { Authorization: "Bearer " + AuthUser.token },
                  }
                )
                .then((response) => window.location.reload());
            }
            form.classList.add("was-validated");
          },
          false
        );
      });
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
