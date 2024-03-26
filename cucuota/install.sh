#! /bin/bash

package_names=("dotnet8" "unzip" "nodejs" "git")
LogFile=""
QuoteFile=""
BannedFile=""

echo "Searching packages..."

for package_name in "${package_names[@]}"; do
  if dpkg -l | grep -q "^ii  $package_name "; then
    echo "$package_name is installed."
  else
    echo "$package_name is not installed."
    echo "Installing package $package_name"
    #apt install $package_name
  fi
done

echo "Downloading projects"

#git clone https://github.com/kaelthasmanu/cucuota
#git clone https://github.com/kaelthasmanu/cucuota-frontend

#cd cucuota/cucuota

all_file_exist=true
while [ $all_file_exist ]
do
  read -p "Enter the path of the squid log example -> /var/log/squid/access.log :" LogFile
  read -p "Enter the path of the file banned users example -> /etc/squid/banned.txt: " QuoteFile
  read -p "Enter the path of the file quota example -> /etc/squid/quota.txt : " BannedFile
  if [ -e $LogFile -a -e $QuoteFile -a -e $BannedFile ]; then
    all_file_exist=false
  else
    echo "Files no exist check again..."
  fi
done

config_file="./appsettings.json"

if [ -e $config_file ]; then
    echo "File exists."
    jq --arg new_logfilepath "LogFile" \
       --arg new_filepathquota "QuoteFile" \
       --arg new_filepathbanned "BannedFile" \
       '.WorkingFiles.LogFile = $new_logfilepath |
        .WorkingFiles.QuoteFile = $new_filepathquota |
        .WorkingFiles.BannedFile = $new_filepathbanned' \
        "$config_file" > temp.json && mv temp.json "$config_file"
else
    echo "Missing settings file, does not exist."
fi

#cd ..
#dotnet build 
#dotnet cucuota/bin/Debug/net7.0/cucuota.dll

#cd ..
#cd CuCuotaWeb
