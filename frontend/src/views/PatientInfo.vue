<template>
  <head>
    <link
      href="https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css"
      rel="stylesheet"
    />
  </head>

  <div style="margin-right: 1px">
    <navbar></navbar>
  </div>
  <br />
  <h2>Patient File</h2>
  <div class="row g-0"></div>
  <div class="row g-1">
    <div class="container p-1 my-3 bg-light border" style="width: 50%">
      <br />
      <form class="row g-3 needs-validation" novalidate>
        <h5 style="text-align: left">Personal Information</h5>
        <div class="col-md-4">
          <label for="validationCustom1" class="form-label">First name</label>
          <input
            v-model="patientToEdit.firstName"
            type="text"
            class="form-control"
            id="validationCustom1"
            placeholder="First Name"
            required
          />
          <div class="invalid-feedback">Please write the First Name.</div>
        </div>
        <div class="col-md-3">
          <label for="validationCustom2" class="form-label">Middle name</label>
          <input
            v-model="patientToEdit.middleName"
            type="text"
            class="form-control"
            id="validationCustom2"
            placeholder="Middle name"
            required
          />
          <div class="valid-feedback">Looks good!</div>
        </div>
        <div class="col-md-4">
          <label for="validationCustom3" class="form-label">Last name</label>
          <input
            v-model="patientToEdit.lastName"
            type="text"
            class="form-control"
            id="validationCustom3"
            placeholder="Last name"
            required
          />
          <div class="invalid-feedback">Please write the Last Name.</div>
        </div>
        <div class="col-md-3">
          <label for="validationCustom4" class="form-label"
            >Personal Identifier No.</label
          >
          <input
            v-model="patientToEdit.cnp"
            type="text"
            class="form-control"
            id="validationCustom4"
            placeholder="Personal Identifier Number"
            required
          />
          <div class="invalid-feedback">
            Please write the Personal Identifier Number().
          </div>
        </div>
        <div class="col-md-3">
          <label for="validationCustom5" class="form-label">Birth Date</label>
          <input
            v-model="patientToEdit.birthDate"
            type="text"
            class="form-control"
            id="validationCustom5"
            required
          />
          <div class="invalid-feedback">Please provide a valid date.</div>
        </div>
        <div class="col-md-4">
          <label for="validationCustom6" class="form-label">Email</label>
          <input
            v-model="patientToEdit.email"
            type="text"
            class="form-control"
            id="validationCustom6"
            placeholder="Email"
            required
          />
          <div class="valid-feedback">Looks good!</div>
        </div>
        <div class="col-md-5">
          <label for="validationCustom7" class="form-label">Address</label>
          <input
            v-model="patientToEdit.address"
            type="text"
            class="form-control"
            id="validationCustom7"
            placeholder="Address"
            required
          />
          <div class="invalid-feedback">Please provide a valid city.</div>
        </div>

        <div class="col-md-3">
          <label for="validationCustom8" class="form-label">Phone Number</label>
          <input
            v-model="patientToEdit.phoneNumber"
            type="text"
            class="form-control"
            id="validationCustom8"
            placeholder="Phone Number"
            required
          />
          <div class="invalid-feedback">
            Please provide a valid phone number.
          </div>
          <br />
        </div>
        <h5 style="text-align: left">Emergency Person</h5>
        <div class="col-md-4">
          <label for="validationCustom9" class="form-label">Full Name</label>
          <input
            v-model="patientToEdit.emergencyContact"
            type="text"
            class="form-control"
            id="validationCustom9"
            placeholder="Full Name"
            required
          />
          <div class="invalid-feedback">Please provide a name.</div>
        </div>
        <div class="col-md-3">
          <label for="validationCustom10" class="form-label"
            >Phone Number</label
          >
          <input
            v-model="patientToEdit.emergencyContactPhoneNumber"
            type="text"
            class="form-control"
            id="validationCustom10"
            placeholder="Phone Number"
            required
          />
          <div class="invalid-feedback">Please provide a phone number.</div>
          <br />
        </div>

        <div class="col-12">
          <br />
          <button
            id="save"
            class="btn btn-primary"
            type="submit"
            style="margin: 2px"
            v-if="savemode"
            @click="savePatient()"
          >
            Save Patient
          </button>
          <button
            id="edit"
            class="btn btn-success"
            type="submit"
            v-if="editmode"
            @click="editPatient()"
          >
            Edit Patient
          </button>

          <button
            id="save"
            class="btn btn-danger"
            type="submit"
            style="margin: 2px"
            @click="deletePatient()"
          >
            Delete Patient
          </button>
          <br />
        </div>
      </form>
    </div>
    <div><h1>Diseases</h1></div>
    <br />
    <div class="row g-0">
      <div class="col-sm-6 col-md-10">
        <button
          class="btn btn-primary btn"
          data-bs-toggle="modal"
          data-bs-target="#AddModal"
          style="float: right"
          @click="loadDiseases"
        >
          <i class="fa fa-plus-square"></i>
        </button>
      </div>
    </div>
  </div>
  <div class="row g-2">
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
          </tr>
        </thead>
        <tbody>
          <tr
            v-for="disease in patientToEdit.disease.slice(2)"
            :key="disease.diseaseId"
          >
            <td>{{ disease.name }}</td>
            <td>{{ disease.description }}</td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>

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
          <h5 class="modal-title" id="AddModalLabel">Add Disease To Patient</h5>
          <button
            type="button"
            class="btn-close"
            data-bs-dismiss="modal"
            aria-label="Close"
            @click="reset()"
          ></button>
        </div>
        <form class="row g-2 needs-validation" style="float:center">
          <div class="col-md-12">
            <br />
            <label for="validationCustom03" class="form-label" style="float:center"
              >Please Select A Disease</label
            >
            <select
              type="textarea"
              class="form-control"
              id="modalDisease"
              placeholder="Name"
              v-model="modalDisease"
              required
            >
              <option
                v-bind:value="{
                  diseaseId: disease.diseaseId,
                  diseaseName: disease.name,
                }"
                v-for="disease in diseases"
                :key="disease.diseaseId"
              >
                {{ disease.name }}
              </option>
            </select>
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
            <button
              id="save"
              @click="addDiseaseToPatient()"
              class="btn btn-primary"
            >
              Save changes
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
import navbar from "../components/NavigationBar.vue";
import axios from "axios";
export default {
  components: {
    navbar,
  },
  props: ["patientId"],
  data() {
    return {
      firstName: "f",
      lastName: "f",
      middleName: "f",
      cnp: "f",
      birthdate: "2017-07-04",
      email: "f",
      address: "f",
      phone: "f",
      emergencyPerson: "f",
      emergencyPersonPhoneNumber: "f",
      disease: true,
      savemode: false,
      editmode: true,
      options: ["A", "B", "C"],
      diseaseForPatient: {},
      modalStartDate: Date,
      modalEndDate: Date,
      modalDescription: "",
      modalTreatment: "",
      modalDisease: {},
      patientToEdit: {},
      patientDisease: {
        diseaseId: "",
        name: "",
        description: "",
        treatment: "",
        discovered: Date,
        ended: Date,
        patientId: "",
      },
    };
  },
  computed: {
    currentUser() {
      return this.$store.state.auth.user;
    },
    diseases() {
      return this.$store.getters.diseases;
    },
  },
  beforeCreate() {
    let AuthUser = this.$store.state.auth.user;
    axios({
      method: "get",
      url: "https://localhost:44333/api/Elder/" + this.patientId,
      headers: {
        Authorization: "Bearer " + AuthUser.token,
      },
      responseType: "json",
    })
      .then((response) => (this.patientToEdit = response.data))
      .then((error) => {});
  },
  mounted() {
    for (var i = 1; i <= 10; ++i) {
      var id = "validationCustom" + i;
      document.getElementById(id).disabled = true;
      id = "validationCustom";
    }
  },

  methods: {
    loadDiseases() {
      this.$store.dispatch("loadDiseases", this.currentUser);
    },
    deletePatient() {
      let AuthUser = this.$store.state.auth.user;
      axios
        .delete("https://localhost:44333/api/Elder/" + this.patientId, {
          headers: { Authorization: "Bearer " + AuthUser.token },
        })
        .then((response) => this.$router.push({ path: "/patients" }));
    },
    editPatient() {
      for (var i = 1; i <= 10; ++i) {
        var id = "validationCustom" + i;
        document.getElementById(id).disabled = false;
        id = "validationCustom";
      }
      this.editmode = false;
      this.savemode = true;
    },
    savePatient() {
      let AuthUser = this.$store.state.auth.user;
      var sendPatientToEdit = this.patientToEdit;
      console.log(sendPatientToEdit);
      var forms = document.querySelectorAll(".needs-validation");
      Array.prototype.slice.call(forms).forEach(function (form) {
        form.addEventListener(
          "submit",
          function (event) {
            if (!form.checkValidity()) {
              event.preventDefault();
              event.stopPropagation();
            } else {
              axios({
                method: "put",
                url: "https://localhost:44333/api/Elder",
                data: sendPatientToEdit,
                headers: {
                  Authorization: "Bearer " + AuthUser.token,
                  "Content-Type": "application/json",
                },
                responseType: "json",
              });
            }
            form.classList.add("was-validated");
          },
          false
        );
      });
    },

    changeDisease(disease) {
      console.log(disease);
    },
    reset() {
      document.getElementById("modalDescription").value = "";
      document.getElementById("modalTreatment").value = "";
      document.getElementById("modalStartDate").value = "";
      document.getElementById("modalEndDate").value = "";
    },
    addDiseaseToPatient() {
      this.patientDisease.description = this.modalDescription;
      this.patientDisease.treatment = this.modalDescription;
      this.patientDisease.name = this.modalDisease.diseaseName;
      this.patientDisease.diseaseId = this.modalDisease.diseaseId;
      this.patientDisease.discovered = this.modalStartDate;
      this.patientDisease.ended = this.modalEndDate;
      this.patientDisease.patientId = this.patientId;
      let AuthUser = this.$store.state.auth.user;

      axios
        .post(
          "https://localhost:44333/api/PatientDisease",
          this.patientDisease,
          {
            headers: {
              Authorization: "Bearer " + AuthUser.token,
            },
          }
        )
        .then((response) => window.location.reload());
    },
  },
};
</script>

<style>
/* html,body{
overflow:auto;
} */
</style>
