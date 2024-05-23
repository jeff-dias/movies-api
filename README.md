## Context
This project is a web API built using .NET 7.0 and clean architecture.

The application represents a Cinema. We want to manage the showtimes of the cinema, getting some data from a RPC Provider.

The current features are:
- Create showtimes.
- Reserve seats.
- Buy seats.

## Solution
The solution was created following the clean architecture approach. Therefore, here you'll find 5 app layers with specific responsabilities and interactions. Being more visual, here is an image that represents the clean architecture layers and its interactions:
![Clean architecture diagram](img/clean-arch.png)
- [Clean architecture ref](https://medium.com/codenx/code-in-clean-vs-traditional-layered-architecture-net-31c4cad8f815)

Some patterns and technologies that you will find in this project:
- Clean architecture;
- CQRS by Mediator;
- AutoMapper;
- gRPC Client API;
- REST API;
- Rich domains;
- Entity Framework;
- Business Rules;
- Exception Filters;
- Redis;

### Domain
The domain was created using the rich domains. A pattern that aims to solve the problem of anemic domain models. Where the domain is more a data transfer object than a real domain, delegating the domain validations to other layers like `XptoService` or `Business Layers`.
In a rich domain the class is created with specific constructors and, normally, the sets are private to "force" the correct use of the constructors. Also, the class will have internal functions that will help in centralize the data changes and validations also inside the domain model.
Also, in the Domain, we can find the repository interfaces refering the entities.
- [Rich Domain ref](https://www.linkedin.com/pulse/anemic-model-micha%C5%82-%C4%87wi%C4%99ka%C5%82a/)

### Infra.Data
The Data layer was created using repositories. A pattern that aims to decouple the context and the rest of the application. Using repository pattern with a good ORM (EF Core, in our case) make easier to change the app database if necessary. Also, it becomes the interactions with the context standardized and easier to understand and replicate.
Also, in the Data layer, we can find the context and entities configuration used by Entity Framework Core to interact with the database (in-memory in our scenario).
- [Repository pattern ref](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-design#the-repository-pattern)

### Application
In the App layer were created the services. A pattern that aims to decouple the controllers from the repositories. It's here where the most magic happens because the App layer will be the translator between the Data layer and the Web API.
For this data transfer we use the DTO (Data Transfer Object) pattern. This pattern is helpful to have classes totally focused in the Web API layer, showing different values than the domain class (e.g. a formatted date). The DTO pattern is used to avoid couple the Domain layer with the Web API also and is pretty common in clean architectures.
Still in the services, we'll use Mediator, to handle the queries and commands, following the CQRS pattern that stands for Command and Query Responsibility Segregation. It separates read and update operations for a data store. With this pattern applied we can, in some moment, divide our database to have one for `read` and other for `write` without big impact in our application. This pattern also avoids giant service classes and make the code readbility easier.
Also in the service layer, we'll find the `External Movies Service` which doesn't use CQRS, but a gRPC Client, because the data is from an external API and not from the database. The gRPC is used to generate API connections (Clients and Servers) by protobuf files. It's very useful when we have services of different stacks but, even in the same stack, it acelerate the development of APIs. Still here in our `movie service`, we have a Redis to cache the gRPC server responses because it was created to return timeout sometimes to simulate a real external server. Redis is a NoSQL database pretty helpful when we need to store information by key and value. It is mainly used as a cache database but, in some cases, can be used as storage database also.
- [DTO ref](https://medium.com/@orcunyilmazoy/the-dto-pattern-data-transfer-objects-8146b262636e)
- [CQRS ref](https://medium.com/@matii96/cqrs-vs-classical-n-layer-application-1ecb74188d14)
- [gRPC ref](https://learn.microsoft.com/en-us/dotnet/architecture/cloud-native/grpc)
- [Redis ref](https://redis.io/learn/howtos/quick-start)

### Inversion of Control
Also, I developed the inversion of control container and the dependency injection.
The Inversion of Control container is a design pattern created to decouple the core application from the consumers. In our case, the consumer is the Web API layer and it can only reference the IoC project to have access to our application and its services.
The IoC container is also responsible to centralize our application dependency injection. The DI is a principle that promotes the decoupling of high-level modules from low-level modules by introducing abstractions. The main idea behind DI is that higher-level modules should not depend on lower-level modules. This principle is required in clean architectures.
- [Dependency Injection ref](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection)

### Web API
The Web API layer is the most simple possible. It exposes the routes to we interact with our Cinema app and use the IoC container to have access to the service interfaces. It also uses the app DTOs as contract to be presented on Swagger.
Swagger is a web interface that help us to interact with our Web API routes.

### Unit tests
The unit tests were developed for the core projects (domain, application and infra data). The xUnit library was used to implement the unit tests. 
- [xUnit ref](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-dotnet-test)

### Integration tests
To present another test approach, I used the Postman to interact with my API and do validations about the expected responses.
The Postman collection can be found in the project root also and used in local tests.
- [Postman collections ref](https://learning.postman.com/docs/collections/collections-overview/)

## Testing the solution
To test the solution, you can use the Docker Compose. Just run the command below in the root folder of the solution:

```powershell
docker-compose up --remove-orphans
```

After that, you can use the Postman collection to test the web api. The collection is located in the root folder.
Finally, you can use the command below to clean your environment:

```powershell
docker-compose down
```

### Docker compose throubleshoting
If you have any trouble to run the docker compose, this is probably because I developed using a Mac. So, you need to review the volume path that I used for my local machine.
Also, if you still have problems with this approach, no worries! Just run the solution that was configured to launch in the same HTTPS port that the docker compose is configured to use (7143). So, you can test the solution using the Postman collection.

### Postman collection - Scenario
1. Get all the auditoriums created on the system launch, by seed, and set the first auditorium id as a variable.
2. Get all the movies provided by the gRPC API server and set the first movie id as a variable.
3. Create a new showtime using the auditorium and the movie ids. Also, set the showtime id created as a variable.
4. Get the showtime created using the showtime id variable. Also, set the showtime seats as variables.
5. Try to create a new ticket using the showtime id and seats not contiguous. The system should return a bad request.
6. Try to create a new ticket using the showtiem id and seats contiguous. The system should return a created status. Also, set the ticket id as a variable.
7. Try to create a new ticket using the showtime id and seats already taken. The system should return a bad request.
8. Get the showtime previous created to assure that the seats appears as reserved
9. Try to cancel the ticket using the ticket id variable. The system should return OK.
10. Try to get the ticket using the ticket id variable. The system should return not found.
11. Get the showtime previous created to assure that the seats appears as available again.
12. Try to reserve the seats again using the showtime id and seats contiguous. The system should return a created status. Also, set the ticket id as a variable.
13. Get the showtime previous created to assure that the seats appears as reserved again.
14. Get the ticket using the ticket id variable. The system should return OK. Also, the ticket should appear as not paid.
15. Try to pay the ticket using the ticket id variable. The system should return OK.
16. Get the ticket using the ticket id variable. The system should return OK. Also, the ticket should appear as paid.
17. Try to reserve the seats again using the showtime id and seats contiguous. The system should return a bad request.

## Conclusion
I hope this project could be helpful for you understand more about the concepts here presented. During the development of this application I didn't have the whole view about some pattterns and techs and exercise them was very useful to learn more and achieve a new level in my .NET and architecture comprehension.
Follow me for more and see you around!
