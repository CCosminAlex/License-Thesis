<template>
  <div id="nav">
   
  </div>
  <router-view/>
  
</template>

<script>
import Login from "../src/views/Login.vue"
export default {
  name: 'App',
  components: {
    Login
  },
  props: {
    msg: String
  }
}
</script>

<style lang="scss">
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}

#nav {
  padding: 30px;

  a {
    font-weight: bold;
    color: #2c3e50;

    &.router-link-exact-active {
      color: #42b983;
    }
  }
}
</style>
