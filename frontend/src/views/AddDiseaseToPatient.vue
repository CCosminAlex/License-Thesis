<template>
  <div><navbar></navbar></div>

  <h2 style="font-family: Times New Roman">Add Patient Disease</h2>
  <div class="container p-3 my-3 bg-light border">
    <br />
    <form class="row g-2 needs-validation" novalidate>
      
      <div class="col-md-7">
        <br />
        <label for="validationDefault04" class="form-label">State</label>
        <select class="form-select" id="validationDefault04" required>
          <option selected disabled value="">Choose...</option>
          <option value="1">One</option>
          <option value="2">Two</option>
          <option value="3">Three</option>
        </select>
      </div>

      <div class="col-md-6"><br>
        <label for="validationCustom03" class="form-label">Treatment</label>
        <input
          type="textarea"
          class="form-control"
          id="validationCustom03"
          placeholder="Treatment"
          required
        />
        <div class="invalid-feedback">Please write the Treatment.</div>
      </div>

      <div class="col-md-2"><br>
        <label for="validationCustom05" class="form-label"
          >Date Discovered</label
        >
        <input
          type="date"
          class="form-control"
          id="validationCustom05"
          required
        />
        <div class="invalid-feedback">Please provide a valid date.</div>
      </div>
      <div class="col-md-2"><br>
        <label for="validationCustom05" class="form-label">Date Ended</label>
        <input type="date" class="form-control" />
        <div class="invalid-feedback">Please provide a valid date.</div>
      </div>
      <div class="col-12">
        <br />
        <button id="save" class="btn btn-primary" type="submit" @click="save()">
          Save
        </button>
      </div>
    </form>
  </div>
</template>

<script>
import navbar from "../components/NavigationBar.vue";
export default {
  components: {
    navbar,
  },
  methods: {
    save() {
      var forms = document.querySelectorAll(".needs-validation");
      Array.prototype.slice.call(forms).forEach(function (form) {
        console.log(form);
        form.addEventListener(
          "submit",
          function (event) {
            if (!form.checkValidity()) {
              this.ok = false;
              event.preventDefault();
              event.stopPropagation();
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

<style></style>
