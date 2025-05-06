# Stage 1: Building the app

FROM node:18-alpine as builder

WORKDIR /app

COPY package.json package-lock.json ./

RUN npm ci

COPY . .

RUN npm run build

# Stage 2: Serve the application using Nginx

FROM nginx:stable-alpine

COPY --from=builder /app/dist/front-end/browser /usr/share/nginx/html

COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
