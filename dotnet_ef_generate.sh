dotnet ef dbcontext scaffold \
  "Host=localhost;Port=5632;Database=hrms;Username=postgres;Password=ddgpemhf" \
  Npgsql.EntityFrameworkCore.PostgreSQL \
  --schema core \
  --schema attendance \
  --schema payroll \
  --schema rules \
  --output-dir Models \
  --context-dir Data \
  --context HrmsDbContext \
  --no-onconfiguring \
  --use-database-names