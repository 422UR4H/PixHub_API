# PixHub_API

A REST API built in C# with ASP.NET for provide a web service to creates/processes Pix for PSPs (Payment Service Providers).

## Description

The application is responsible for simulating part of the processing of the logic of the PIX mechanism within the **Central Bank** (BC), being used by different financial institutions to process the creation of PIX keys, consultations, payments and the like (for natural persons). In this context, these institutions are called **Payment Service Providers** (PSP).

This Web Service contains a Dockerized environment to run a PostgreSQL database with Grafana K6 for stress testing, RabbitMQ as Message Broker to manage queues, Prometheus and Grafana for monitoring the system and the application itself, providing an integrated and isolated environment for observability and tests.

<br />

## Quick start

Clone the repository:

`git clone https://github.com/422UR4H/PixHub_API`

Enter the folder and run the Docker environment to generate de app image:

```bash
cd PixHub_API/
cd Monitoring/
docker compose up
```

## Usage

### How it works?

Owns the entities: `PaymentProvider`, `User`, `PixKey`, `PaymentProviderAccount` and `Payments`.

The characteristics of these entities are in `Models/`.

And the DTOs of the entities are in `Dtos/`.

### Routes:

Use Swagger to access route documentation and dynamically interact with the application!

Access the link in the browser: `http://localhost:8080/swagger/index.html`.

## Technologies used

For this project, I used:

- C#;
- ASP.NET Core (version 8.0.2);
- Entity Framework Core (ORM) (version 8.0.2);
- PostgreSQL;
- RabbitMQ;
- Prometheus;
- Grafana;
- Grafana K6 with Node.js interface;
- Docker;
- Swagger;

<br />

## Stress Tests

Create a .env file in `.k6/` folder following the .env.example file and insert the test database url to run the automated tests.

Note: If you are running Postgres with Docker, you can simply copy the contents of `.env.example` to a .env, or simply delete the ".example", leaving just the .env in the file name.

In the `.k6/` folder, run: `npm run seed`.

Be careful! The file is configured to create many records. your machine may crash!
If you want to decrease to apply the test, manually change the following values in the seed.js file:

```aspx-csharp
const USERS = 1_000_000;
const PIX_KEYS = 1_000_000;
const PAYMENTS = 1_000_000;
const PAYMENT_PROVIDERS = 1_000_000;
const PAYMENT_PROVIDER_ACCOUNTS = 1_000_000;
```

When you have adjusted it as you want, go to the `tests/` folder and run:

- `k6 run healthTest.js` to stress the GET /Health route
- `k6 run createPixKeysTest.js` to stress the POST /keys route
- `k6 run findPixKeysTest.js` to stress the GET /keys route
- `k6 run createPaymentsTest.js` to stress the POST /payments route

The metrics can be seen at `http://localhost:3000/` in your browser.

The login and password can be set by yourself. Feel free to explore Grafana and its dashboards!

The RabbitMQ Management can be seen at `http://localhost:15672/` in your browser.