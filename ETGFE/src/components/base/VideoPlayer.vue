<script>
import { defineComponent } from "vue";
import Plyr from "plyr";

export default defineComponent({
  name: "VideoPlayer",
  data() {
    return {
      player: null,
    };
  },
  mounted() {
    const self = this;

    let css = document.createElement("link");
    css.rel = "stylesheet";
    css.media = "all";
    css.href = "https://cdn.plyr.io/3.7.8/plyr.css";
    document.getElementsByTagName("head")[0].appendChild(css);

    const playerDiv = this.$el.querySelector(".player");
    const posterDiv = this.$el.querySelector(".poster");
    const parentDiv = this.$el.querySelector(".video-player");
    const rand = Math.floor(Math.random() * 100000000000000000);
    playerDiv.classList.add("player-" + rand);
    posterDiv.classList.add("poster-" + rand);
    self.player = new Plyr(".player-" + rand, {
      controls: ["play", "progress", "current-time", "mute", "volume", "fullscreen"],
    });

    self.player.on("playing", () => {
      parentDiv.classList.add("playing");
    });

    self.player.on("ended", () => {
      parentDiv.classList.remove("playing");
    });

    posterDiv.addEventListener("click", function (event) {
      event.preventDefault();
      self.player.play();
    });
  },
});
</script>

<template>
  <div>
    <slot></slot>
  </div>
</template>
