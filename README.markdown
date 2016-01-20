# Fitbit.NET API Client Library

## IMPORTANT: We're moving in the direction of a breaking change v2 of Fitbit.NET
See more information about this. If you're starting a new projet, you should take a closer look at using the new, almost complete library: 
https://github.com/aarondcoleman/Fitbit.NET/wiki/PREPARING-FOR-BREAKING-CHANGES---v2-Fitbit.NET-API-Library-In-Development

## V1 Info

### License: Apache License 2.0

### Features

* Makes structured calls to the Fitbit API from .NET projects
* Logs in users using the OAuth flow
* Easy data storage using POCO objects responses
* Mono compatibility
* Based on the popular RestSharp (http://restsharp.org/)

### Basic Usage

```csharp
FitbitClient client = new FitbitClient(ConsumerKey, ConsumerSecret, userProfile.FitbitAuthToken, userProfile.FitbitAuthSecret);

Activity dayActivity = client.GetDayActivity(new DateTime(2012,7,1));

```

### Project Contents

* Fitbit - Client Library
* Fitbit.Tests - NUnit tests
* Fitbit.IntegrationTests - NUnit integration tests with the API
* SampleWebMVC - A quick simple site using the client library

### Getting Started

1. Download the Project
2. Go to dev.fitbit.com and create an app account (ConsumerKey and ConsumerSecret). If you are debugging in Visual Studio, set the callback URL to localhost and your local debug port, something like localhost:12345/Fitbit/Callback
3. Open Web.config and replace the settings with the ones you obtained from Fitbit
```
<add key="FitbitConsumerKey" value="YOUR_CONSUMER_KEY_HERE" />
<add key="FitbitConsumerSecret" value="YOUR_CONSUMER_SECRET_HERE" />
```
4. Run the sample web MVC project
5. (optional) Setting up the Integration Tests (which connect to the live API)
Open the Configuration.cs file and insert an app ConsumerKey and ConsumerSecret, then follow the 3 step process listed in that app. You're trying to end up with permanent oauth credentials, doing that once in NUnit and saving it locally.

### Contributing

Lots of ways to contribute

* Help fill in the rest of the API calls. Please use the existing calls as an example. Also, at least one unit test and integration test (NUnit) are required before I'll take a pull request.
* Documentation - If you'd like to write some getting started guides, or more indepth walkthroughs, you're a hero to me.
* Suggestions for code cleanup / shrinking - Please engage in some conversation here on Github. 
* Adding example pages to the SampleWebMVC site showing what the API can do.

### Meta

This is an open source project in progress. If you're interested in getting involved and want to chat about it, please email me at aaron@smallstepslabs.com or twitter @aaronc

Additional Thanks to:
Gavin Draper - github.com/gavdraper - desktop auth
Jonathan Walz - github.com/jonathanwalz - food and weight logs
Chris Fletcher - github.com/cfletcher - sleep data
