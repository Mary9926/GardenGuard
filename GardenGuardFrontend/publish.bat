set varsion=%1
docker build --file GardenGuardFrontend/Server/Dockerfile --tag 244501/gardenguard:%varsion% .
docker push 244501/gardenguard:%varsion%