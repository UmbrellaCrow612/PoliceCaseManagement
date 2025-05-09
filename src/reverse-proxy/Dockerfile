# Use a lightweight Nginx image
FROM nginx:stable-alpine

# Remove the default Nginx configuration file
RUN rm /etc/nginx/conf.d/default.conf

# Copy your custom Nginx configuration file to the Nginx configuration directory
COPY nginx.conf /etc/nginx/nginx.conf

# Expose port 80 (which Nginx listens on by default as per our config)
EXPOSE 80

