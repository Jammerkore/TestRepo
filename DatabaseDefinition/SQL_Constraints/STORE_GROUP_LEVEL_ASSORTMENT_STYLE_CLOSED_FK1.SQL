alter table "ASSORTMENT_STYLE_CLOSED"
	add constraint "STORE_GROUP_LEVEL_ASSORTMENT_STYLE_CLOSED_FK1" foreign key (
		"SGL_RID")
	 references "STORE_GROUP_LEVEL" (
		"SGL_RID")  

go
