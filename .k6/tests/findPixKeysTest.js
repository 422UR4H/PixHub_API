import http from "k6/http";
import { sleep } from "k6";
import { SharedArray } from "k6/data";

const TOKEN_PROVIDER = "123token";

export const options = {
  vus: 10,
  duration: "10s",
};

const pixKeysData = new SharedArray("pixKeys", () => {
  const result = JSON.parse(open("../seed/existing_pixKeys.json"));
  return result;
});

export default function () {
  const randomPixKey =
    pixKeysData[Math.floor(Math.random() * pixKeysData.length)];

  const headers = {
    "Content-Type": "application/json",
    token: TOKEN_PROVIDER,
  };

  const response = http.get(
    `http://localhost:8080/keys/${randomPixKey.Type}/${randomPixKey.Value}`,
    { headers }
  );

  if (response.status != 200) {
    console.log(response.body);
  }
  sleep(1);
}
