# OZone

#This is set up commit

## Install EF commands in dotnet
dotnet tool install --global dotnet-ef

dotnet ef database update

## Add migration
dotnet ef migrations add InitialCreate

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
    "personOfContact": "kishan.vaishnav@thoughtworks.com",
    "rules": "Everyone must come in party wear",
    "deadline": "2023-09-18T06:35:25.471Z",
    "community": "fun@thoguhtworks.com",
    "capacity": 123,
    "type": 0,
    "tags": "fun, devs"
}