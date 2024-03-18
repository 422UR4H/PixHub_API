import http from "k6/http";
import { sleep } from "k6";
import { SharedArray } from "k6/data";

export const options = {
  vus: 10,
  duration: "10s",
};

const TOKEN_PROVIDER = "123token";
const MAX_CENTS_PIX_PAYMENTS = 300000;

const pixKeysData = new SharedArray("pixKeys", () => {
  const result = JSON.parse(open("../seed/existing_pixKeys.json"));
  return result;
});

const accountData = new SharedArray("accounts", () => {
  const result = JSON.parse(open("../seed/existing_accounts.json"));
  return result;
});

const userData = new SharedArray("users", () => {
  const result = JSON.parse(open("../seed/existing_users.json"));
  return result;
});

export default function () {
  const randomPixKey =
    pixKeysData[Math.floor(Math.random() * pixKeysData.length)];

  const randomAccount =
    accountData[Math.floor(Math.random() * accountData.length)];

  const userByAccount = userData.find((u) => u.Id === randomAccount.UserId);

  const user = {
    cpf: userByAccount.CPF,
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
    token: TOKEN_PROVIDER,
  };

  const response = http.post("http://localhost:8080/payments", body, {
    headers,
  });

  if (response.status != 201) {
    console.log(response.body);
  }
  sleep(1);
}
