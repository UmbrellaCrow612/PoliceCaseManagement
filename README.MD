# Currently working on identity 

## Features so far
- username password sign in 
- MFA Time based One Time passwaord using google authenticator 
- MFA SMS and Email codes
- One time passcodes
- JWT auth strategy
- Device Fingerptining
- Refresh tokens
- Magic link auth
- Rabbit mq and mass tranit events 
- GRPC communication between api's using proto files

## Mobile app
- Scan QR codes and show OTP or register TOTP codes like google authenticator

# Biometrics - part of indetity
## Features so far

- Endpoint to compare two faces

# Cases

- Create cases
- Assign users to them
- Add actions to them
- Upload attachments to them
- Fine grained permission per user

# Evidence
- Upload pieces of evidences to the system to be assigned to cases and more - amazon s3.

# Running

- Run the frontend - inctructions in `web-client` project
- Run `docker-compose.yml` file in root here with `docker-compose up -d` to run redis and db
- Open the `sln` file and select start up project to be the `multi` launch
