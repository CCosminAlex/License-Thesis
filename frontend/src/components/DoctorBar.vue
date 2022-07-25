<template>
  <div>
    <ul id="ul1">
      <li style="float: right; color: black; font-size: large">
        <a>Dr. {{ currentUser.lastName }} {{ currentUser.firstName }}</a>
      </li>
    </ul>
  </div>
</template>
<script>
export default {
  computed: {
    currentUser() {
      return this.$store.state.auth.user;
    },
    patients() {
      return this.$store.getters.patients;
    },
  },
};
</script>
