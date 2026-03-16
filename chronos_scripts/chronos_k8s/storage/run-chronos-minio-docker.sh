# Standalone MinIO
docker run -d \
  --name minio \
  -p 9000:9000 \
  -p 9001:9001 \
  -v minio-data:/data \
  -e MINIO_ROOT_USER=super_user \
  -e MINIO_ROOT_PASSWORD=Y2hyb25vc19taW5pb19zZWNyZXRfa2V5Cg== \
  quay.io/minio/minio server /data --console-address ":9001"
