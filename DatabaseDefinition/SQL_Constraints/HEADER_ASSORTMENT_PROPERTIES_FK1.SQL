/* Add foreign key constraints to table "ASSORTMENT_PROPERTIES".                                  */
alter table "ASSORTMENT_PROPERTIES"
	add constraint "HEADER_ASSORTMENT_PROPERTIES_FK1" foreign key (
		"HDR_RID")
	 references "HEADER" (
		"HDR_RID")  

go
