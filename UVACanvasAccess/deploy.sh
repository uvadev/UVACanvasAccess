pkg=`find . -type f -iname "UVACanvasAccess.*.nupkg"`
curl -vX PUT -u "$1:$2" -F package="@$pkg" https://nuget.pkg.github.com/uvadev/

