@echo off
echo Insert a new item
curl -X POST "http://localhost:62341/api/inventory/commands" -H  "accept: application/json" -H  "Content-Type: application/json-patch+json" -d "{  \"inventoryItemId\": \"8496c0b7-92f5-4d1a-a018-981d2999a280\",  \"name\": \"First Item\"}"
ping localhost -n 1 >NUL
curl -X GET "http://localhost:62341/api/inventory" -H  "accept: text/plain"
echo .
echo Rename with NewName
curl -X PUT "http://localhost:62341/api/inventory/commands/rename" -H  "accept: application/json" -H  "Content-Type: application/json-patch+json" -d "{  \"inventoryItemId\": \"8496c0b7-92f5-4d1a-a018-981d2999a280\",  \"newName\": \"New Name\",  \"originalVersion\": 0}"
ping localhost -n 1 >NUL
curl -X GET "http://localhost:62341/api/inventory" -H  "accept: text/plain"
echo .
echo Add 7 parts
curl -X PUT "http://localhost:62341/api/inventory/commands/checkin" -H  "accept: application/json" -H  "Content-Type: application/json-patch+json" -d "{  \"inventoryItemId\": \"8496c0b7-92f5-4d1a-a018-981d2999a280\",  \"count\": 7,  \"originalVersion\": 1}"
ping localhost -n 1 >NUL
curl -X GET "http://localhost:62341/api/inventory/8496c0b7-92f5-4d1a-a018-981d2999a280" -H  "accept: text/plain"
echo .
echo Removed 2 parts
curl -X PUT "http://localhost:62341/api/inventory/commands/remove" -H  "accept: application/json" -H  "Content-Type: application/json-patch+json" -d "{  \"inventoryItemId\": \"8496c0b7-92f5-4d1a-a018-981d2999a280\",  \"count\": 2,  \"originalVersion\": 2}"
ping localhost -n 1 >NUL
curl -X GET "http://localhost:62341/api/inventory/8496c0b7-92f5-4d1a-a018-981d2999a280" -H  "accept: text/plain"
echo .
pause