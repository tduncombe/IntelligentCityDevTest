# IntelligentCityDevTest

I chose to modify the test methods quite a lot. I wanted to use the assignment as a way to demonstrate unit testing vs integration testing. I have left an example of the dictionary method implemented in the way that I did before refactoring.

As such I've made some changes that feel a little shoehorned in, or unnecessarily complicated for such a small task.

I chose to comment the code in a different manner than I usually would in production code. I used more verbose comments than I usually would to explain my thought process and possible modifications/alternatives.

The code expects secrets.json to be placed in the output directory, I chose not to commit it for security reasons.
By default for me that is AirtableConnector.Tests/bin/Debug/net5.0/secrets.json

All tests pass using `dotnet test` as well as testing using visual studio code's test explorer.

Please let me know if something doesn't run correctly for you!