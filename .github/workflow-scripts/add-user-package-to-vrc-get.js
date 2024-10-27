const { readFileSync, existsSync, writeFileSync, mkdir, mkdirSync } = require('fs')
const { homedir } = require('os')
const path = require('path')

if (process.platform !== 'linux') {
  throw new Error("This script is only for linux")
}

if (process.argv.length !== 3) {
  throw new Error("Usage: node add-user-package-to-vrc-get.js <path>")
}

let vccConfigPath = path.join(homedir(), ".local", "share", 'VRChatCreatorCompanion')
let vccConfigJsonPath = path.join(vccConfigPath, "settings.json")

console.log('vrc-get config path:', vccConfigPath)
console.log('vrc-get config json path:', vccConfigJsonPath)

if (!existsSync(vccConfigJsonPath)) {
  mkdirSync(vccConfigPath, { recursive: true })
  writeFileSync(vccConfigJsonPath, '{}')
}

const originConfigRaw = readFileSync(vccConfigJsonPath, 'utf8')
const originConfig = JSON.parse(originConfigRaw)

if (!originConfig.userPackageFolders || Array.isArray(originConfig.userPackageFolders)) {
  originConfig.userPackageFolders = []
}

originConfig.userPackageFolders.push(process.argv[2])

const newConfigRaw = JSON.stringify(originConfig)

writeFileSync(vccConfigJsonPath, newConfigRaw)

console.log('Added user package folder to vrc-get config:', process.argv[2])