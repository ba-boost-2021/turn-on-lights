import * as signalR from "@microsoft/signalr";
import store from "./store";
let connection = new signalR.HubConnectionBuilder()
    .withUrl(import.meta.env.VITE_API_URL + "/game")
    .build();

connection.on("UserJoined", (id, name) => {
    store.commit("user/add", { id: id, name: name });
});

connection.start();
export default connection;