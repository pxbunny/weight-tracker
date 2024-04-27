param keyVaultName string
param webAppIdentityPrincipalId string
param functionAppIdentityPrincipalId string
param storageAccountName string

resource keyVault 'Microsoft.KeyVault/vaults@2023-07-01' existing = {
  name: keyVaultName
}

resource storageAccount 'Microsoft.Storage/storageAccounts@2023-01-01' existing = {
  name: storageAccountName
}

resource storageConnectionStringSecret 'Microsoft.KeyVault/vaults/secrets@2023-07-01' = {
  parent: keyVault
  name: 'storage-connection-string'
  properties: {
    value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};AccountKey=${storageAccount.listKeys().keys[0].value};EndpointSuffix=core.windows.net'
  }
}

resource accessPolicies 'Microsoft.KeyVault/vaults/accessPolicies@2023-07-01' = {
  parent: keyVault
  name: 'add'
  properties: {
    accessPolicies: [
      {
        tenantId: subscription().tenantId
        objectId: webAppIdentityPrincipalId
        permissions: {
          keys: []
          secrets: ['get']
          certificates: []
        }
      }
      {
        tenantId: subscription().tenantId
        objectId: functionAppIdentityPrincipalId
        permissions: {
          keys: []
          secrets: ['get']
          certificates: []
        }
      }
    ]
  }
}
