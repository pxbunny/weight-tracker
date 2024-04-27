param webAppServiceName string
param functionAppServiceName string

param webAppServicePlanName string
param functionAppServicePlanName string

param webAppSkuName string = 'F1'
param functionAppSkuName string = 'Y1'

param storageAccountName string

param location string = resourceGroup().location

module webAppPlan 'modules/appServicePlan.bicep' = {
  name: 'appServicePlanDeployment'
  params: {
    appServicePlanName: webAppServicePlanName
    skuName: webAppSkuName
    location: location
  }
}

module functionAppPlan 'modules/appServicePlan.bicep' = {
  name: 'functionAppPlanDeployment'
  params: {
    appServicePlanName: functionAppServicePlanName
    skuName: functionAppSkuName
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

module appService 'modules/appService.bicep' = {
  name: 'appServiceDeployment'
  params: {
    appName: webAppServiceName
    appServicePlanName: webAppServicePlanName
    storageAccountName: storageAccountName
    location: location
  }
  dependsOn: [
    webAppPlan
    storageAccount
  ]
}

module functionApp 'modules/functionApp.bicep' = {
  name: 'functionAppDeployment'
  params: {
    appName: functionAppServiceName
    appServicePlanName: functionAppServicePlanName
    storageAccountName: storageAccountName
    location: location
  }
  dependsOn: [
    functionAppPlan
    storageAccount
  ]
}
