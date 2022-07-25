<template>
<html id="all">
<div>
<navbar></navbar>
</div>
</html>

</template>

<script>

import navbar from "../components/NavigationBar.vue"

export default {
  computed:{
    currentUser(){
      return this.$store.state.auth.user;
    },
  },
    components:{
        navbar,
    },
    data(){
        return{
          
        }
    }
}
</script>

<style>
#nav {
padding: 0%;
}
html{
    background : rgb(255, 255, 255);
}


</style>