**CodebridgeTestApp** is an application that uses **SQL Server** as the database to store dog-related data. The project provides the following API endpoints:

- `GET /ping`: Returns "Dogshouseservice.Version1.0.1"
- `GET /dogs?pageNumber=3&pageSize=10&attribute=weight&order=desc`: Retrieves a list of all dogs with support for pagination, filtering, and sorting.
- `POST /dog`: Creates a new dog with data validation.
Body Example
{
  "name": "string",
  "color": "string",
  "tail_length": 2147483647,
  "weight": 2147483647
}
