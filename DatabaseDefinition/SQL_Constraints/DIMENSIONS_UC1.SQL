/* Add the remaining keys, constraints and indexes for the table "DIMENSIONS".                */
alter table "DIMENSIONS" add constraint "DIMENSIONS_UC1" unique (
	"SIZE_CODE_SECONDARY")  


go
