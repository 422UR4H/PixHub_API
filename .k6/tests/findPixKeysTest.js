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
      rate: 667,
      timeUnit: "1s",
    },
  },
  thresholds: {
    "http_reqs{scenario:spike_usage}": ["count>=20000"],
  },
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
    token: randomPixKey.Token,
  };

  const response = http.get(
    `http://127.0.0.1:8080/keys/${randomPixKey.Type}/${randomPixKey.Value}`,
    { headers }
  );

  if (response.status != 200) {
    console.log(response.body);
  }
  // sleep(1);
}
