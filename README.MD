Hub and Spoke pattern

---

### **1. Core Case Management Spoke**
**Purpose:** Handles the central concept of cases and their direct relationships.  
**Entities:**
- `Case`
- `CasePerson`
- `CaseUser`
- `CaseTag`
- `CaseEvidence`
- `CaseCrimeScene`
- `CaseDocument`
- `Report`

---

### **2. Entity Management Spoke**
**Purpose:** Groups all tangible and relational entities involved in cases.  
**Entities:**
- `Person`
- `Vehicle`
- `Property`
- `Location`
- `IncidentPerson`
- `CrimeScenePerson`
- `PropertyPerson`
- `VehiclePerson`

---

### **3. Crime Scene and Evidence Management Spoke**
**Purpose:** Manages data related to crime scenes, evidence, and documentation.  
**Entities:**
- `CrimeScene`
- `CrimeSceneEvidence`
- `Evidence`
- `Document`
- `Statement`
- `StatementUser`

---

### **4. Administrative Management Spoke**
**Purpose:** Handles administrative and organizational aspects.  
**Entities:**
- `Department`
- `User`
- `Role`
- `UserRole`
- `Tag`

---

### **Rationale for Grouping**
- **Core Case Management**: Focuses purely on cases and their direct relationships, ensuring separation of the primary domain from supporting entities.
- **Entity Management**: Groups all participants and physical objects associated with cases.
- **Crime Scene and Evidence Management**: Clusters entities specific to investigations and documentation.
- **Administrative Management**: Keeps organizational and metadata entities separate.

This structure ensures you have **4 spokes** that effectively cover all entities while maintaining clear and logical groupings.



# Standards 

## Params

- Don't use `Id` Naming - instead name the specific Id it is like `userId` in any param as it can cause confusion down the road.

## Methods

- Async methods always append with `async` so `SendEmail` becomes -> `SendEmailAsync` to distinguish between async and sync operations

## Return types

- For methods that need validation use Class or tuple to encapsulate the return possibles like success and errors etc


## API

Methods:

- `POST` - return `Created()`
- `GET`  - return `Ok(json)`
- `PATCH` - return `NoContent()`
- `DELETE` - return `NoContent()`

Fetching data from DB:

- Dose not exist - `NotFound(Item not found)` - Item replace with the resource that is not found

Updating an entry

- Invalid data or shape - `BadRequest(Errors)`

Posting data:

- return thr ID of the created item -> `Created({id = id})`

### Date

- throughout the app use `UTC now` time

### Authentication

- Follow JWT
- Use role based Authentication
- Mark controllers with authorize and methods with roles when needed and anonyms when needed 

### Url format

use dashed case - `labResults` into -> `lab-results`

### Error / Bad request responses

Use a standard response format across all bad request responses and error responses so api is easy to use and understand 
as an expected error response format will be expected

These `ErrorDetail` array to explain what went wrong or is wrong with either the request or endpoint, this would be sent across with a status code response.

### App settings 

In the application never read directly from IConfig, instead make a class that describes it - and use data annotations for validation on start. 

```cs
public class JWTOptions
{
    [Required]
    public string Issuer { get; set; } = default!;

    [Required]
    [MinLength(1)]
    public string[] Audiences { get; set; } = default!;

    [Required]
    public string Key { get; set; } = default!;

    [Range(1, int.MaxValue, ErrorMessage = "ExpiresInMinutes must be greater than 0.")]
    public int ExpiresInMinutes { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "RefreshTokenExpiriesInMinutes must be greater than 0.")]
    public int RefreshTokenExpiriesInMinutes { get; set; }
}

builder.Services.AddOptions<JWTOptions>()
    .Bind(builder.Configuration.GetSection("Jwt"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

    IOptions<JWTOptions> JWTOptions private readonly JWTOptions _JWTOptions = JWTOptions.Value;
```

### Todo when you don't have problems to solve:
refactor to use Error Detail throughout the API 
- First do it in the stores
- Refactor the endpoint return types to return those
