import http from "k6/http";

export const options = {
  vus: 100, // virtual users (clients)
  duration: "10s",
};

export default function () {
  // endpoint
  http.get("http://localhost:8080/Health");
}
