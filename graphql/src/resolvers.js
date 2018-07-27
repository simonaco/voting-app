import { get } from 'axios';
export const resolvers = {
  Query: {
    teams: (parent, _, context, info) => {
      return get('https://graphqlvoting.azurewebsites.net/api/score').then(
        res => res.data
      );
    }
  },

  Mutation: {
    incrementPoints: (parent, args, context, info) => {
      return get(
        `https://graphqlvoting.azurewebsites.net/api/score/${args.id}`
      ).then(res => res.data);
    }
  }
};
