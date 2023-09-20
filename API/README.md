# OZone

### Create a SendGrid account
Get the Key and configure the send grid.

### Create a openAI (chatgpt)
Get the API and Key.

Update these values in appsettings.
 
### after that run below command to set up test data using SQlite.
Before that do cleanup any local file (db) is create by SQlite earlir. you can get that file in you project folder .local => event.db in mac os.
After that follow below steps.

#This is set up commit

## Install EF commands in dotnet

dotnet tool install --global dotnet-ef

dotnet ef database update

## Add migration

dotnet ef migrations add InitialCreate

dotnet ef database update



### Create Event

http://localhost:5160/Events

#### Request json

{
"name": "First Event",
"date": "2023-09-16T06:35:25.471Z",
"mode": 0,
"modelDetails": "joining info",
"topic": "Demo topic",
"speakers": "ozone",
"details": "This is a demo topic, anything will be discussed here",
"personOfContact": "someone@tw.com",
"rules": "Everyone must come in party wear",
"deadline": "2023-09-18T06:35:25.471Z",
"community": "fun@tw.com",
"capacity": 123,
"type": 0,
"tags": "fun, devs"
}
