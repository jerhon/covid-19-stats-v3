# Grab the latest swagger definition
Invoke-WebRequest -Uri "https://localhost:5001/swagger/hs-covid-19-v1/swagger.json" -OutFile ".\src\api\hs-covid-19-v1.json"

# Run NSwag to update the API client.
pushd
cd src\api\
docker run --rm -v "${PWD}/src/api:/local" openapitools/openapi-generator-cli generate `
    -i ./local/hs-covid-19-v1.json `
    -g typescript-axios `
    -o /local/hs-covid-19
popd
