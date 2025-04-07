set up `mkcert -install` as admin

then gen `mkcert localhost 127.0.0.1 ::1`

then `ng serve --ssl --ssl-key localhost+2-key.pem --ssl-cert localhost+2.pem` or `npm start`

when opening browser you should not get warning