# Cheatsheet

1. Rest endpoints to connect to:

   1. teams: https://graphqlvoting.azurewebsites.net/api/score
   2. incrementPoints: https://graphqlvoting.azurewebsites.net/api/score/${obj.id}

2. Deployed graphql endpoint: https://graphqlplayground.azurewebsites.net/api/graphql

3. Deployed graphiql endpoint:
   https://graphqlplayground.azurewebsites.net/api/graphiql

4. Edit local.settings.json file to include local CORS settings:

"Host": {
"CORS": "\*"
}
