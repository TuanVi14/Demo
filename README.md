Cách để người khác clone và chạy demo project này
3 bước chính là đủ:

git clone + vào folder

git clone <repo-url>
cd Demo-main (chỗ chứa Demo.csproj)
cài thư viện & build

dotnet restore
dotnet build
nếu lỗi package thì dotnet restore lại, hoặc kiểm tra internet/nuget source.
chạy ứng dụng

dotnet run
Mở trình duyệt:
https://localhost:xxxx (xem console output để biết port)
hoặc http://localhost:5112 tùy config
vào trang:
-/
/Customers (CRUD customer)
/Account/Register / /Account/Login
🗄️ Cấu hình DB cần có
Project dùng SQL Server LocalDB (có thể thay bằng SQL Server).

appsettings.json ConnectionStrings:DefaultConnection
Nếu không có LocalDB, dùng:
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=(localdb)\\mssqllocaldb;Database=DemoDb;Trusted_Connection=True;"
hoặc cập nhật Program.cs.
Chạy app tự Apply migration + tạo bảng (có sẵn code db.Database.Migrate();).
👤 Tài khoản test
admin mặc định:
username: admin
password: 123
role: Admin
user mới:
đăng ký /Account/Register (role user)
đăng nhập /Account/Login
🔁 Lưu ý khi chạy trên máy khác
Build lại nếu target .NET 8/10 chưa cài.
Nếu dùng Visual Studio:
mở Demo.sln, set project Demo làm startup, F5.
Nếu dùng VS Code:
mở terminal dotnet run.
Cấu hình launch.json không bắt buộc.
Kiểm tra log nếu lỗi migration (có thể delete DB khi cần).
✅ Tóm lại
“Người khác chỉ cần clone + restore + build + run + vào URL”.
Project đã có sẵn API + MVC + DB migration, nên chỉ việc chạy và test (Customers CRUD, Products, Users, Agents...).
