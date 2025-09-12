# SMS Worker

Background worker that listens for SMS events and sends SMS messages.

---

## Running Outside of Docker

To run the SMS worker outside of Docker:

1. Set it as a **single startup project** or include it in a **multi-launch setup**.
2. Ensure the configuration connects to RabbitMQ correctly.
   Most likely, RabbitMQ will still be running inside Docker.

Example `appsettings.json` configuration:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "RabbitMqSettings": {
    "Host": "localhost",
    "Username": "user",
    "Password": "StrongPassword!"
  }
}
```

---

## Running Inside Docker Orchestration

If the worker is running as part of a Docker orchestration setup, update the RabbitMQ connection to use the internal Docker hostname.

Example `appsettings.json` configuration:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "RabbitMqSettings": {
    "Host": "broker",
    "Username": "user",
    "Password": "StrongPassword!"
  }
}
```
