<template>
  <div>
    <navbar></navbar>
  </div>
  <br />
  <h2>Patient File</h2>

  <div style="width:800px; margin:0 auto;" class="container p-3 my-3 bg-light border">
    <br />
    <form class="row g-3 needs-validation" novalidate>
      <h5 style="text-align: left">Personal Information</h5>
      <div class="col-md-4">
        <label for="validationCustom01" class="form-label">First name</label>
        <input
          v-model="firstName"
          type="text"
          class="form-control"
          id="validationCustom01"
          placeholder="First Name"
          required
        />
        <div class="invalid-feedback">Please write the First Name.</div>
      </div>
      <div class="col-md-3">
        <label for="validationCustom02" class="form-label">Middle name</label>
        <input
          v-model="middleName"
          type="text"
          class="form-control"
          id="validationCustom02"
          placeholder="Middle name"
          required
        />
        <div class="valid-feedback">Looks good!</div>
      </div>
      <div class="col-md-4">
        <label for="validationCustom03" class="form-label">Last name</label>
        <input
          v-model="lastName"
          type="text"
          class="form-control"
          id="validationCustom03"
          placeholder="Last name"
          required
        />
        <div class="invalid-feedback">Please write the Last Name.</div>
      </div>
      <div class="col-md-3">
        <label for="validationCustom04" class="form-label"
          >Personal Identifier</label
        >
        <input
          v-model="cnp"
          type="text"
          class="form-control"
          id="validationCustom04"
          placeholder="Personal Identifier"
          required
        />
        <div class="invalid-feedback">
          Please write the Personal Identifier Number().
        </div>
      </div>
      <div class="col-md-3">
        <label for="validationCustom05" class="form-label">Birth Date</label>
        <input
          v-model="birthdate"
          type="date"
          class="form-control"
          id="validationCustom05"
          required
        />
        <div class="invalid-feedback">Please provide a valid date.</div>
      </div>
      <div class="col-md-4">
        <label for="validationCustom06" class="form-label">Email</label>
        <input
          v-model="email"
          type="text"
          class="form-control"
          id="validationCustom06"
          placeholder="Email"
          required
        />
        <div class="valid-feedback">Looks good!</div>
      </div>
      <div class="col-md-5">
        <label for="validationCustom07" class="form-label">Address</label>
        <input
          v-model="address"
          type="text"
          class="form-control"
          id="validationCustom07"
          placeholder="Address"
          required
        />
        <div class="invalid-feedback">Please provide a valid city.</div>
      </div>

      <div class="col-md-3">
        <label for="validationCustom08" class="form-label">Phone Number</label>
        <input
          v-model="phone"
          type="text"
          class="form-control"
          id="validationCustom08"
          placeholder="Phone Number"
          required
        />
        <div class="invalid-feedback">Please provide a valid phone number.</div>
        <br />
      </div>
      <h5 style="text-align: left">Emergency Person</h5>
      <div class="col-md-4">
        <label for="validationCustom09" class="form-label">Full Name</label>
        <input
          v-model="EmergencyContact"
          type="text"
          class="form-control"
          id="validationCustom09"
          placeholder="Full Name"
          required
        />
        <div class="invalid-feedback">Please provide a name.</div>
      </div>
      <div class="col-md-3">
        <label for="validationCustom010" class="form-label">Phone Number</label>
        <input
          v-model="EmergencyContactPhoneNumber"
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
        <h5 style="text-align: left">Monitoring File</h5>
      </div>
      <div class="form-check">
        <label for="exampleFormControlFile1" class="form-label">
          Please upload the monitoring file for this patient </label
        ><br />
        <input
        @change="getFile($event)"
          type="file"
          class="form-control"
          id="exampleFormControlFile1"
          required
        />
        <br />
      </div>
      <div class="col-12">
        <button
          id="save"
          class="btn btn-primary"
          type="submit"
          @click="addPatient()"
        >
          Submit form
        </button>
        <br />
      </div>
    </form>
  </div>
</template>

<script>
import navbar from "../components/NavigationBar.vue";
import axios from "axios";
import fs from "fs";
export default {
  data() {
    return {
      firstName: "",
      lastName: "",
      middleName: "",
      cnp: "",
      birthdate: Date,
      email: "",
      address: "",
      phone: "",
      EmergencyContact: "",
      EmergencyContactPhoneNumber: "",
      byteArray: "",
      file: File,
      
    };
  },
  components: {
    navbar,
  },
  mounted() {
    
  },
  methods: {
    addPatient() {
      let AuthUser = this.$store.state.auth.user;
      var formData = new FormData()
      formData.append("MonitoringFile", this.file, this.file.name)
      formData.append("firstName", this.firstName)
      formData.append("middleName", this.middleName)
      formData.append("lastName", this.lastName)
      formData.append("email", this.email)
      formData.append("phone", this.phone)
      formData.append("birthDate", this.birthdate)
      formData.append("address", this.address)
      formData.append("cnp", this.cnp)
      formData.append("emergencyContact", this.EmergencyContact)
      formData.append("emergencyContactPhoneNumber", this.EmergencyContactPhoneNumber)
      formData.append("doctorId", AuthUser.id)
      var forms = document.querySelectorAll(".needs-validation");
      Array.prototype.slice.call(forms).forEach(function (form) {
        form.addEventListener(
          "submit",
          function (event) {
            if (!form.checkValidity()) {
              event.preventDefault();
              event.stopPropagation();
            } else {
             
            }
            form.classList.add("was-validated");
          },
          false
        );
      });
       axios
                .post(
                  "https://localhost:44333/api/Authenticate/registerElder/" +
                    AuthUser.id,
                  formData,
                  {
                    headers: {
                      Authorization: "Bearer " + AuthUser.token,
                      Accept: "*/*",
                    },
                  }
                )
                
    },
    getFile(event) {
      this.file = event.target.files[0]
    },
  },
};
// Example starter JavaScript for disabling form submissions if there are invalid fields
</script>

<style></style>
