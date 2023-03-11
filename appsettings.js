// appsettings.js
const config = {
  "Logging": {
    "IncludeScopes": false,
    "LogLevel": {
      "Default": "Warning"
    }
  },
  "ApiSettings": {
    "IdentityApiUrl": `http://${process.env.SERVICE_API_IDENTITY}`,
    "JobsApiUrl": `http://${process.env.SERVICE_API_JOBS}`
  }
};

// write the generated JSON to a file, or use it directly in your code
const fs = require('fs');
fs.writeFileSync('appsettings.json', JSON.stringify(config));
