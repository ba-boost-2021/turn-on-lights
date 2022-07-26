FROM nginx:1.21.1-alpine
RUN rm -rf /usr/share/nginx/html/*
COPY . /usr/share/nginx/html
RUN rm /usr/share/nginx/html/default.conf
COPY default.conf /etc/nginx/conf.d/default.conf