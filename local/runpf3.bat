docker run --name pf3 -d --cpus="0.5" -p 1031:80 --link mssql -e ConnectionStrings__db="Server=mssql;Database=demo;User Id=sa;Password=Password+;MultipleActiveResultSets=True;Connection Timeout=30;Max Pool Size=1000;Pooling=true;"   performancetest:3
docker run --name pf3Fast -d --cpus="0.5" -p 1032:80 --link mssql -e ConnectionStrings__db="Server=mssql;Database=demo;User Id=sa;Password=Password+;MultipleActiveResultSets=True;Connection Timeout=30;Max Pool Size=1000;Pooling=true;"   performancetest:3