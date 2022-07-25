<template>
<head>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/MaterialDesign-Webfont/3.6.95/css/materialdesignicons.css">
</head>
<div class="limit">
    <div class="login-container">
        <div class="bb-login">
             <span class="bb-form-title p-b-26"> Register </span> <span class="bb-form-title p-b-48"> <i class="mdi mdi-bandage"></i> </span>
                <div class="wrap-input100 validate-input" data-validate="Valid email is: a@b.c"> <input v-model="user.email" class="input100" type="email" name="email" required> <span class="bbb-input" data-placeholder="Email"></span> </div>
                 <div class="wrap-input100 validate-input" data-validate="Valid email is: a@b.c"> <input v-model="user.firstName" class="input100" type="text" name="first_name" required> <span class="bbb-input" data-placeholder="First Name"></span> </div>
                  <div class="wrap-input100 validate-input" data-validate="Valid email is: a@b.c"> <input v-model="user.middleName" class="input100" type="text" name="middle_name"> <span class="bbb-input" data-placeholder="Middle Name"></span> </div>
                   <div class="wrap-input100 validate-input" data-validate="Valid email is: a@b.c"> <input v-model="user.lastName" class="input100" type="text" name="last_name" required> <span class="bbb-input" data-placeholder="Last Name"></span> </div>
                    <div class="wrap-input100 validate-input" data-validate="Valid email is: a@b.c"> <input  v-model="user.phone" class="input100" type="text" name="phone" required> <span class="bbb-input" data-placeholder="Phone"></span> </div>
                <div class="wrap-input100 validate-input" data-validate="Enter password"> <span class="btn-show-pass"> <i class="mdi mdi-eye show_password"></i> </span> <input v-model="user.password" class="input100" type="password" name="pass" required> <span class="bbb-input" data-placeholder="Password"></span> </div>
                <div class="login-container-form-btn">
                    <div class="bb-login-form-btn">
                        <div class="bb-form-bgbtn"></div> <button @click="handleRegister()" class="bb-form-btn"> Register </button>
                    </div>
                </div>
                <div class="text-center p-t-115"> <span class="txt1"> Already have an account? </span> <a class="txt2" @click="redirect()"> Log in </a> </div>
        </div>
    </div>
</div>

</template>

<script>
import $ from "jquery"

  export default {
      components:{
          
      },
        data(){
    return{
      user: {
        firstName:'',
        middleName:'',
        lastName:'',
        email:'',
        password:'',
        phone: '',
      },
    }
  },
    mounted(){
        
var showPass = 0;
$('.btn-show-pass').on('click', function(){
if(showPass == 0) {
$(this).next('input').attr('type','text');
$(this).find('i').removeClass('mdi-eye');
$(this).find('i').addClass('mdi-eye-off');
showPass = 1;
}
else {
$(this).next('input').attr('type','password');
$(this).find('i').addClass('mdi-eye');
$(this).find('i').removeClass('mdi-eye-off');
showPass = 0;
}

});

    },
    methods:{
        redirect(){
            this.$router.push({path:'/', replace:true}) 
        },
      handleRegister() {
      this.message = "";
      this.successful = false;
      this.loading = true;
      console.log(this.user);
      this.$store.dispatch("auth/register", this.user).then(
        (data) => {
         
         this.$router.push({path:'/', replace:true})
        },
        (error) => {
          
        }
      );
      
    }
    }
  }
</script>

<style>
@import '../../css/mystyle.css'
</style>