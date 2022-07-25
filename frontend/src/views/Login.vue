<template>
  <head>
    <link
      rel="stylesheet"
      href="https://cdnjs.cloudflare.com/ajax/libs/MaterialDesign-Webfont/3.6.95/css/materialdesignicons.css"
    />
  </head>
  <notifications :width="450" position="top right" style="font-size: 30px" />
  <div class="limit">
    <div class="login-container">
      <div class="bb-login">
        <span class="bb-form-title p-b-26"> Welcome </span>
        <span class="bb-form-title p-b-48">
          <i class="mdi mdi-bandage"></i>
        </span>
        <div
          class="wrap-input100 validate-input"
          data-validate="Valid email is: a@b.c"
        >
          <input
            v-model="user.email"
            class="input100"
            type="text"
            name="email"
            id="email"
          />
          <span class="bbb-input" data-placeholder="Email"></span>
        </div>
        <div
          class="wrap-input100 validate-input"
          data-validate="Enter password"
        >
          <span class="btn-show-pass">
            <i class="mdi mdi-eye show_password"></i>
          </span>
          <input
            v-model="user.password"
            class="input100"
            type="password"
            name="pass"
            id="password"
          />
          <span class="bbb-input" data-placeholder="Password"></span>
        </div>
        <div class="login-container-form-btn">
          <div class="bb-login-form-btn">
            <div class="bb-form-bgbtn"></div>
            <button
              class="bb-form-btn"
              @click="handleLogin(user.email, user.password)"
            >
              Login
            </button>
          </div>
        </div>
        <div class="text-center p-t-115">
          <span class="txt1"> Don’t have an account? </span>
          <a class="txt2" @click="redirect()"> Sign Up </a>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import $ from "jquery";
import { notify } from "@kyvg/vue3-notification";
export default {
  components: {
    notify,
  },
  data() {
    return {
      error: false,
      user: {
        email: "",
        password: "",
      },
    };
  },
  computed: {
    loggedIn() {
      return this.$store.state.auth.user;
    },
  },
  created() {
    if (this.role == "Doctor") {
      this.$router.push("/doctorView");
    }
    if (this.role == "User") {
      this.$router.push("/userView");
    }
    if (this.role == "Admin") {
      this.$router.push("/adminView");
    }
  },
  mounted() {
    var showPass = 0;
    $(".btn-show-pass").on("click", function () {
      if (showPass == 0) {
        $(this).next("input").attr("type", "text");
        $(this).find("i").removeClass("mdi-eye");
        $(this).find("i").addClass("mdi-eye-off");
        showPass = 1;
      } else {
        $(this).next("input").attr("type", "password");
        $(this).find("i").addClass("mdi-eye");
        $(this).find("i").removeClass("mdi-eye-off");
        showPass = 0;
      }
    });
  },
  methods: {
    redirect() {
      this.$router.push({ path: "/register", replace: true });
    },
    handleLogin(user) {
      this.loading = true;
      this.$store.dispatch("auth/login", this.user).then(
        () => {
          if (this.$store.state.auth.user.role == "Doctor") {
            this.$router.push("/patients");
          } else if (this.$store.state.auth.user.role == "User") {
            this.$router.push("/userView");
          }
          if (this.$store.state.auth.user.role == "Admin") {
            this.$router.push("/adminView");
          }
        },
        (error) => {
          this.loading = false;
          (this.message =
            (error.response &&
              error.response.data &&
              error.response.data.message) ||
            error.message ||
            error.toString()),
            this.danger();
        }
      );
    },
    danger() {
      notify({
        title: "Log in error",
        text: "Something went wrong",
        duration: 3000,
        type: "error",
      });
    },
  },
};
</script>

<style>
@import "../../css/mystyle.css";

.my-notification {
  /*...*/
  height: 30px;
}
.notification-title {
  font-size: 25px;
}

.notification-content {
  font-size: 20px;
}
</style>
