alter table "USER_ASSORTMENT"
	add constraint "SG_USER_ASSRT_SELECTED_FK1" foreign key (
		"SG_RID")
	 references "STORE_GROUP" (
		"SG_RID")  

go
