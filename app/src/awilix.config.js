// awilix.config.js
const awilix = require('awilix');
const DriverService = require('path/to/DriverService');

const container = awilix.createContainer();

container.register({
  driverService: awilix.asClass(DriverService)
});

module.exports = container;