/* Add foreign key constraints to table "HEADER_STORE_GRADE".                                 */
alter table "HEADER_STORE_GRADE"
	add constraint "HEADER_HEADER_STORE_GRADE_FK1" foreign key (
		"HDR_RID")
	 references "HEADER" (
		"HDR_RID")  

go
