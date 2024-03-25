import http from "k6/http";
import { sleep } from "k6";
import { SharedArray } from "k6/data";

export const options = {
  scenarios: {
    spike_usage: {
      executor: "constant-arrival-rate",
      duration: "60s",
      preAllocatedVUs: 50,
      maxVUs: 120,
      rate: 500,
      timeUnit: "1s",
    },
  },
  thresholds: {
    "http_reqs{scenario:spike_usage}": ["count>=10000"],
  },
};

const accountData = new SharedArray("accounts", () => {
  const result = JSON.parse(open("../seed/existing_accounts.json"));
  return result;
});

export default function () {
  const randomAccount =
    accountData[Math.floor(Math.random() * accountData.length)];

  const user = {
    cpf: randomAccount.CPF,
  };

  const randomNumber = Math.floor(Math.random() * 5);

  const number =
    randomNumber < 4
      ? randomAccount.AccountNumber
      : `${Date.now()}${Math.floor(Math.random() * 1000)}`;

  const agency =
    randomNumber < 4
      ? randomAccount.Agency
      : `${new Date(Date.now()).toISOString()}`.slice(-20);

  const account = {
    number,
    agency,
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
    token: randomAccount.Token,
  };

  const response = http.post("http://127.0.0.1:8080/keys", body, { headers });
  if (response.status != 201) {
    console.log(response.body);
  }
  // sleep(1);
}
