/* Add the remaining keys, constraints and indexes for the table "SIZES".                     */
alter table "SIZES" add constraint "SIZES_UC1" unique (
	"SIZE_CODE_PRIMARY")  


go
