import http from "k6/http";
import { sleep } from "k6";
import { SharedArray } from "k6/data";

const TOKEN_PROVIDER = "123token";

export const options = {
  vus: 10,
  duration: "10s",
};

const usersData = new SharedArray("users", () => {
  const result = JSON.parse(open("../seed/existing_users.json"));
  return result;
});

export default function () {
  const randomUser = usersData[Math.floor(Math.random() * usersData.length)];

  const user = {
    cpf: randomUser.CPF,
  };

  const account = {
    number: `${Date.now()}${Math.floor(Math.random() * 10)}`,
    agency: `${new Date(Date.now()).toISOString()}`.slice(-20),
  };

  const key = {
    value: `${Date.now()}${Math.floor(Math.random() * 100)}`,
    type: "Random",
  };

  const body = JSON.stringify({
    key,
    user,
    account,
  });

  const headers = {
    "Content-Type": "application/json",
    token: TOKEN_PROVIDER,
  };

  const response = http.post("http://localhost:8080/keys", body, { headers });
  if (response.status != 201) {
    console.log(response.body);
  }
  sleep(1);
}
