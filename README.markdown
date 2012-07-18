# Fitbit.NET API Client Library

### Features

* Makes structured calls to the Fitbit API from .NET projects
* Logs in users using the OAuth flow
* Easy data storage using POCO objects responses

### Basic Usage

```csharp
FitbitClient client = new FitbitClient(ConsumerKey, ConsumerSecret, userProfile.FitbitAuthToken, userProfile.FitbitAuthSecret);

Activity dayActivity = client.GetDayActivity(new DateTime(2012,7,1));

```
### Getting Started

1) Download the Project
2) Go to dev.fitbit.com and create an app account (ConsumerKey and ConsumerSecret). If you are debugging in Visual Studio, set the callback URL to localhost and your local debug port, something like localhost:12345/Fitbit/Callback
3) Open Web.config and replace the settings with the ones you obtained from Fitbit

```<add key="FitbitConsumerKey" value="YOUR_CONSUMER_KEY_HERE" />
```<add key="FitbitConsumerSecret" value="YOUR_CONSUMER_SECRET_HERE" />

4) Run the sample web MVC project

