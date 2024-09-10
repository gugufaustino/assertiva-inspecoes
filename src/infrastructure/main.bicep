// Parâmetros para personalizar o deploy
param location string = 'EastUS'
 
param appServicePlanName string = 'meuAppServicePlan'
param appServiceName string = 'meuAppService'

// Recurso 1: Criar App Service Plan
resource appServicePlan 'Microsoft.Web/serverfarms@2021-02-01' = {
  name: appServicePlanName
  location: location
  sku: {
    name: 'F1' // Free tier
    tier: 'Free'
  }
}

// Recurso 2: Criar App Service
resource webApp 'Microsoft.Web/sites@2021-02-01' = {
  name: appServiceName
  location: location
  properties: {
    serverFarmId: appServicePlan.id // Associar o App Service ao App Service Plan
  }
}

// Saídas do deploy (opcional)
output appServicePlanId string = appServicePlan.id
output webAppDefaultHostName string = webApp.properties.defaultHostName
