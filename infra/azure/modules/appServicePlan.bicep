param appServicePlanName string
param skuName string
param location string

resource appServicePlan 'Microsoft.Web/serverfarms@2022-09-01' = {
  name: appServicePlanName
  location: location
  sku: {
    name: skuName
  }
  properties: {}
}
