export class UserStore {
  #apiClient;
  #url = "user"

  constructor(apiClient) {
    this.#apiClient = apiClient
  }

  async get() {
    return await this.#apiClient.get(this.#url);
  }

  async save(users) {
    // console.log("Saving users:", users);
    await this.#apiClient.post(this.#url, users[1]);
  }
}
