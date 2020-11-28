/* Add the remaining keys, constraints and indexes for the table "FORECAST_VERSION".          */
alter table "FORECAST_VERSION" add constraint "FORECAST_VERSION_DESC" unique (
	"DESCRIPTION")  


go
