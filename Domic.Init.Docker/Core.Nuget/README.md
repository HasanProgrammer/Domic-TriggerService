for create [Username] & [Password] in linux server for nginx :

docker run --rm -it -v $(pwd)/data/certs:/certs httpd:2.4-alpine htpasswd -cb /certs/.htpasswd [username] [password]