# microservices-platformservice

Les Jackson - .NET Microservices course (https://youtu.be/DgVjEo3OGBI)

### Platform Service
This service is a .NET 6 Web API which implements GET and POST operations for the platform resource. This service uses a SQL Server database.
The POST operation, apart from adding the new resource to the database, also publishes a message to the message queue to be processed by the Command service.

### Kubernetes architecture

![Kubernetes architecture](https://user-images.githubusercontent.com/62215591/188326182-557c9b75-7c5c-4a4e-b837-f39f42a95c01.png)
