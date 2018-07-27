import { makeExecutableSchema } from 'graphql-tools';
import { resolvers } from './resolvers';

const typeDefs = `
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
`;

export const schema = makeExecutableSchema({ typeDefs, resolvers });
