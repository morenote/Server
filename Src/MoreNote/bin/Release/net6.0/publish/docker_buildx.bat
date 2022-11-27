docker buildx create --name lilqbuilder
docker buildx use lilqbuilder
docker buildx ls
docker buildx inspect lilqbuilder --bootstrap
docker buildx ls
docker buildx build --platform linux/amd64,linux/arm64  -t registry.cn-hangzhou.aliyuncs.com/hyfree/morenote:0.0.47  --push .