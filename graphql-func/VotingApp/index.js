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
  teams: async () => {
    let teams = await axios.get(
      'https://graphqlvoting.azurewebsites.net/api/score'
    );
    return teams.data;
  },
  incrementPoints: async obj => {
    let response = await axios.get(
      `https://graphqlvoting.azurewebsites.net/api/score/${obj.id}`
    );

    return response.data;
  }
};

module.exports = async function(context, req) {
  const body = req.body;
  let response = await graphql(
    typeDefs,
    body.query,
    root,
    null,
    body.variables,
    body.operationName
  );
  context.res = {
    body: response
  };
};
