/* Add foreign key constraints to table "ASSORTMENT_STORE_SUMMARY".                                   */
alter table "ASSORTMENT_STORE_SUMMARY"
	add constraint "HEADER_ASSORTMENT_STORE_SUMMARY_FK1" foreign key (
		"HDR_RID")
	 references "HEADER" (
		"HDR_RID")  

go
