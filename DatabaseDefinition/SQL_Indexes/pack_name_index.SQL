/* Add the remaining keys, constraints and indexes for the table "HEADER_PACK".               */
create unique index "pack_name_index" on "HEADER_PACK" (
	"HDR_RID",
	"HDR_PACK_NAME")  


go
