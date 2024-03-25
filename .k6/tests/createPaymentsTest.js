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
      rate: 42,
      timeUnit: "1s",
    },
  },
  thresholds: {
    "http_reqs{scenario:spike_usage}": ["count>=2500"],
  },
};

const MAX_CENTS_PIX_PAYMENTS = 300000;

const pixKeysData = new SharedArray("pixKeys", () => {
  const result = JSON.parse(open("../seed/existing_pixKeys.json"));
  return result;
});

const accountData = new SharedArray("accounts", () => {
  const result = JSON.parse(open("../seed/existing_accounts.json"));
  return result;
});

export default function () {
  const randomPixKey =
    pixKeysData[Math.floor(Math.random() * pixKeysData.length)];

  const randomAccount =
    accountData[Math.floor(Math.random() * accountData.length)];

  const user = {
    cpf: randomAccount.CPF,
  };
  const account = {
    number: randomAccount.AccountNumber,
    agency: randomAccount.Agency,
  };
  const origin = { user, account };

  const key = {
    value: randomPixKey.Value,
    type: randomPixKey.Type,
  };
  const destiny = { key };

  const body = JSON.stringify({
    origin,
    destiny,
    amount: Math.floor(Math.random() * MAX_CENTS_PIX_PAYMENTS),
    description: `${new Date(Date.now()).toISOString()}`.slice(-20),
  });

  const headers = {
    "Content-Type": "application/json",
    token: randomAccount.Token,
  };

  const response = http.post("http://127.0.0.1:8080/payments", body, {
    headers,
  });

  if (response.status != 200) {
    console.log(response.body);
  }
  // sleep(1);
}
