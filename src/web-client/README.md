# Running 

Set up 

To run your Angular frontend over HTTPS locally, you need to set up a self-signed SSL certificate and configure Angular CLI to serve the app over HTTPS.

Here’s a step-by-step guide on how to do this:

### 1. **Generate a Self-Signed SSL Certificate**
To serve your Angular app over HTTPS, you'll need a certificate. If you don't already have one, you can generate a self-signed certificate using OpenSSL.

#### On Windows (using Git Bash or Command Prompt):
1. Open **Git Bash** or **Command Prompt**.
2. Run the following commands to generate the SSL certificate and key files:

```bash
openssl genpkey -algorithm RSA -out ssl/private.key
openssl req -new -key ssl/private.key -out ssl/csr.csr
openssl x509 -req -in ssl/csr.csr -signkey ssl/private.key -out ssl/certificate.crt
```

This will generate:
- `ssl/private.key` (the private key)
- `ssl/certificate.crt` (the certificate)

#### On macOS or Linux:
1. Open a terminal.
2. Run the following commands to generate the SSL certificate and key files:

```bash
mkdir -p ssl
openssl genpkey -algorithm RSA -out ssl/private.key
openssl req -new -key ssl/private.key -out ssl/csr.csr
openssl x509 -req -in ssl/csr.csr -signkey ssl/private.key -out ssl/certificate.crt
```

### 2. **Configure Angular to Use the SSL Certificate**
1. Place the `certificate.crt` and `private.key` files in your Angular project folder (e.g., in a folder called `ssl`).
2. Open your `angular.json` file.
3. Find the `serve` options under the `architect` configuration of your Angular application.
4. Modify the `serve` section to include HTTPS configuration:

```json
"options": {
  "ssl": true,
  "sslKey": "ssl/private.key",
  "sslCert": "ssl/certificate.crt",
  "port": 4200,  // Make sure the port is correct
  "host": "localhost"
}
```

### 3. **Run the Angular App Over HTTPS**
Now that you've configured Angular CLI to use SSL, you can start your Angular app with HTTPS:

1. Run the following command in your terminal:

```bash
ng serve --ssl true --ssl-key ssl/private.key --ssl-cert ssl/certificate.crt
```

This will serve your Angular app over HTTPS on `https://localhost:4200`.

### 4. **Trust the Self-Signed Certificate (Optional)**
Since you’re using a self-signed certificate, your browser might show a warning about the certificate being untrusted. You can either ignore this warning (for development purposes) or manually trust the certificate in your system:

- **On Windows**: Double-click the certificate file (`certificate.crt`) and install it into your trusted root certificates store.
- **On macOS**: Double-click the certificate file and add it to the keychain as a trusted root certificate.

### 5. **Verify the Setup**
Once your Angular app is running, navigate to `https://localhost:4200` in your browser. You should be able to access your Angular app over HTTPS.

### Troubleshooting:
- **CORS Issues**: If you face CORS issues while communicating with your .NET API, ensure that your API is configured to allow HTTPS and handle cookies appropriately for cross-origin requests.
- **Certificate Warnings**: If you see certificate warnings in your browser, it's because the certificate is self-signed. You can ignore it for development purposes.

Let me know if you need any further clarification!


Then run `npm start`

Issues:

- Use cmd prompt in admin mode and install `choco install openssl`