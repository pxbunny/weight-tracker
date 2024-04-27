param appName string
param appServicePlanName string
param keyVaultName string
param notificationEmailHost string
param notificationEmailPort string
param notificationSenderEmail string
param notificationSenderName string
param storageConnectionStringSecretName string
param notificationEmailPasswordSecretName string
param location string

resource appServicePlan 'Microsoft.Web/serverfarms@2023-01-01' existing = {
  name: appServicePlanName
}

resource functionApp 'Microsoft.Web/sites@2023-01-01' = {
  name: appName
  location: location
  kind: 'functionapp'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: appServicePlan.id
    siteConfig: {
      appSettings: [
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~4'
        }
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: 'dotnet-isolated'
        }
        {
          name: 'WEBSITE_USE_PLACEHOLDER_DOTNETISOLATED'
          value: '1'
        }
        {
          name: 'AzureWebJobsStorage'
          value: '@Microsoft.KeyVault(VaultName=${keyVaultName};SecretName=${storageConnectionStringSecretName})'
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
          value: '@Microsoft.KeyVault(VaultName=${keyVaultName};SecretName=${notificationEmailPasswordSecretName})'
        }
      ]
    }
  }
}
