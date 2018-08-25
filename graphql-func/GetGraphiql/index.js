const fs = require('fs');
const path = require('path');

module.exports = function(context, req) {
  context.log('JavaScript HTTP trigger function processed a request.');

  var res = {
    body: '',
    headers: {
      'Content-Type': 'text/html'
    }
  };

  if (req.query.name || (req.body && req.body.name)) {
    // Just return some HTML
    res.body = '<h1>Hello ' + (req.query.name || req.body.name) + '</h1>';

    context.done(null, res);
  } else {
    // Read an HTML file in the directory and return the contents
    fs.readFile(
      path.resolve(__dirname, 'index.html'),
      'UTF-8',
      (err, htmlContent) => {
        context.res = {
          headers: {
            'Content-Type': 'text/html',
            MyCustomHeader: 'Testing'
          },
          body: htmlContent
        };
        context.done();
      }
    );
  }
};
