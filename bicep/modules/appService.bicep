param appName string
param appServicePlanName string
param location string

resource appServicePlan 'Microsoft.Web/serverfarms@2022-09-01' = {
  name: appServicePlanName
  location: location
  tags: {}
  properties: {}
  sku: {
    name: 'F1'
  }
}

resource appweighttrackerd 'Microsoft.Web/sites@2022-09-01' = {
  name: appName
  location: location
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
  }
}
