
# Covercy Home Assignment
This is a Home Assignment for job at Covercy.

I was tasked with creating a small application, which generates api-keys and uses these api-keys to generate JWT tokens on demand.

## Main information
The assignment saves data to SQLite-based database, using Entity framework.
I had written it using VS 2022, so, to run it i suggest using it.
The app is based on localhost:7046.

The development took about 3 hours and 20 minutes - from 18:00 to 21:20 on 07.10.24. However, this time does not include some last minute changes I made after I started writing this README at about 22:00. These changes mainly concern the authenticateRequest method and all the request mechanics that were changed to work correctly with it.

## Methods
### 1. Create API key
   To create API key you should send a POST request to server, which contains following parameters:

       int: userId
       string: permissions

   
   Given an authenticated user request (which contains the userId) and a list of required permissions (basically any string, parsing was not required for the assignment. Howether, i suggest using a JSON, or a string separated by some symbols, for example, a comma ','), generates a new api key for the user.

### 2. Use API key
   To use API key you should send a POST request to server in that form: "/authenticate", which contains following parameters:

       string: ApiKey

   Given a request with a valid api-key, generates a new signed JWT token for the user with the pre-defined set of permissions and update the "last usage" date of that token.

### 3. Revoke API key
   To revoke API key you should send a DELETE request to server , which contains following parameters:

       int: userId
       string: ApiKey

   Given an authenticated user request and an API token, revokes that token.

### 4. Get tokens
   To get JWT token you should send a get request to server , which contains following parameters:

       int: userId

   Given an authenticated user request, gets all the tokens of that user with their status and last recently used date.

## Example of work
Here is an example of it's work:
API token, as presented by GET request:

    {
    "UserId":  2,
    "ApiKey":  "112b4cf8-e8c0-4b9f-825c-dc746d244a18",
    "Status":  "Valid",
    "LastUsage":  "2024-10-07T22:58:31.2810043"
    }

Resulting JWT token:   `eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJwZXJtaXNzaW9ucyI6InBvc3Qsd3JpdGUsZGVsZXRlLHVwZGF0ZSxkaWVfb2xkIiwiVXNlcklkIjoiMiJ9.Bk7U5XeDDSl0X9N1Ra1yRM8OcpXLvYp1oUSntvQhGf8`

Decoded JWT:

    {"header": {"alg":"http://www.w3.org/2001/04/xmldsig-more#hmac-sha256","typ":"JWT"},
    "payload":"permissions":"post,write,delete,update,die_old","UserId":"2"}}

