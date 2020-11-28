alter table "ASSORTMENT_PROPERTIES"
	add constraint "APPLICATION_USER_ASSORTMENT_PROPERTIES_FK1" foreign key (
		"USER_RID")
	 references "APPLICATION_USER" (
		"USER_RID")  
go
