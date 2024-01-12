MiniApiProject

MiniApiProject is a simple ASP.NET Core Minimal API project that provides endpoints to manage information about people, their interests, and links. The project uses Entity Framework Core to interact with a SQL Server database.


Clone the Repository

   bash
   git clone https://github.com/dinozoff1981/MiniApiProject.git

ER Diagram


![MiniApi](https://github.com/dinozoff1981/MiniApiProject/assets/51277761/e93aa9b1-e05d-453a-90d0-c441fc8ecbf2)



UML Diagram

![Uml](https://github.com/dinozoff1981/MiniApiProject/assets/51277761/c560cbe8-b1e5-44d5-8749-8e4bec9ac07f)


Endpoints
1. Get All People
Endpoint: /api/getall
Method: GET
Description: Retrieves all people along with their phones, interests, and links.

2. Get Single Person
Endpoint: /api/person/{personId}
Method: GET
Description: Retrieves information about a specific person, including phones, interests, and links.

3. Get Interests of a Person
Endpoint: /api/getinterest/{personId}
Method: GET
Description: Retrieves the interests of a specific person.

4. Get Links of a Person
Endpoint: /api/getlink/{personId}
Method: GET
Description: Retrieves the links associated with a specific person.

5. Create a New Person
Endpoint: /api/createperson
Method: POST
Description: Creates a new person along with associated phones, interests, and links.

6. Add Links to Interest
Endpoint: /api/addlinks/{personId}/{interestId}
Method: POST
Description: Adds new links to a specific person's interest.


7. Add Interests to a Person
Endpoint: /api/addinterest/{personId}
Method: POST
Description: Adds new interests to a specific person.

8. Remove Person and Related Data
Endpoint: /api/removeperson/{personId}
Method: DELETE
Description: Deletes all information of a specific person, including phones, interests, and links.
