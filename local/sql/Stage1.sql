USE demo;
INSERT INTO "Address2" ("Region", "City", "CityType", "District", "Street", "BuildNumber", "Index") 
SELECT "Region", "City", "CityType", "District", "Street", "BuildNumber", "Index" FROM Address;

INSERT INTO dbo.AddressIndex (id, Address)
SELECT Id, CONCAT (Region, ' ', City, ' ', CityType, ' ', [Index], ' ', District, ' ', Street, ' ', BuildNumber) FROM dbo.Address2;

CREATE UNIQUE INDEX Index_Id ON AddressIndex(Id);
CREATE FULLTEXT CATALOG ft AS DEFAULT;
CREATE FULLTEXT INDEX ON AddressIndex(Address)
   KEY INDEX Index_Id
   WITH STOPLIST = SYSTEM;


SELECT SERVERPROPERTY('IsFullTextInstalled')