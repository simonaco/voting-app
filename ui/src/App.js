import React from 'react';
import ReactBubbleChart from 'react-bubble-chart';
import gql from 'graphql-tag';
import { Query } from 'react-apollo';

var colorLegend = [
  { color: '#ff0000', text: 'red' },
  { color: '#008000', text: 'green' }
];
const GET_TEAMS = gql`
  query {
    teams {
      id
      name
      points
    }
  }
`;

const App = ({}) => (
  <Query query={GET_TEAMS} pollInterval={1000}>
    {({ loading, error, data }) => {
      if (loading) return 'Loading..';
      if (error) return `Error! ${error}`;
      const points = data.teams.map(team => ({
        _id: team.id,
        value: team.points,
        colorValue: team.id * 51,
        selected: false
      }));
      return (
        <ReactBubbleChart
          className="my-cool-chart"
          colorLegend={colorLegend}
          data={points}
        />
      );
    }}
  </Query>
);

export default App;
