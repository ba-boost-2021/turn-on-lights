<template>
    <div class="card">
        <ul v-if="$store.state.user.users.length > 0">
            <li v-for="u in $store.state.user.users" :class="{ 'is-selected' : isSelected(u.id) }" :key="u.id" @click="select(u.id)">{{ u.name }}</li>
        </ul>
        <div v-else>Henüz çevrimiçi kullanıcı yok</div>
    </div>
    <div class="card mt">
        <h3>Sohbet</h3>

        <input v-model="message" @keydown.enter="send" />
        <ul>
            <li v-for="m in messages">{{ m.self ? "Siz" : "O" }} - {{ m.content}}</li>
        </ul>
    </div>
</template>
<script>
import signalr from "../helpers/signalr";
export default {
    name: "Chat",
    data() {
        return {
            selected: null,
            message: null,
            messages: []
        }
    },
    mounted() {
        this.$ajax.get("game/users").then(response => {
            this.$store.commit("user/load", response.data);
        });

        signalr.on("UserLeft", id => {
            this.$store.commit("user/remove", id);
        });

        signalr.on("MessageSent", message => {
            this.messages.unshift({ content: message, self: false });
        });
    },
    methods: {
        select(id) {
            if(this.$store.state.user.currentUser === id) {
                return;
            }
            if (this.selected === id) {
                return;
            }
            this.selected = id;
            this.messages = [];
        },
        isSelected(id) {
            return this.selected === id;
        },
        send() {
            signalr.send("SendMessage", this.selected, this.message);
            this.messages.unshift({ content: this.message, self: true });
            this.message = null;
        }
    }
}
</script>
<style scoped>
ul {
    margin: 0;
    padding-inline-start: 0;
    list-style: none;
    text-align: left;
}

li {
    padding: 5px;
    margin: 5px;
    border: 1px dashed #999;
    border-radius: 3px;
}

li:hover {
    background-color: rgb(46, 179, 128);
    cursor: pointer;
}

.mt {
    margin-top: 10px;
}

.is-selected {
    background-color: #dd33dd;
}
</style>