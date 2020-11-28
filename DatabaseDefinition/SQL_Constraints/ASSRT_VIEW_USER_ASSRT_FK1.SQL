alter table "USER_ASSORTMENT"
	add constraint "ASSRT_VIEW_USER_ASSRT_FK1" foreign key (
		"VIEW_RID")
	 references "ASSORTMENT_VIEW" (
		"VIEW_RID")  

go
