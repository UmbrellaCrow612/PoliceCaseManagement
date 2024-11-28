To minimize the number of spokes while maximizing the entities they hold, we can focus on grouping related entities based on common purposes or domains. Here's a breakdown of how to achieve this:

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