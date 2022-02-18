<template>
  <div id="app">
    <img src="./assets/logo.png" />
    <h1>Hello from docker front</h1>
    <p>
      Getting data from API [request counter
      <span class="counter">{{ currentTime }}</span
      >]:
    </p>
    <div class="data">
      Server time: {{ backdata }}
      <transition name="bounce">
        <p v-if="!isCached" class="fire">updated</p>
      </transition>
    </div>
  </div>
</template>

<script>
export default {
  name: "app",
  data: function () {
    return {
      isCached: true,
      backdata: "-",
      currentTime: 0,
      timer: null,
    };
  },
  destroyed() {
    this.stopTimer();
  },
  mounted() {
    console.log(process.env);
    this.startTimer();
  },
  methods: {
    doFetch() {
      fetch(process.env.VUE_APP_ROOT_API + "ApiSample")
        .then((response) => {
          response.text().then((text) => {
            this.isCached = this.backdata === text;
            this.backdata = text;
          });
        })
        .catch(function (err) {
          console.log("Fetch Error :-S", err);
        });
    },
    startTimer() {
      this.timer = setInterval(() => {
        if (this.currentTime < 100) {
          this.doFetch();
          this.currentTime++;
        } else this.stopTimer();
      }, 1000);
    },
    stopTimer() {
      clearTimeout(this.timer);
    },
  },
};
</script>

<style>
#app {
  font-family: "Avenir", Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}

h1,
h2 {
  font-weight: normal;
}

ul {
  list-style-type: none;
  padding: 0;
}

li {
  display: inline-block;
  margin: 0 10px;
}

a {
  color: #42b983;
}

.counter {
  background-color: chartreuse;
  color: black;
  border-radius: 50%;
  width: auto;
  height: auto;
  padding: 5px;
}

.data {
  color: firebrick;
  font-family: cursive;
}

.fire {
  color: green;
  font-size: 20px;
  font-weight: bold;
}
.bounce-enter-active {
  animation: bounce-in 0.5s;
}
.bounce-leave-active {
  animation: bounce-in 0.5s reverse;
}
@keyframes bounce-in {
  0% {
    transform: scale(0);
  }
  50% {
    transform: scale(1.5);
  }
  100% {
    transform: scale(1);
  }
}
</style>
