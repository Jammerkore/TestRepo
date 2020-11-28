alter table "ASSORTMENT_PROPERTIES"
	add constraint "STORE_GROUP_ASSORTMENT_PROPERTIES_FK1" foreign key (
		"SG_RID")
	 references "STORE_GROUP" (
		"SG_RID")  

go
