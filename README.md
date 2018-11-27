# buildit-exercise
### About
This application has been implemented using ASP.NET Core 2.1.
### Running
The application has been container and the image has been uploaded to a public repo on Docker Hub.
```
docker run -it -p 5000:5000 frasdav/wipro-webcrawler
```
..and then browse to http://localhost:5000. Log data will be visible in the console.
### Building for yourself
Download the .NET Core 2.1.1 SDK for your particular operating system from https://www.microsoft.com/net/download/dotnet-core/2.1.
```
dotnet build src/Wipro.WebCrawler.Web/
dotnet run --project src/Wipro.WebCrawler.Web/
```
..and then browse to http://localhost:5000. Log data will be visible in the console.
### Reasoning
The application is containerised and stateless, and can be run on any operating system supported by .NET Core 2.1 (including Windows, Linux and MacOS). .NET Core 2.1 does not have to be installed on the host system for the container to run. ASP.NET MVC has been used to provide a web based UI for the application, with business logic abstracted away from the web/presentation layer. A limited amount of unit tests have been implemented, but the abstraction necessary to achieve a high percentage of coverage is implemented.
### Improvements
This solution would benefit from the following improvements:
  * Add error/retry handling.
  * Use parallel processing for the crawl in order to increase performance.
  * Implement a message queue and run the crawl outside the web app (in a separate application) in order to free the resources in the web app and allow for more appropriate scaling (i.e. scaling the crawler, not the presentation layer).
  * Implement the UI using a JavaScript framework so that, in tandem with the above, the UI can queue a crawl job and then check on the status of that job via a REST API implemented in the presentation layer. This would improve the user experience rather than performing full page reload as it does currently.
  * Create another implementation of IWebCrawler that honoured robots.txt.
  * Implement more tests!
