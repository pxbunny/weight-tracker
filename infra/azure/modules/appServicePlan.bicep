param appServicePlanName string
param skuName string
param location string

resource appServicePlan 'Microsoft.Web/serverfarms@2023-01-01' = {
  name: appServicePlanName
  location: location
  sku: {
    name: skuName
  }
  properties: {}
}

output appServicePlanId string = appServicePlan.id
