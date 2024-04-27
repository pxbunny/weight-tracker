param keyVaultName string
param functionAppName string

resource keyVault 'Microsoft.KeyVault/vaults@2019-09-01' existing = {
  name: keyVaultName
}

resource functionApp 'Microsoft.Web/sites@2023-01-01' existing = {
  name: functionAppName
}

resource accessPolicy 'Microsoft.KeyVault/vaults/accessPolicies@2019-09-01' = {
  parent: keyVault
  name: 'add'
  properties: {
    accessPolicies: [
      {
        tenantId: subscription().tenantId
        objectId: functionApp.identity.principalId
        permissions: {
          keys: []
          secrets: ['get']
          certificates: []
        }
      }
    ]
  }
}
