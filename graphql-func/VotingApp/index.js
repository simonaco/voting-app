const { graphql, buildSchema } = require('graphql');
const axios = require('axios');
const typeDefs = buildSchema(`
    type Team {
        id: ID
        name: String
        points: Int
    }
    type Query {
        teams: [Team]
    }
    type Mutation {
        incrementPoints(id: ID!): Team
    }
`);

const root = {
  teams: (obj, args, context) => {
    return axios
      .get('https://graphqlvoting.azurewebsites.net/api/score')
      .then(res => res.data);
  },
  incrementPoints: (obj, args, context) => {
    return axios
      .get(`https://graphqlvoting.azurewebsites.net/api/score/${obj.id}`)
      .then(res => res.data);
  }
};

module.exports = function(context, req) {
  context.log(`GraphQL request: ${req.body}`);

  graphql(typeDefs, req.body, root).then(response => {
    context.res = {
      body: response
    };
    context.done();
  });
};
