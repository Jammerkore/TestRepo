/* BEGIN TT#618 - RMatelic - Allocation Override - Add Attribute Sets (#35)  */
alter table "HEADER_STORE_GRADE_VALUES"
	add constraint "HEADER_HDR_STR_GRD_VAL_FK1" foreign key (
		"HDR_RID")
	 references "HEADER" (
		"HDR_RID")  
go
