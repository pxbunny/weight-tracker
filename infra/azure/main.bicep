param webAppServiceName string
param functionAppServiceName string

param webAppServicePlanName string
param functionAppServicePlanName string

param webAppSkuName string = 'F1'
param functionAppSkuName string = 'Y1'

param storageAccountName string
param keyVaultName string

param notificationEmailHost string
param notificationEmailPort string
param notificationSenderEmail string
param notificationSenderName string

param location string = resourceGroup().location

var storageConnectionStringSecretName   = 'storage-connection-string'
var notificationEmailPasswordSecretName = 'notification-email-password'

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

module webApp 'modules/appService.bicep' = {
  name: 'appServiceDeployment'
  params: {
    appName: webAppServiceName
    appServicePlanName: webAppServicePlanName
    keyVaultName: keyVaultName
    storageConnectionStringSecretName: storageConnectionStringSecretName
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
    keyVaultName: keyVaultName
    notificationEmailHost: notificationEmailHost
    notificationEmailPort: notificationEmailPort
    notificationSenderEmail: notificationSenderEmail
    notificationSenderName: notificationSenderName
    storageConnectionStringSecretName: storageConnectionStringSecretName
    notificationEmailPasswordSecretName: notificationEmailPasswordSecretName
    location: location
  }
  dependsOn: [
    functionAppPlan
    storageAccount
  ]
}

module keyVault 'modules/keyVault.bicep' = {
  name: 'keyVaultDeployment'
  params: {
    keyVaultName: keyVaultName
    webAppName: webAppServiceName
    functionAppName: functionAppServiceName
    storageAccountName: storageAccountName
  }
  dependsOn: [
    webApp
    functionApp
    storageAccount
  ]
}
