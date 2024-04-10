# Use the official Node.js LTS image as the base image
FROM node:lts-alpine AS builder

# Set the working directory to /app
WORKDIR /app

# Copy package.json and package-lock.json files into the image
COPY package*.json ./

# Debugging: List files in the current directory
RUN ls -al

# Install dependencies
RUN npm install

# Copy the rest of the application files
COPY . .

# Build the Angular application
RUN npm run build

# Create the final image with a minimal NGINX image
FROM nginx:alpine

# Copy the built files from the previous stage into the NGINX server's html directory
COPY --from=builder /app/dist/* /usr/share/nginx/html/

# Expose port 80
EXPOSE 80
