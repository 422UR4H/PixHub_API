import http from "k6/http";
import { sleep } from "k6";
import { SharedArray } from "k6/data";

export const options = {
  vus: 10,
  duration: "10s",
};

const MAX_CENTS_PIX_PAYMENTS = 300000;

const pixKeysData = new SharedArray("pixKeys", () => {
  const result = JSON.parse(open("../seed/existing_pixKeys.json"));
  return result;
});

const originData = new SharedArray("origin", () => {
  const result = JSON.parse(open("../seed/existing_origin.json"));
  return result;
});

export default function () {
  const randomPixKey =
    pixKeysData[Math.floor(Math.random() * pixKeysData.length)];

  const user = {
    cpf: originData[0].cpf,
  };
  const account = {
    number: originData[0].number,
    agency: originData[0].agency,
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
    token: originData[0].token,
  };

  const response = http.post("http://localhost:8080/payments", body, {
    headers,
  });

  if (response.status != 201) {
    console.log(response.body);
  }
  sleep(1);
}
