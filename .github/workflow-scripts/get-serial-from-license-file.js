const license = require('fs').readFileSync(0, 'utf-8');

const startKey = `<DeveloperData Value="`;
const endKey = `"/>`;
const startIndex = license.indexOf(startKey) + startKey.length;
if (startIndex < 0) {
  throw new Error(`License File was corrupted, unable to locate serial`);
}
const endIndex = license.indexOf(endKey, startIndex);

const serial = Buffer.from(license.slice(startIndex, endIndex), 'base64').toString('binary').slice(4)
process.stdout.write(serial);