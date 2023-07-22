param appServiceName string
param appServicePlanName string
param storageAccountName string

param location string = resourceGroup().location

module appService 'modules/appService.bicep' = {
  name: 'appServiceDeployment'
  params: {
    appName: appServiceName
    appServicePlanName: appServicePlanName
    location: location
  }
}

module storageAccount 'modules/storageAccount.bicep' = {
  name: 'storageAccountDeployment'
  params: {
    storageAccountName: storageAccountName
    location: location
  }
}
