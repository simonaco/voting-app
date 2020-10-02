[![Build Status](https://dev.azure.com/sicotin/sicotin/_apis/build/status/simonaco.voting-app?branchName=master)](https://dev.azure.com/sicotin/sicotin/_build/latest?definitionId=11%3FbranchName%3Dmaster&WT.mc_id=votingapp-github-sicotin)

# Voting App

In this workshop we'll build a Serverless GraphQL endpoint for an existing voting API.

## Prerequisites

1. A recent version of Node (8+)

1. VS Code: [download page](https://code.visualstudio.com/download/?WT.mc_id=workshop-ldnjs-sicotin)  

1. Azure Functions CLI: [documentation](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local?WT.mc_id=workshop-ldnjs-sicotin)  

1. Azure Functions Extension for VS Code: [marketplace](https://marketplace.visualstudio.com/items/?WT.mc_id=workshop-ldnjs-sicotin&itemName=ms-azuretools.vscode-azurefunctions)  

1. Azure account: [https://aka.ms/free](https://aka.ms/free)

## Steps

1. Create your own services following steps on [services readme](https://github.com/simonaco/serverless-graphql-workshop/blob/master/services/Readme.md)

1. Create GraphQL endpoint following steps on [api readme](https://github.com/simonaco/serverless-graphql-workshop/blob/master/graphql-api/Readme.md)

1. Add GraphiQL UI using steps in [readme](https://github.com/simonaco/serverless-graphql-workshop/blob/master/graphiql/Readme.md)

## Demo app

GraphQL endpoint: [https://graphqlplayground.azurewebsites.net/api/graphql](https://graphqlplayground.azurewebsites.net/api/graphql)

GraphiQL endpoint:
[https://graphqlplayground.azurewebsites.net/api/graphiql](https://graphqlplayground.azurewebsites.net/api/graphiql)

Sample query:

```
query {
  teams {
    id
    name
    points
  }
}
```

Sample mutation:

```
mutation {
  incrementPoints(id:2) {
    id
    name
    points
  }
}
```

## Install Mobile App

| Mobile OS | Installation Link |
|-----------|-------------------|
| iOS | [Install iOS App](./mobile/VotingApp/iOS_Download_Instructions.md) |
| Android | [Install Android App](https://install.appcenter.ms/orgs/voting-app/apps/bubble-war/distribution_groups/london%20graphql%20workshop) |

|CI Tool                    |Build Status|
|---------------------------|---|
| App Center, iOS | [![Build status](https://build.appcenter.ms/v0.1/apps/60056d45-f42f-4bcd-870b-19c10c400c66/branches/master/badge)](https://appcenter.ms) |
| App Center, Android | [![Build status](https://build.appcenter.ms/v0.1/apps/b1cdcf1b-2685-4105-894e-9b60087dfc48/branches/master/badge)](https://appcenter.ms) |
