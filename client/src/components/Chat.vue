<template>
<div class="card">
        <ul>
            <li v-for="u in users" :key="u.id">{{ u.name }}</li>
        </ul>
    </div>
</template>
<script>
export default {
    name: "Chat",
    data() {
        return {
            users: []
        }
    },
    mounted(){
        this.$ajax.get("game/users").then(response => {
            this.users = response.data;
        })
    },
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
li:hover{
    background-color: rgb(46, 179, 128);
    cursor: pointer;
}
</style>