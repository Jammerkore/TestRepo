/* Add foreign key constraints to table "ASSORTMENT_PROPERTIES_BASIS".                            */
alter table "ASSORTMENT_PROPERTIES_BASIS"
	add constraint "HEADER_ASSORTMENT_PROP_BASIS_FK1" foreign key (
		"HDR_RID")
	 references "ASSORTMENT_PROPERTIES" (
		"HDR_RID")  

go
