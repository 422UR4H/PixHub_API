const fs = require("fs");
const ndjson = require("ndjson");
const { v4: uuid } = require("uuid");
const { faker } = require("@faker-js/faker");
const generateJSON = require("./generateJSON");
const dotenv = require("dotenv");

dotenv.config();

const knex = require("knex")({
  client: "pg",
  connection: process.env.DATABASE_URL,
});

const MAX_PIX_PAYMENTS_AMOUNT = 300_000;
const AMOUNT_DRAWN = 2;

const USERS = 800_000;
const PIX_KEYS = 800_000;
const PAYMENT_PROVIDERS = 800_000;
const PAYMENT_PROVIDER_ACCOUNTS = 800_000;
const PAYMENTS = 1_000_000;

const ERASE_DATA = true;

async function run() {
  if (ERASE_DATA) {
    console.log(`Erasing DB...`);

    await knex("PaymentProvider").del();
    await knex("User").del();
    await knex("PaymentProviderAccount").del();
    await knex("PixKey").del();
    await knex("Payments").del();
  }
  const start = new Date();

  let paymentProviders = generatePaymentProviders();
  paymentProviders = await populate("PaymentProvider", paymentProviders);

  let users = generateUsers();
  users = await populate("User", users);

  let { accounts, tokens, cpfs } = generatePaymentProviderAccounts(
    paymentProviders,
    users
  );
  paymentProviders = null;
  users = null;
  accounts = await populate("PaymentProviderAccount", accounts);
  accounts.forEach((a, i) => {
    a.Token = tokens[i];
    a.CPF = cpfs[i];
  });
  tokens = null;
  cpfs = null;
  generateJSON("./seed/existing_accounts.json", accounts);

  let result = await generatePixKeys(accounts);
  let pixKeys = result.pixKeys;
  tokens = result.tokens;
  pixKeys = await populate("PixKey", pixKeys);
  pixKeys.forEach((pk, i) => (pk.Token = tokens[i]));
  tokens = null;
  generateJSON("./seed/existing_pixKeys.json", pixKeys);

  accounts = drawRandomVector(accounts, AMOUNT_DRAWN);
  pixKeys = drawRandomVector(pixKeys, AMOUNT_DRAWN);
  await generatePaymentsAndNDJSONForConcilliation(accounts, pixKeys);

  console.log("Closing DB connection...");
  await knex.destroy();

  const end = new Date();
  console.log(`Finished in ${(end - start) / 1000} seconds`);
}

run();

function generatePaymentProviders() {
  console.log(`Generating ${PAYMENT_PROVIDERS} payment providers...`);
  const paymentProviders = [];

  for (let i = 0; i < PAYMENT_PROVIDERS; i++) {
    paymentProviders.push({
      Token: uuid(),
      BankName: faker.company.name(),
      Webhook: "http://localhost:5040/payments/pix",
      CreatedAt: new Date(Date.now()).toISOString(),
      UpdatedAt: new Date(Date.now()).toISOString(),
    });
  }
  return paymentProviders;
}

function generateUsers() {
  console.log(`Generating ${USERS} users...`);
  const users = [];

  for (let i = 0; i < USERS; i++) {
    users.push({
      CPF: (10000000000 + i).toString(),
      Name: faker.person.fullName(),
      CreatedAt: new Date(Date.now()).toISOString(),
      UpdatedAt: new Date(Date.now()).toISOString(),
    });
  }
  return users;
}

function generatePaymentProviderAccounts(paymentProviders, users) {
  console.log(
    `Generating ${PAYMENT_PROVIDER_ACCOUNTS} payment provider accounts...`
  );
  const accounts = [];
  const tokens = [];
  const cpfs = [];

  for (let i = 0; i < PAYMENT_PROVIDER_ACCOUNTS; i++) {
    const paymentProvider =
      paymentProviders[Math.floor(Math.random() * paymentProviders.length)];
    const user = users[Math.floor(Math.random() * users.length)];

    accounts.push({
      Agency: faker.finance.accountName(),
      AccountNumber: (10000000000 + i).toString(),
      CreatedAt: new Date(Date.now()).toISOString(),
      UpdatedAt: new Date(Date.now()).toISOString(),
      PaymentProviderId: paymentProvider.Id,
      UserId: user.Id,
    });
    tokens.push(paymentProvider.Token);
    cpfs.push(user.CPF);
  }
  return { accounts, tokens, cpfs };
}

async function generatePixKeys(accounts) {
  console.log(`Generating ${PIX_KEYS} pix keys...`);
  const pixKeys = [];
  const tokens = [];

  for (let i = 0; i < PIX_KEYS; i++) {
    const account = accounts[Math.floor(Math.random() * accounts.length)];

    pixKeys.push({
      PaymentProviderAccountId: account.Id,
      Type: "Random",
      Value: faker.string.uuid().substring(0, 32),
      CreatedAt: new Date(Date.now()).toISOString(),
      UpdatedAt: new Date(Date.now()).toISOString(),
    });
    tokens.push(account.Token);
  }
  return { pixKeys, tokens };
}

async function generatePaymentsAndNDJSONForConcilliation(accounts, pixKeys) {
  for (let i = 0; i < AMOUNT_DRAWN; i++) {
    console.log(`Generating ${PAYMENTS} payments for concilliation...`);
    const account = accounts[i];
    const pixKey = pixKeys[i];
    const payments = [];

    if (pixKey.PaymentProviderAccountId === account.Id) {
      console.log("A rare conflict occurred!");
      console.log(`${PAYMENTS} payments less than expected will be generated.`);
      continue;
    }

    const writer = ndjson.stringify();
    const outputStream = fs.createWriteStream(
      `./seed/Token=${account.Token}.ndjson`
    );
    writer.pipe(outputStream);

    for (let i = 0; i < PAYMENTS; i++) {
      const transactionId = uuid();

      payments.push({
        TransactionId: transactionId,
        PixKeyId: pixKey.Id,
        PaymentProviderAccountId: account.Id,
        Status: "SUCCESS",
        Amount: faker.number.int({ min: 1, max: MAX_PIX_PAYMENTS_AMOUNT }),
        Description: faker.lorem.sentence(),
        CreatedAt: new Date(Date.now()).toISOString(),
        UpdatedAt: new Date(Date.now()).toISOString(),
      });

      writer.write({
        id: transactionId,
        status: "SUCCESS",
      });
    }
    await populate("Payments", payments);
    writer.end();
  }
}

async function populate(tableName, entities) {
  console.log(`Storing ${tableName} on DB...`);
  return knex.batchInsert(tableName, entities).returning("*");
}

function drawRandomVector(vector, amountDrawn) {
  const newVector = [];

  for (let i = 0; i < amountDrawn; i++) {
    newVector.push(vector[Math.floor(Math.random() * vector.length)]);
  }
  return newVector;
}
