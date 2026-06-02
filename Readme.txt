ThreadifyLab>dotnet publish -c Release -r linux-x64 --self-contained false -o ./publish/linux-x64


sudo systemctl stop threadifylab
rm -r -f /var/www/threadifylab.com/html/*
sudo unzip html.zip -d /var/www/threadifylab.com/html/
sudo chown -R www-data:www-data /var/www/threadifylab.com/html/wwwroot/images
sudo chmod -R 755 /var/www/threadifylab.com/html/wwwroot/images
sudo systemctl start threadifylab
sudo nginx -t
sudo systemctl reload nginx

