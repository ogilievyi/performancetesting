-- Dumping structure for таблиця demo.Address
CREATE TABLE  "Address" (
	"Region" NVARCHAR(max) NULL DEFAULT NULL,
	"City" NVARCHAR(max) NULL DEFAULT NULL,
	"CityType" NVARCHAR(max) NULL DEFAULT NULL ,
	"District" NVARCHAR(max) NULL DEFAULT NULL ,
	"Street" NVARCHAR(max) NULL DEFAULT NULL ,
	"BuildNumber" NVARCHAR(max) NULL DEFAULT NULL ,
	"Index" NVARCHAR(max) NULL DEFAULT NULL 
);

CREATE TABLE  "Log" (
	"Message" NVARCHAR(max) NULL DEFAULT NULL ,
	"Exception" NVARCHAR(max) NULL DEFAULT NULL ,
	"Level" NVARCHAR(max) NULL DEFAULT NULL 
);

CREATE TABLE  "Statistic" (
	"User" NVARCHAR(max) NULL DEFAULT NULL ,
	"Count" NVARCHAR(max) NULL DEFAULT NULL ,
	"SearchText" NVARCHAR(max) NULL DEFAULT NULL 
);

