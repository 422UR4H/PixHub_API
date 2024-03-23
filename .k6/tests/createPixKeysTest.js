import http from "k6/http";
import { sleep } from "k6";
import { SharedArray } from "k6/data";

export const options = {
  scenarios: {
    spike_usage: {
      executor: "constant-arrival-rate",
      duration: "60s",
      preAllocatedVUs: 50,
      maxVUs: 100,
      rate: 170,
      timeUnit: "1s",
    },
  },
  thresholds: {
    "http_reqs{scenario:spike_usage}": ["count>=20000"],
  },
};

const usersData = new SharedArray("users", () => {
  const result = JSON.parse(open("../seed/existing_users.json"));
  return result;
});

const paymentProvidersData = new SharedArray("paymentProviders", () => {
  const result = JSON.parse(open("../seed/existing_paymentProviders.json"));
  return result;
});

export default function () {
  const randomUser = usersData[Math.floor(Math.random() * usersData.length)];
  const randomPaymentProvider =
    paymentProvidersData[
      Math.floor(Math.random() * paymentProvidersData.length)
    ];

  const user = {
    cpf: randomUser.CPF,
  };

  const account = {
    number: `${Date.now()}${Math.floor(Math.random() * 1000)}`,
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
    token: randomPaymentProvider.Token,
  };

  const response = http.post("http://127.0.0.1:8080/keys", body, { headers });
  if (response.status != 201) {
    console.log(response.body);
  }
  // sleep(1);
}
