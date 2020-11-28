alter table "USER_ASSORTMENT_BASIS"
	add constraint "USER_USER_ASSORTMENT_BASIS_FK1" foreign key (
		"USER_RID")
	 references "USER_ASSORTMENT" (
		"USER_RID")  

go
