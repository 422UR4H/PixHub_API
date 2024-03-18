const { v4: uuid } = require("uuid");
const { faker } = require("@faker-js/faker");
const generateJSON = require("./generateJSON");
const dotenv = require("dotenv");

dotenv.config();

const knex = require("knex")({
  client: "pg",
  connection: process.env.DATABASE_URL,
});

const TOKEN_PROVIDER = "123token";
const MAX_PIX_PAYMENTS_AMOUNT = 300_000;

const USERS = 1_000_000;
const PIX_KEYS = 1_000_000;
const PAYMENTS = 1_000_000;
const PAYMENT_PROVIDERS = 1_000_000;
const PAYMENT_PROVIDER_ACCOUNTS = 1_000_000;

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
  generateJSON("./seed/existing_paymentProviders.json", paymentProviders);

  let users = generateUsers();
  users = await populate("User", users);
  generateJSON("./seed/existing_users.json", users);

  let accounts = generatePaymentProviderAccounts(paymentProviders, users);
  accounts = await populate("PaymentProviderAccount", accounts);
  generateJSON("./seed/existing_accounts.json", accounts);

  let pixKeys = await generatePixKeys(accounts);
  pixKeys = await populate("PixKey", pixKeys);
  generateJSON("./seed/existing_pixKeys.json", pixKeys);

  let payments = await generatePayments(accounts, pixKeys);
  payments = await populate("Payments", payments);
  generateJSON("./seed/existing_payments.json", payments);

  console.log("Closing DB connection...");
  await knex.destroy();

  const end = new Date();
  console.log("Done!");
  console.log(`Finished in ${(end - start) / 1000} seconds`);
}

run();

function generatePaymentProviders() {
  console.log(`Generating ${PAYMENT_PROVIDERS} payment providers...`);
  const paymentProviders = [];

  for (let i = 0; i < PAYMENT_PROVIDERS; i++) {
    paymentProviders.push({
      Token: TOKEN_PROVIDER,
      BankName: faker.company.name(),
      Webhook: "http://localhost:5039/payments/pix",
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
      CPF: faker.number.int({ min: 10000000000, max: 99999999999 }).toString(),
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

  for (let i = 0; i < PAYMENT_PROVIDER_ACCOUNTS; i++) {
    const paymentProvider =
      paymentProviders[Math.floor(Math.random() * paymentProviders.length)];
    const user = users[Math.floor(Math.random() * users.length)];

    accounts.push({
      Agency: faker.finance.accountName(),
      AccountNumber: faker.finance.accountNumber(),
      CreatedAt: new Date(Date.now()).toISOString(),
      UpdatedAt: new Date(Date.now()).toISOString(),
      PaymentProviderId: paymentProvider.Id,
      UserId: user.Id,
    });
  }
  return accounts;
}

async function generatePixKeys(accounts) {
  console.log(`Generating ${PIX_KEYS} pix keys...`);
  const pixKeys = [];

  for (let i = 0; i < PIX_KEYS; i++) {
    const account = accounts[Math.floor(Math.random() * accounts.length)];

    pixKeys.push({
      PaymentProviderAccountId: account.Id,
      Type: "Random",
      Value: faker.string.uuid().substring(0, 32),
      CreatedAt: new Date(Date.now()).toISOString(),
      UpdatedAt: new Date(Date.now()).toISOString(),
    });
  }
  return pixKeys;
}

async function generatePayments(accounts, pixKeys) {
  console.log(`Generating ${PAYMENTS} payments...`);
  const payments = [];

  for (let i = 0; i < PAYMENTS; i++) {
    const account = accounts[Math.floor(Math.random() * accounts.length)];
    const pixKey = pixKeys[Math.floor(Math.random() * pixKeys.length)];

    payments.push({
      TransactionId: uuid(),
      PixKeyId: pixKey.Id,
      PaymentProviderAccountId: account.Id,
      Status: "SUCCESS",
      Amount: faker.number.int({ min: 1, max: MAX_PIX_PAYMENTS_AMOUNT }),
      Description: faker.lorem.sentence(),
      CreatedAt: new Date(Date.now()).toISOString(),
      UpdatedAt: new Date(Date.now()).toISOString(),
    });
  }
  return payments;
}

async function populate(tableName, entities) {
  console.log(`Storing ${tableName} on DB...`);
  return knex.batchInsert(tableName, entities).returning("*");
}
