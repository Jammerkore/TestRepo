/* Add foreign key constraints to table "ASSORTMENT_PROPERTIES_STORE_GRADE".                      */
alter table "ASSORTMENT_PROPERTIES_STORE_GRADE"
	add constraint "HEADER_ASSORTMENT_PROP_STORE_GRADE_FK1" foreign key (
		"HDR_RID")
	 references "ASSORTMENT_PROPERTIES" (
		"HDR_RID")  

go
