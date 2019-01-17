This project was bootstrapped with [Create React App](https://github.com/facebookincubator/create-react-app).

Below you will find some information on how to perform common tasks.<br>
You can find the most recent version of this guide [here](https://github.com/facebookincubator/create-react-app/blob/master/packages/react-scripts/template/README.md).

React client:
1. install create-react-app 
2. create new app npx create-react-app
3. run app using npm start
4. install npm packages: npm i apollo-boost react-apollo graphql graphql-tag

Import ApolloClient from apollo-boost
Initialize client using local (deployed) function URL
Test endpoint works by running query:

```javascript
client
  .query({
    query: gql`
      {
        teams {
          id
        }
      }
    `
  })
  .then(result => console.log(result));
```

Import ApolloProvider component from react-apollo & set the client property to the client you created earlier