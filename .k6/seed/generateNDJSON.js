const fs = require("fs");
const ndjson = require("ndjson");

function generateNDJSON(filepath, data) {
  if (fs.existsSync(filepath)) {
    fs.unlinkSync(filepath);
  }
  const writer = ndjson.stringify();
  const outputStream = fs.createWriteStream(filepath);

  writer.pipe(outputStream);

  for (let i = 0; i < data.length; i++) {
    writer.write(data[i]);
  }
  writer.end();
}
module.exports = generateNDJSON;
