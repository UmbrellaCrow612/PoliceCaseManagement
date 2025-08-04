set up `mkcert -install` as admin

then gen `mkcert localhost 127.0.0.1 ::1`

then `ng serve --ssl --ssl-key localhost+2-key.pem --ssl-cert localhost+2.pem` or `npm start`

when opening browser you should not get warning

there is a docker image but for local dev use above steps and not docker image

so generate the certs and run `npm start`

# Style guide

## Code

Front end:

- Types that are only ever used two times and are just at most 2-3 fields long and just inline type them
- Any type that has more fields or is used more than 3 times must have its own type
- For services method if you pass it a object that has at most 2 fields don't pass it as a object but pass them as named params
- For anywhere where you reference or pass a objects ID name if fully for example `getUserById(userId)`
- Use angular built in forms library and use as much validation as you can - disable buttons based on validity of data
  and any method that sends data to backend must first check the validity of the input form before processing for example

```js
if (this.form.valid) {
  // Do logic knowing it is safe
}
```

- Handle error codes that are send from the backend correctly and do specific actions based on them
- `NOTE`: Use UTC time when sending time data to backend so we use both utc on backend and frontend `toISOString` for date objects being sent
- Sync reactive form validators preface with `Validator` like `Validator_containsOnlyNumeric` - any none domain specific logic put it on `control.ts` else 
make a specific folder within the business domain folder validators for example `cases/validators/...`
- Async reactive form validators use `UniqueEvidenceReferenceNumberAsyncValidator` as a ref
- Use `dialogs` for basic linking, editing and creating of items - refer to how case action is created through a dialog
- For backend enums refer to `CaseRoleNames` for how to handle them properly in a nice and standard way
- Methods that run when somthing is clicked or related to ui interactions name convetion is `handleSearchClicked` format  
