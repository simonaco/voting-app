# Bubble War

GraphQL endpoint: [https://graphqlplayground.azurewebsites.net/api/graphql](https://graphqlplayground.azurewebsites.net/api/graphql)

GraphiQL endpoint:
[https://graphqlplayground.azurewebsites.net/api/graphiql](https://graphqlplayground.azurewebsites.net/api/graphiql)

Sample query:

```json
query {
  teams {
    id
    name
    points
  }
}
```

Sample mutation:

```json
mutation {
  incrementPoints(id:2) {
    id
    name
    points
  }
}
```

|CI Tool                    |Build Status|
|---------------------------|---|
| App Center, iOS | [![Build status](https://build.appcenter.ms/v0.1/apps/60056d45-f42f-4bcd-870b-19c10c400c66/branches/master/badge)](https://appcenter.ms) |
| App Center, Android | [![Build status](https://build.appcenter.ms/v0.1/apps/b1cdcf1b-2685-4105-894e-9b60087dfc48/branches/master/badge)](https://appcenter.ms) |


## Install Mobile App

| Mobile OS | Installation Link |
|-----------|-------------------|
| iOS | [Install iOS App](./mobile/iOS_Download_Instructions.md) |
| Android | [Install Android App](https://install.appcenter.ms/orgs/bubble-war/apps/bubble-war/distribution_groups/sf%20graphql%20workshop) |