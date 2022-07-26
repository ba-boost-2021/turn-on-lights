import { createStore } from "vuex";
import userStore from "./store/user";
const store = createStore({
    modules: {
        user: userStore
    },
});
export default store;