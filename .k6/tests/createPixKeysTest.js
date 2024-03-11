import http from "k6/http";
import { sleep } from "k6";
import { SharedArray } from "k6/data";

export const options = {
  vus: 10,
  duration: "10s",
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
    number: `${new Date(Date.now()).toISOString()}`.slice(-20),
    agency: `${new Date(Date.now()).toISOString()}`.slice(-20),
  };

  const key = {
    value: `${new Date(Date.now()).toISOString()}`.slice(-20),
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

  const response = http.post("http://localhost:5000/keys", body, { headers });
  if (response.status != 201) {
    console.log(response.body);
  }
  sleep(1);
}
