const fs = require("fs");

function generateJSON(filepath, data) {
  if (fs.existsSync(filepath)) {
    fs.unlinkSync(filepath);
  }
  fs.writeFileSync(filepath, JSON.stringify(data));
}
module.exports = generateJSON;