# Pet Store

## Introduction
This is a demonstration of how to create APIs using a microservices architecture. The endpoints are taken from https://petstore.swagger.io/ and the APIs has been implemented as Web APIs. The implemented endpoints are working and you can use Postman for example to test them. Links to Docker images has also been provided below to be able to run this without worrying about any dependencies.

## Implementation Details
I implemented this in Visual Studio 2019 using **.NET Core 3.0.** and tested it with Postman. It has been done as one solution with two API projects, one for Orders and one for Users. I implemented most of the specified endpoints using standard Web APIs. See API Endpoints header for more details.

To quickly get it up and running, I used Entity Framework Core and took advantage of their in-memory-database. The two projects are completely separated and have their own in-memory-database. Be aware that this did create some limitations as it's not a proper relational database and I couldn't change things like keys and indexes. With the user for example, where I assume the username is unique, I had to add some minor handling outside of the database to ensure this condition was satisfied.

Each project is its own microservice with its own Docker image. I used docker-compose for running the two containers at the same time, which you can run directly from Visual Studio editor. The Docker images are also uploaded to Docker Hub. See Docker header for more details.

## Implementation Considerations
* In a production environment you would obviously use a more appropriate database. This was just to show that the APIs are working.
* Docker-compose is convenient for a small demo like this, but in a real production scenario, Kubernetes seems to be the best way for orchestrating. I had a quick look but decided that it was too big of a task to learn Kubernetes for this challenge.
* I did not look at authentication.
* Sounds like you can use Swashbuckle to add some Swagger integration into the actual project. Would be interesting to explore, but I did not look at it any further.
* Although I can run both containers at the same time with docker-compose, it's not completely seamless as they are using different ports. For example: http://localhost:64783/users and http://localhost:64788/store/orders. To solve this and to make it look like they are running on the same port, it seems like I need to use an API gateway. For .NET core, the one that seems to be recommended is called Ocelot. I did not implement any API gateway.

## Docker
I connected my Docker Hub repository with my GitHub repository, so that whenever a change is made in GitHub, the Docker images are re-built automatically. I added both my images (latestOrders and latestUsers) to the same Docker repository. In hindsight it looks like the convention is to use different repositories so that each only has different versions of the same image, but for this demo you can find both images in:
https://hub.docker.com/r/thrberglund/petstore.

You can run the images in a container individually by using the following commands:
```
docker run -it --rm  -p 64783:80 thrberglund/petstore:latestUsers
```
or
```
docker run -it --rm  -p 64788:80 thrberglund/petstore:latestOrders
```

You can also run both of them at the same time by using docker-compose. If you want to use the simple test file I provided, simply run the following command from the directory where you put the docker-compose.simpletest.yml file:
```
docker-compose -f docker-compose.simpletest.yml up
```

## API Endpoints
Most endpoints for User and Order has been implemented as specified in https://petstore.swagger.io/. 

Please note that I have also implemented 2 additional endpoints, which I'm aware are technically not part of the spec. One is for getting all orders and the other is for getting all users. This was simply added for convenience.

The following endpoints has been implemented:
```
GET 	/store/orders
GET 	/store/order/{orderId}
DELETE 	/store/order/{orderId}
POST 	/store/order
```
```
GET 	/users
GET 	/user/{username}
PUT 	/user/{username}
DELETE 	/user/{username}
POST 	/user
POST 	/user/createWithArray
POST 	/user/createWithList