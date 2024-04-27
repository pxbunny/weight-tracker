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

var storageConnectionStringReference   = '@Microsoft.KeyVault(VaultName=${keyVaultName};SecretName=${storageConnectionStringSecretName})'
var notificationEmailPasswordReference = '@Microsoft.KeyVault(VaultName=${keyVaultName};SecretName=${notificationEmailPasswordSecretName})'

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
    appServicePlanId: webAppPlan.outputs.appServicePlanId
    location: location
    customAppSettings: [
      {
        name: 'AzureWebJobsStorage'
        value: storageConnectionStringReference
      }
    ]
  }
}

module functionApp 'modules/functionApp.bicep' = {
  name: 'functionAppDeployment'
  params: {
    appName: functionAppServiceName
    appServicePlanId: functionAppPlan.outputs.appServicePlanId
    location: location
    customAppSettings: [
      {
        name: 'AzureWebJobsStorage'
        value: storageConnectionStringReference
      }
      {
        name: 'Notifications:EmailHost'
        value: notificationEmailHost
      }
      {
        name: 'Notifications:EmailPort'
        value: notificationEmailPort
      }
      {
        name: 'Notifications:SenderEmail'
        value: notificationSenderEmail
      }
      {
        name: 'Notifications:SenderName'
        value: notificationSenderName
      }
      {
        name: 'Notifications:EmailPassword'
        value: notificationEmailPasswordReference
      }
    ]
  }
}

module keyVault 'modules/keyVault.bicep' = {
  name: 'keyVaultDeployment'
  params: {
    keyVaultName: keyVaultName
    webAppIdentityPrincipalId: webApp.outputs.identityPrincipalId
    functionAppIdentityPrincipalId: functionApp.outputs.identityPrincipalId
    storageAccountName: storageAccountName
  }
  dependsOn: [
    storageAccount
  ]
}
