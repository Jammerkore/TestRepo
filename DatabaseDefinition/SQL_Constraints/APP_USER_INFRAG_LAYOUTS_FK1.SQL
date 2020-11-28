/* Add foreign key constraints to table "INFRAGISTICS_LAYOUTS".                               */
alter table "INFRAGISTICS_LAYOUTS"
	add constraint "APP_USER_INFRAG_LAYOUTS_FK1" foreign key (
		"USER_RID")
	 references "APPLICATION_USER" (
		"USER_RID")  

go
