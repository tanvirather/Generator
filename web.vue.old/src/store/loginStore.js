export class LoginStore {
  #apiClient;
  #url = "account"

  constructor(apiClient) {
    this.#apiClient = apiClient
  }

  async login(model) {
    let result = await this.#apiClient.post(this.#url + "/login", model);
    console.log(result.token);
    return result;
  }

}
