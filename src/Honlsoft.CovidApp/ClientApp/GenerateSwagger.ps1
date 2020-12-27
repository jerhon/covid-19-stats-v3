# Grab the latest swagger definition
Invoke-WebRequest -Uri "https://localhost:5001/swagger/hs-covid-19-v1/swagger.json" -OutFile ".\src\api\hs-covid-19-v1.json"

# Run NSwag to update the API client.
pushd
cd src\api\
nswag run /runtime:NetCore31
popd
