export default {
    namespaced: true,
    state: () => ({
      users: [],
      currentUser: null
    }),
    mutations: {
      add(state, user) {
        if (state.users.some(f => f.id === user.id)){
          return;
        }
        state.users.push(user);
      },
      login(state, id) {
        state.currentUser = id;
      },
      load(state, users) {
        state.users = users;
      },
      remove(state, id) {
        state.users = state.users.filter(f => f.id != id);
      }
    },
  };