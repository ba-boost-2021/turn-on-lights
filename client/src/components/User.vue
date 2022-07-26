<template>
    <div class="card" v-if="registered">
        <h4>Merhaba, {{ name }}</h4>
    </div>
    <div class="card" v-else>
        <div class="input-section">
            <label>Adınız</label>
            <input v-model="name" type="text" />
        </div>

        <button @click="register">Kayıt Ol</button>
    </div>
</template>
<script>
import signalr from "../helpers/signalr";
export default {
    name: "User",
    data() {
        return {
            id: null,
            name: null,
            registered: false
        }
    },
    mounted() {
        let session = sessionStorage.getItem("session");
        if (session) {
            this.registered = true;
            let user = JSON.parse(session);
            this.name = user.name;
            this.id = user.id;
            this.$store.commit("user/login", user.id);
            let timer = setInterval(() => {
                if (signalr.state === "Connected") {
                    signalr.send("NewUserJoined", user.id, user.name);    
                    clearInterval(timer);
                }
            }, 500);            
        }
    },
    methods: {
        register() {
            this.$ajax.post("game/register", this.name).then(response => {
                sessionStorage.setItem("session", JSON.stringify(response.data));
                this.registered = true;
                signalr.send("NewUserJoined", response.data.id, response.data.name);
            });
        }
    }
}
</script>