#!/bin/bash

# Verificar se o usuário está autenticado
# if ! az account show > /dev/null 2>&1; then
#   echo "Não está autenticado. Faça login usando 'az login'."
#   exit 1
# fi


# Parâmetros
RESOURCE_GROUP="rg-diff-brazil-dev"
LOCATION="brazilsouth"
APP_SERVICE_PLAN_NAME="diffServicePlan"

# 1. Criar grupo de recursos (se ainda não existir)
 az group create --name $RESOURCE_GROUP --location $LOCATION

az appservice plan create \
  --name $APP_SERVICE_PLAN_NAME \
  --resource-group $RESOURCE_GROUP \
  --location $LOCATION \
  --sku F1



# 2. Fazer deploy da infraestrutura definida no Bicep
# az deployment group create \
#   --resource-group meuResourceGroup \
#   --template-file ./main.bicep \
#   --parameters appServicePlanName='meuAppServicePlan' appServiceName='meuAppService'

#diff-desenv 