const dotenv = require("dotenv");
const { faker } = require("@faker-js/faker");
const generateJSON = require("./generateJSON");

dotenv.config();

const knex = require("knex")({
  client: "pg",
  connection: process.env.DATABASE_URL,
});

const USERS = 700_000;
const PAYMENT_PROVIDERS = 700_000;
const PAYMENT_PROVIDER_ACCOUNTS = 700_000;
const PIX_KEYS = 700_000;

const ERASE_DATA = true;

async function run() {
  if (ERASE_DATA) {
    await knex("PaymentProvider").del();
    await knex("User").del();
    await knex("PaymentProviderAccount").del();
    await knex("PixKey").del();
  }
  const start = new Date();

  const paymentProviders = generatePaymentProviders();
  await populatePaymentProviders(paymentProviders);
  generateJSON("./seed/existing_paymentProviders.json", paymentProviders);

  const users = generateUsers();
  await populateUsers(users);
  generateJSON("./seed/existing_users.json", users);

  const pixKeys = await generatePixKeys();
  await populatePixKeys(pixKeys);
  generateJSON("./seed/existing_pixKeys.json", pixKeys);

  const accounts = await generatePaymentProviderAccounts();
  await populatePaymentProviderAccounts(accounts);
  generateJSON("./seed/existing_accounts.json", accounts);

  console.log("Closing DB connection...");
  await knex.destroy();

  // generateRandomData();

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
  generateJSON("./seed/existing_bank.json", [{ token: paymentProvider[0].Token }]);

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

async function populatePaymentProviders(paymentProviders) {
  console.log("Storing payment providers on DB...");
  const tableName = "PaymentProvider";

  await knex.batchInsert(tableName, paymentProviders);
}

async function populateUsers(users) {
  console.log("Storing users on DB...");
  const tableName = "User";

  await knex.batchInsert(tableName, users);
}

async function populatePaymentProviderAccounts(accounts) {
  console.log("Storing payment provider accounts on DB...");
  const tableName = "PaymentProviderAccount";

  await knex.batchInsert(tableName, accounts);
}

async function populatePixKeys(pixKeys) {
  console.log("Storing pix keys on DB...");
  const tableName = "PixKey";

  await knex.batchInsert(tableName, pixKeys);
}
