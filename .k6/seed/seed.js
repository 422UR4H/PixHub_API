const { v4: uuid } = require("uuid");
const { faker } = require("@faker-js/faker");
const generateJSON = require("./generateJSON");
const dotenv = require("dotenv");

dotenv.config();

const knex = require("knex")({
  client: "pg",
  connection: process.env.DATABASE_URL,
});

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

  const paymentProviders = generatePaymentProviders();
  await populate("PaymentProvider", paymentProviders);
  generateJSON("./seed/existing_paymentProviders.json", paymentProviders);

  const users = generateUsers();
  await populate("User", users);
  generateJSON("./seed/existing_users.json", users);

  const pixKeys = await generatePixKeys();
  await populate("PixKey", pixKeys);
  generateJSON("./seed/existing_pixKeys.json", pixKeys);

  const payments = await generatePayments();
  await populate("Payments", payments);
  generateJSON("./seed/existing_payments.json", payments);

  const accounts = await generatePaymentProviderAccounts();
  await populate("PaymentProviderAccount", accounts);
  generateJSON("./seed/existing_accounts.json", accounts);

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
      Token: faker.string.uuid(),
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

async function generatePixKeys() {
  console.log(`Generating ${PIX_KEYS} pix keys...`);
  const pixKeys = [];

  const user = await knex.select().from("User").limit(1);
  const paymentProvider = await knex.select().from("PaymentProvider").limit(1);

  const account = [
    {
      Id: 1,
      Agency: faker.finance.accountName(),
      AccountNumber: faker.finance.accountNumber(),
      CreatedAt: new Date(Date.now()).toISOString(),
      UpdatedAt: new Date(Date.now()).toISOString(),
      PaymentProviderId: paymentProvider[0].Id,
      UserId: user[0].Id,
    },
  ];

  await knex.batchInsert("PaymentProviderAccount", account);
  generateJSON("./seed/existing_bank.json", [
    { token: paymentProvider[0].Token },
  ]);

  for (let i = 0; i < PIX_KEYS; i++) {
    pixKeys.push({
      PaymentProviderAccountId: account[0].Id,
      Type: "Random",
      Value: faker.string.uuid().substring(0, 32),
      CreatedAt: new Date(Date.now()).toISOString(),
      UpdatedAt: new Date(Date.now()).toISOString(),
    });
  }
  return pixKeys;
}

async function generatePayments() {
  console.log(`Generating ${PAYMENTS} payments...`);
  const payments = [];

  const user = await knex.select().from("User").limit(1);
  const pixKey = await knex.select().from("PixKey").limit(1);
  const paymentProvider = await knex.select().from("PaymentProvider").limit(1);

  const account = [
    {
      Id: 2,
      Agency: faker.finance.accountName(),
      AccountNumber: faker.finance.accountNumber(),
      CreatedAt: new Date(Date.now()).toISOString(),
      UpdatedAt: new Date(Date.now()).toISOString(),
      PaymentProviderId: paymentProvider[0].Id,
      UserId: user[0].Id,
    },
  ];

  await knex.batchInsert("PaymentProviderAccount", account);
  generateJSON("./seed/existing_origin.json", [
    {
      cpf: user[0].CPF,
      token: paymentProvider[0].Token,
      agency: account[0].Agency,
      number: account[0].AccountNumber,
    },
  ]);

  for (let i = 0; i < PAYMENTS; i++) {
    payments.push({
      TransactionId: uuid(),
      PixKeyId: pixKey[0].Id,
      PaymentProviderAccountId: account[0].Id,
      Status: "SUCCESS",
      Amount: faker.number.int({ min: 1, max: 300000 }),
      Description: faker.lorem.sentence(),
      CreatedAt: new Date(Date.now()).toISOString(),
      UpdatedAt: new Date(Date.now()).toISOString(),
    });
  }
  return payments;
}

async function generatePaymentProviderAccounts() {
  console.log(
    `Generating ${PAYMENT_PROVIDER_ACCOUNTS} payment provider accounts...`
  );
  const accounts = [];

  const user = await knex.select().from("User").limit(1);
  const paymentProvider = await knex.select().from("PaymentProvider").limit(1);

  for (let i = 0; i < PAYMENT_PROVIDER_ACCOUNTS; i++) {
    accounts.push({
      Agency: faker.finance.accountName(),
      AccountNumber: faker.finance.accountNumber(),
      CreatedAt: new Date(Date.now()).toISOString(),
      UpdatedAt: new Date(Date.now()).toISOString(),
      PaymentProviderId: paymentProvider[0].Id,
      UserId: user[0].Id,
    });
  }
  return accounts;
}

async function populate(tableName, entities) {
  console.log(`Storing ${tableName} on DB...`);
  await knex.batchInsert(tableName, entities);
}
