/* Add the remaining keys, constraints and indexes for the table "HEADER_BULK_COLOR_SIZE".    */
create unique index "HEADER_BULK_COLOR_SIZE_AK1" on "HEADER_BULK_COLOR_SIZE" (
	"HDR_BC_RID",
	"SIZE_CODE_RID")  


go
