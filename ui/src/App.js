import React from 'react';
import ReactBubbleChart from 'react-bubble-chart';
import gql from 'graphql-tag';
import { Query } from 'react-apollo';

var colorLegend = [
  //reds from dark to light
  { color: '#67000d', text: 'Negative', textColor: '#ffffff' },
  '#a50f15',
  '#cb181d',
  '#ef3b2c',
  '#fb6a4a',
  '#fc9272',
  '#fcbba1',
  '#fee0d2',
  //neutral grey
  { color: '#f0f0f0', text: 'Neutral' },
  // blues from light to dark
  '#deebf7',
  '#c6dbef',
  '#9ecae1',
  '#6baed6',
  '#4292c6',
  '#2171b5',
  '#08519c',
  { color: '#08306b', text: 'Positive', textColor: '#ffffff' }
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
        colorValue: Math.floor(Math.random() * 51),
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
